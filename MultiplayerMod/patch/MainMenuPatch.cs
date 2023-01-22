﻿using System;
using System.Reflection;
using HarmonyLib;
using MultiplayerMod.multiplayer;
using MultiplayerMod.steam;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MultiplayerMod.patch
{
    [HarmonyPatch(typeof(MainMenu), "OnPrefabInit")]
    public class MainMenuPatch
    {
        public static void Prefix(MainMenu __instance)
        {
            var multiplayerGameObject = new GameObject();
            multiplayerGameObject.AddComponent<Server>();
            multiplayerGameObject.AddComponent<Client>();
            multiplayerGameObject.AddComponent<ClientActions>();
            multiplayerGameObject.AddComponent<ServerActions>();
            Object.DontDestroyOnLoad(multiplayerGameObject);
            __instance.AddButton("Load MultiPlayer", true,
                () =>
                {
                    Server.HostServerAfterInit();
                    __instance.InvokePrivate("LoadGame");
                });
            __instance.AddButton("Start MultiPlayer", false,
                () =>
                {
                    Server.HostServerAfterInit();

                    __instance.InvokePrivate("NewGame");
                });
            __instance.AddButton("Join MultiPlayer", false, Client.ShowJoinToFriend);
        }
    }

    static class Extension
    {
        public static void AddButton(this MainMenu mainMenu, string text, bool topStyle, System.Action action)
        {
            var type = typeof(MainMenu);

            var buttonInfoType = type.GetNestedType("ButtonInfo", BindingFlags.NonPublic | BindingFlags.Instance);

            mainMenu.InvokePrivate("MakeButton",
                Activator.CreateInstance(
                    buttonInfoType,
                    new LocString(text),
                    action,
                    22,
                    mainMenu.GetPrivateField<ColorStyleSetting>(topStyle ? "topButtonStyle" : "normalButtonStyle"))
            );
        }
    }
}