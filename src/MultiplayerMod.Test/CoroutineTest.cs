using EIV_Common.Coroutines;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace MultiplayerMod.Test;

internal class CoroutineTest
{
    [Test]
    public void TestWaitUntils()
    {
        // This exist here to make our test faster, running at 144 fps
        CoroutineWorkerCustom.UpdateRate = 1 / 144f;
        CoroutineWorkerCustom.Instance.Start();
        Debug.Log("AAAAAAAAAAA");
        var handle = CoroutineWorkerCustom.StartCoroutine(_CountingDown(), CoroutineType.Custom, "Test");
        Assert.IsNotNull(handle);
        Assert.That(handle.CoroutineHash, Is.Not.EqualTo(0));
        Assert.That(CoroutineWorkerCustom.IsCoroutineExists(handle), Is.EqualTo(true));
        Stopwatch stopwatch = Stopwatch.StartNew();
        var sec = TimeSpan.FromSeconds(5);
        Debug.Log("stopwatch and time stuff");
        while (!CoroutineWorkerCustom.IsCoroutineSuccessInstance(handle))
        {
            if (stopwatch.Elapsed > sec)
            {
                Assert.Fail();
            }
            // wait until test over.
        }
        stopwatch.Stop();
        Thread.Sleep(10);
        Assert.That(CoroutineWorkerCustom.IsCoroutineExists(handle), Is.EqualTo(false));
        Assert.That(CoroutineWorkerCustom.IsCoroutineSuccessInstance(handle), Is.EqualTo(false));

        CoroutineWorkerCustom.Instance.Quit();
    }


    public IEnumerator<double> _CountingDown()
    {
        byte i = byte.MaxValue;
        Debug.Log("_CountingDown");
        yield return CoroutineWorkerCustom.WaitUntilFalse(
            () =>
            {
                Debug.Log(i);
                i--;
                return i != 0;
            });
        yield return 0;
        Debug.Log("Finished");
        Debug.Log($"i: {i}");
        i = byte.MaxValue;
        yield return CoroutineWorkerCustom.WaitUntilTrue(
            () =>
            {
                Debug.Log(i);
                i--;
                return i == 0;
            });
        Debug.Log("Finished");
        Debug.Log($"i: {i}");
        yield return 0;
        yield break;
    }
}
