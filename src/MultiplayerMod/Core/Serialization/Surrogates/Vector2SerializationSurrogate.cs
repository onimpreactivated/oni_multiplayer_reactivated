using System.Runtime.Serialization;
using UnityEngine;

namespace MultiplayerMod.Core.Serialization.Surrogates;

/// <summary>
/// Unity Vector 2 for Serialization
/// </summary>
public class Vector2SerializationSurrogate : ISerializationSurrogate, ISurrogateType
{
    /// <inheritdoc/>
    public Type Type => typeof(Vector2);

    /// <inheritdoc/>
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
        var vector = (Vector2) obj;
        info.AddValue("x", vector.x);
        info.AddValue("y", vector.y);
    }

    /// <inheritdoc/>
    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
        var vector = (Vector2) obj;
        vector.x = info.GetSingle("x");
        vector.y = info.GetSingle("y");
        return vector;
    }
}
