using EIV_Common.Coroutines;
using HarmonyLib;
using KMod;
using MultiplayerMod.Core;
using MultiplayerMod.Events;
using System.Reflection;

namespace MultiplayerMod;

/// <summary>
/// Mod Entry Point
/// </summary>
public class ModLoad : UserMod2
{
    /// <summary>
    /// Loading Events, Commands, Initializing
    /// </summary>
    /// <param name="harmony">The Harmony class</param>
    public override void OnLoad(Harmony harmony)
    {
        base.OnLoad(harmony);
        var version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
        Debug.Log("MultiplayerMod Version: " + version);
        Debug.Log("MultiplayerMod GetPatchedMethods: " + harmony.GetPatchedMethods().Count());
        foreach (var item in harmony.GetPatchedMethods())
        {
            Debug.Log("Patched: " + item.FullDescription());
        }
        CoroutineWorkerCustom.Instance.Start();
        EventManager.LoadMain(assembly);
        MultiplayerManager.Instance.Init(harmony);
    }
}
