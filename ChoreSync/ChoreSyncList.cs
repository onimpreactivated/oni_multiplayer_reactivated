using MultiplayerMod.ChoreSync.Syncs;
using MultiplayerMod.ChoreSync.Syncs.MonitorSyncs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MultiplayerMod.ChoreSync
{
    // Verwende eine statische Klasse, um die Chores zu verwalten
    internal static class ChoreSyncList
    {
        // Liste von IChoreSync-Objekten, die alle registrierten Chores enthalten
        private static readonly List<IChoreSync> _chores = new();

        // Statische Konstruktor-Methode für die Initialisierung
        static ChoreSyncList()
        {
            // Hier registrierst du alle Chores
            RegisterChores();
        }

        // Registrierungsmethode für alle Chores
        private static void RegisterChores()
        {
            //----------- New Chores Here ----------------
            Register(new IdleChoreSync());
            //Register(new MoveToSafetySync());                 
            Register(new FetchAreaChoreSync());                
            Register(new EatChoreSync());                      
            //----------- New Chores Here ----------------

            // Monitor Syncs
            Register(new IdleMonitorSync());
            Register(new SafeCellMonitorSync());
        }

        // Öffentliche Methode, um einen neuen Chore zu registrieren
        public static void Register(IChoreSync chore)
        {
            if (chore != null && !_chores.Contains(chore))
            {
                _chores.Add(chore);
            }
        }

        // Gibt den Sync für einen bestimmten ChoreType zurück
        public static IChoreSync GetSync(Type choreSyncType)
        {
            return _chores.FirstOrDefault(chore => chore.SyncType == choreSyncType);
        }

        // Gibt eine Liste der StateMachine-Typen für alle Chores zurück
        public static List<Type> GetStateMachineTypes()
        {
            return _chores.Select(chore => chore.StateMachineType).ToList();
        }

        // Gibt eine Liste der Sync-Typen für alle Chores zurück
        public static List<Type> GetSyncTypes()
        {
            return _chores.Select(chore => chore.SyncType).ToList();
        }

        // Eine Hilfsmethode, um alle Chores zu entfernen (optional)
        public static void ClearChores()
        {
            _chores.Clear();
        }

        // Eine Methode, um einen bestimmten Chore zu entfernen
        public static void RemoveChore(IChoreSync chore)
        {
            if (chore != null)
            {
                _chores.Remove(chore);
            }
        }
    }
}
