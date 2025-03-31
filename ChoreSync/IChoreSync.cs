namespace MultiplayerMod.ChoreSync
{
    internal interface IChoreSync
    {
        Type SyncType { get; }              // Typ der Synchronisierung
        Type StateMachineType { get; }      // Typ der zugeh�rigen StateMachine

        void Client(StateMachine instance);  // F�hrt die clientseitige Synchronisierung aus
        void Server(StateMachine instance);  // F�hrt die serverseitige Synchronisierung aus
    }
}
