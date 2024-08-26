using System;

namespace MultiplayerMod.Multiplayer.Chores;

public interface IChoreConfigurer {
    public Type ChoreType { get; }
}
