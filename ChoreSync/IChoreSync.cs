namespace MultiplayerMod.ChoreSync
{
    internal interface IChoreSync
    {
        Type SyncType { get; }              // Typ der Synchronisierung
        Type StateMachineType { get; }      // Typ der zugehörigen StateMachine

        void Client(StateMachine instance);  // Führt die clientseitige Synchronisierung aus
        void Server(StateMachine instance);  // Führt die serverseitige Synchronisierung aus
    }
}
