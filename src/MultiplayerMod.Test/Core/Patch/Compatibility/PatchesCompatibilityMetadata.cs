using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;

namespace MultiplayerMod.Test.Core.Patch.Compatibility;

public static class PatchesCompatibilityMetadata {

    /// <summary>
    /// Generated metadata, see <see cref="PatchesCompatibility.Generate"/>.
    /// </summary>
    [SuppressMessage("ReSharper", "ArrangeObjectCreationWhenTypeNotEvident")]
    private static readonly List<MethodMetadata> methodsMetadata = [
        new(typeof(AttackChore.States), "InitializeStates(StateMachine.BaseState&)", "6cd856c28e54e7624864c1e1b5e0788a5c01947b"),
        new(typeof(ThreatMonitor), "InitializeStates(StateMachine.BaseState&)", "e5fb426a773768edde854bd4ad8320028aa90f90"),
        new(typeof(IdleChore.States), "InitializeStates(StateMachine.BaseState&)", "1f76fc606b70389d3ae9871387aec31753e0abce"),
        new(typeof(IdleMonitor), "InitializeStates(StateMachine.BaseState&)", "fb533a4363ea6d7b9b3f5d7a2eed1bb7418fa775"),
        new(typeof(IdleStates), "InitializeStates(StateMachine.BaseState&)", "1b7443635910b683ccd9a8dd03d17d8fbe4e1c4f"),
        new(typeof(MoveToSafetyChore.States), "InitializeStates(StateMachine.BaseState&)", "c2edad05c741576db71406d7d2e97ccdb0e37186"),
        new(typeof(SafeCellMonitor), "InitializeStates(StateMachine.BaseState&)", "fb3ac7a4f21a714afc82921f811ebd4d3aa6a3e9")
    ];

    public static readonly HashAlgorithm HashAlgorithm = SHA1.Create();

    public static readonly Dictionary<Type, Dictionary<string, MethodMetadata>> Metadata;

    static PatchesCompatibilityMetadata() {
        Metadata = methodsMetadata.GroupBy(it => it.Type)
            .ToDictionary(
                it => it.Key,
                grouping => grouping.ToDictionary(it => it.Signature, it => it)
            );
    }

    public record MethodMetadata(Type Type, string Signature, string Hash);

}
