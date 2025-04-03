using System.Diagnostics;

namespace EIV_Common.Coroutines;

/// <summary>
/// Custom Coroutine for C#
/// </summary>
public class CoroutineWorkerCustom
{
    static object _tmpRef;
    static Func<IEnumerator<double>, IEnumerator<double>> ReplacementFunction;
    static CoroutineWorkerCustom instance;
    /// <summary>
    /// Getting the Instance of <see cref="CoroutineWorkerCustom"/>
    /// </summary>
    public static CoroutineWorkerCustom Instance
    {
        get
        {
            if (instance == null)
                return instance = new();
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    List<double> Delays = [];
    List<Coroutine> CustomCoroutines = [];

    Thread UpdateThread;
    Stopwatch Watch;
    /// <summary>
    /// Update rate, how many times should it run. (1 / value). Default is 60. (More -> Faster operation)
    /// </summary>
    public static double UpdateRate { get; set; } = 1f / 60f;  // "fps"
    double prevTime = 0f;
    double accumulator = 0f;
    double TotalTime = 0f;
    /// <summary>
    /// Can the update be paused?
    /// </summary>
    public bool PauseUpdate = false;
    private readonly Mutex _mutex = new();
    internal CoroutineWorkerCustom()
    {
        Instance = this;
        Watch = Stopwatch.StartNew();
        prevTime = Watch.ElapsedMilliseconds / 1000f;
        UpdateThread = new(Update)
        {
            IsBackground = true
        };
        UpdateThread.Start();
    }

    #region Needed stuff for running
    /// <summary>
    /// Starting the coroutine
    /// </summary>
    public void Start()
    {
        Debug.Log("CoroutineWorkerCustom started!");
    }

    /// <summary>
    /// Quitting the coroutine.
    /// </summary>
    public void Quit()
    {
        Kill();
        UpdateThread.Interrupt();
        Debug.Log("Was run until " + TotalTime);
    }

    void Update()
    {
        while (UpdateThread.ThreadState == System.Threading.ThreadState.Background)
        {
            if (PauseUpdate)
                continue;
            double currTime = Watch.ElapsedMilliseconds / 1000f;
            accumulator += currTime - prevTime;
            prevTime = currTime;

            if (accumulator > UpdateRate)
            {
                accumulator -= UpdateRate;
                CustomCorUpdate(UpdateRate);
                TotalTime += UpdateRate;
            }
        }
    }


    void CustomCorUpdate(double deltaTime)
    {
        Kill();
        if (_mutex.WaitOne(1))
        {
            for (int i = 0; i < Instance.CustomCoroutines.Count; i++)
            {
                Coroutine item = Instance.CustomCoroutines[i];
                //Debug.Log($"index: {i} Delay: {Instance.Delays[i]} DeltaTime: {deltaTime} CoroutineItem: {item}");
                if (Instance.Delays[i] > 0f)
                    Instance.Delays[i] -= deltaTime;
                if (Instance.Delays[i] <= 0f)
                {
                    CoroutineWork(ref item, i);
                }
                if (double.IsNaN(Instance.Delays[i]))
                {
                    if (ReplacementFunction != null)
                    {
                        item.Enumerator = ReplacementFunction(item.Enumerator);
                        CoroutineWork(ref item, i);
                        ReplacementFunction = null;
                    }
                }
                Instance.CustomCoroutines[i] = item;
            }
            _mutex.ReleaseMutex();
        }
        Kill();
    }

    private void Kill()
    {
        if (_mutex.WaitOne(1))
        {
            for (int i = 0; i < Instance.CustomCoroutines.Count; i++)
            {
                Coroutine item = Instance.CustomCoroutines[i];
                if (item.ShouldKill)
                {
                    Instance.CustomCoroutines.Remove(item);
                    Instance.Delays.RemoveAt(i);
                }
            }
            _mutex.ReleaseMutex();
        }
    }

    private void CoroutineWork(ref Coroutine coroutine, int index)
    {
        if (coroutine.ShouldKill)
            return;
        if (coroutine.ShouldPause)
            return;
        if (coroutine.IsSuccess)
            return;
        coroutine.IsRunning = true;
        if (!MoveNext(ref coroutine, index))
        {
            coroutine.IsRunning = false;
            coroutine.IsSuccess = true;
            coroutine.ShouldKill = true;
        }
    }

    private bool MoveNext(ref Coroutine coroutine, int index)
    {
        bool result = coroutine.Enumerator.MoveNext();
        Instance.Delays[index] = coroutine.Enumerator.Current;
        return result;
    }
    #endregion
    #region Coroutine Creators
    /// <summary>
    /// Creating a Coroutine
    /// </summary>
    /// <param name="objects"></param>
    /// <param name="type"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
    public static CoroutineHandle StartCoroutine(IEnumerator<double> objects, CoroutineType type = CoroutineType.Custom, string tag = "")
    {
        Coroutine coroutine = new(objects, type, tag);
        Debug.Log(coroutine.ToString());
        Instance.CustomCoroutines.Add(coroutine);
        Instance.Delays.Add(0);
        return coroutine;
    }
    /// <summary>
    /// Creating a Coroutine that is Delayed
    /// </summary>
    /// <param name="timeSpan"></param>
    /// <param name="action"></param>
    /// <param name="type"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
    public static CoroutineHandle CallDelayed(TimeSpan timeSpan, System.Action action, CoroutineType type = CoroutineType.Custom, string tag = "")
    {
        return StartCoroutine(_DelayedCall(timeSpan, action), type, tag);
    }

    /// <summary>
    /// Creating a Coroutine that is calling every update
    /// </summary>
    /// <param name="action"></param>
    /// <param name="type"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
    public static CoroutineHandle CallContinuously(System.Action action, CoroutineType type = CoroutineType.Custom, string tag = "")
    {
        return StartCoroutine(_CallContinuously(TimeSpan.Zero, action), type, tag);
    }

    /// <summary>
    /// Creating a Coroutine that is called ever <paramref name="timeSpan"/> time.
    /// </summary>
    /// <param name="timeSpan"></param>
    /// <param name="action"></param>
    /// <param name="type"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
    public static CoroutineHandle CallPeriodically(TimeSpan timeSpan, System.Action action, CoroutineType type = CoroutineType.Custom, string tag = "")
    {
        return StartCoroutine(_CallContinuously(timeSpan, action), type, tag);
    }
    #endregion
    #region Static Helpers
    private static IEnumerator<double> ReturnTmpRefForRepFunc(IEnumerator<double> coptr)
    {
        if (_tmpRef == null)
            return _Empty();
        if (_tmpRef is IEnumerator<double> that && that != null)
            return that;
        return _Empty();
    }

    private static IEnumerator<double> WaitUntilFalseHelper(IEnumerator<double> coptr)
    {
        return _StartWhenDone(_tmpRef as Func<bool>, true, coptr);
    }

    private static IEnumerator<double> WaitUntilTrueHelper(IEnumerator<double> coptr)
    {
        return _StartWhenDone(_tmpRef as Func<bool>, false, coptr);
    }

    private static IEnumerator<double> StartAfterCoroutineHelper(IEnumerator<double> coptr)
    {
        return _StartWhenDone((Coroutine?) _tmpRef, coptr);
    }
    #endregion
    #region Static IEnumerators
    private static IEnumerator<double> _StartWhenDone(Func<bool> evaluatorFunc, bool continueOn, IEnumerator<double> pausedProc)
    {
        if (evaluatorFunc == null)
            yield break;
        while (evaluatorFunc() == continueOn)
        {
            yield return double.NegativeInfinity;
        }
        _tmpRef = pausedProc;
        ReplacementFunction = new Func<IEnumerator<double>, IEnumerator<double>>(ReturnTmpRefForRepFunc);
        yield return float.NaN;
    }

    private static IEnumerator<double> _StartWhenDone(Coroutine? coroutine, IEnumerator<double> pausedProc)
    {
        if (!coroutine.HasValue)
            yield break;
        coroutine = Instance.CustomCoroutines.Where(x => x.Equals(coroutine)).FirstOrDefault();
        while (coroutine.Value.IsSuccess != true)
        {
            coroutine = Instance.CustomCoroutines.Where(x => x.Equals(coroutine)).FirstOrDefault();
            yield return double.NegativeInfinity;
        }
        _tmpRef = pausedProc;
        ReplacementFunction = new Func<IEnumerator<double>, IEnumerator<double>>(ReturnTmpRefForRepFunc);
        yield return double.NaN;
    }
    private static IEnumerator<double> _Empty()
    {
        yield return 0f;
    }
    private static IEnumerator<double> _DelayedCall(TimeSpan timeSpan, System.Action action)
    {
        yield return timeSpan.TotalSeconds;
        action();
    }
    private static IEnumerator<double> _CallContinuously(TimeSpan timeSpan, System.Action action)
    {
        while (true)
        {
            yield return timeSpan.TotalSeconds;
            action();
        }
    }
    #endregion
    #region Static Doubles
    /// <summary>
    /// Wait until the <paramref name="evaluatorFunc"/> is false
    /// </summary>
    /// <param name="evaluatorFunc"></param>
    /// <returns></returns>
    public static double WaitUntilFalse(Func<bool> evaluatorFunc)
    {
        if (evaluatorFunc == null || evaluatorFunc() == false)
        {
            return 0;
        }
        _tmpRef = evaluatorFunc;
        ReplacementFunction = new Func<IEnumerator<double>, IEnumerator<double>>(WaitUntilFalseHelper);
        return double.NaN;
    }

    /// <summary>
    /// Wait until the <paramref name="evaluatorFunc"/> is true
    /// </summary>
    /// <param name="evaluatorFunc"></param>
    /// <returns></returns>
    public static double WaitUntilTrue(Func<bool> evaluatorFunc)
    {
        if (evaluatorFunc == null || evaluatorFunc())
        {
            return 0;
        }
        _tmpRef = evaluatorFunc;
        ReplacementFunction = new Func<IEnumerator<double>, IEnumerator<double>>(WaitUntilTrueHelper);
        return double.NaN;
    }

    /// <summary>
    /// Wait until the <paramref name="coroutine"/> is Success
    /// </summary>
    /// <param name="coroutine"></param>
    /// <returns></returns>
    public static double StartAfterCoroutine(CoroutineHandle coroutine)
    {
        Coroutine cor = Instance.CustomCoroutines.FirstOrDefault(x => coroutine.Equals((CoroutineHandle) x));
        if (cor.IsSuccess)
        {
            return 0;
        }
        _tmpRef = cor;
        ReplacementFunction = new Func<IEnumerator<double>, IEnumerator<double>>(StartAfterCoroutineHelper);
        return double.NaN;
    }
    #endregion
    #region Statis funcs

    /// <summary>
    /// Kill these <paramref name="coroutines"/> coroutines
    /// </summary>
    /// <param name="coroutines"></param>
    public static void KillCoroutines(IList<CoroutineHandle> coroutines)
    {
        for (int i = 0; i < coroutines.Count; i++)
        {
            KillCoroutineInstance(coroutines[i]);
        }
    }

    /// <summary>
    /// Kill specified Coroutine
    /// </summary>
    /// <param name="coroutine"></param>
    public static void KillCoroutineInstance(CoroutineHandle coroutine)
    {
        Instance.KillCoroutine(coroutine);
    }

    /// <summary>
    /// Kill specified Coroutine
    /// </summary>
    /// <param name="coroutine"></param>
    public void KillCoroutine(CoroutineHandle coroutine)
    {
        int index = Instance.CustomCoroutines.FindIndex(x => coroutine.Equals((CoroutineHandle) x));
        if (index == -1)
        {
            Debug.Log("No Coroutine!");
            return;
        }
        if (_mutex.WaitOne(1))
        {
            Coroutine cor = Instance.CustomCoroutines[index];
            cor.ShouldKill = true;
            Instance.CustomCoroutines[index] = cor;
            _mutex.ReleaseMutex();
        }
    }

    /// <summary>
    /// Kill specified Coroutine that has Tag as <paramref name="tag"/>
    /// </summary>
    /// <param name="tag"></param>
    public static void KillCoroutineTagInstance(string tag)
    {
        Instance.KillCoroutineTag(tag);
    }

    /// <summary>
    /// Kill specified Coroutine that has Tag as <paramref name="tag"/>
    /// </summary>
    /// <param name="tag"></param>
    public void KillCoroutineTag(string tag)
    {
        if (_mutex.WaitOne(1))
        {
            var cors = Instance.CustomCoroutines.Where(x => x.Tag == tag).Select(x => (CoroutineHandle) x).ToList();
            KillCoroutines(cors);
            _mutex.ReleaseMutex();
        }
    }

    /// <summary>
    /// Un/Pause selected Coroutine
    /// </summary>
    /// <param name="coroutine"></param>
    public static void PauseCoroutineInstance(CoroutineHandle coroutine)
    {
        Instance.PauseCoroutine(coroutine);
    }

    /// <summary>
    /// Un/Pause selected Coroutine
    /// </summary>
    /// <param name="coroutine"></param>
    public void PauseCoroutine(CoroutineHandle coroutine)
    {
        int index = Instance.CustomCoroutines.FindIndex(x => coroutine.Equals((CoroutineHandle) x));
        if (index == -1)
        {
            Debug.Log("No Coroutine!");
            return;
        }
        if (_mutex.WaitOne(1))
        {
            Coroutine cor = Instance.CustomCoroutines[index];
            cor.ShouldPause = !cor.ShouldPause;
            Instance.CustomCoroutines[index] = cor;
            _mutex.ReleaseMutex();
        }
    }

    /// <summary>
    /// Checks if the <paramref name="coroutine"/> exists
    /// </summary>
    /// <param name="coroutine"></param>
    /// <returns></returns>
    public static bool IsCoroutineExists(CoroutineHandle coroutine)
    {
        return Instance.CustomCoroutines.FindIndex(x => coroutine.Equals((CoroutineHandle) x)) != -1;
    }

    /// <summary>
    /// Checks if the <paramref name="coroutine"/> running
    /// </summary>
    /// <param name="coroutine"></param>
    /// <returns></returns>
    public static bool IsCoroutineRunningInstance(CoroutineHandle coroutine)
    {
        return Instance.IsCoroutineRunning(coroutine);
    }

    /// <summary>
    /// Checks if the <paramref name="coroutine"/> running
    /// </summary>
    /// <param name="coroutine"></param>
    /// <returns></returns>
    public bool IsCoroutineRunning(CoroutineHandle coroutine)
    {
        int index = Instance.CustomCoroutines.FindIndex(x => coroutine.Equals((CoroutineHandle) x));
        if (index == -1)
        {
            Debug.Log("No Coroutine!");
            return false;
        }
        bool isRunning = false;
        if (_mutex.WaitOne(1))
        {
            Coroutine cor = Instance.CustomCoroutines[index];
            isRunning = cor.IsRunning;
            _mutex.ReleaseMutex();
        }
        return isRunning;
    }

    /// <summary>
    /// Checks if the <paramref name="coroutine"/> success
    /// </summary>
    /// <param name="coroutine"></param>
    /// <returns></returns>
    public static bool IsCoroutineSuccessInstance(CoroutineHandle coroutine)
    {
        return Instance.IsCoroutineSuccess(coroutine);
    }

    /// <summary>
    /// Checks if the <paramref name="coroutine"/> success
    /// </summary>
    /// <param name="coroutine"></param>
    /// <returns></returns>
    public bool IsCoroutineSuccess(CoroutineHandle coroutine)
    {
        bool sucess = false;
        if (_mutex.WaitOne(1))
        {
            int index = Instance.CustomCoroutines.FindIndex(x => coroutine.Equals((CoroutineHandle) x));
            if (index == -1)
            {
                Debug.Log("No Coroutine!");
                return false;
            }
            Coroutine cor = Instance.CustomCoroutines[index];
            sucess = cor.IsSuccess;
            _mutex.ReleaseMutex();
        }
        return sucess;
    }

    /// <summary>
    /// Checks if has Any Coroutine
    /// </summary>
    /// <returns></returns>
    public static bool HasAnyCoroutines()
    {
        return Instance.CustomCoroutines.Count != 0;
    }

    #endregion
}
