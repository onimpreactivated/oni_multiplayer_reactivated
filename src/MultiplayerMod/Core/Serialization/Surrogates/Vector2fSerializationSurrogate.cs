using System.Runtime.Serialization;

namespace MultiplayerMod.Core.Serialization.Surrogates;

/// <summary>
/// Assembly-Csharp-FirstPass Vector 2 for Serialization
/// </summary>
public class Vector2fSerializationSurrogate : ISerializationSurrogate, ISurrogateType
{
    /// <inheritdoc/>
    public Type Type => typeof(Vector2f);

    /// <inheritdoc/>
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
        var vector = (Vector2f) obj;
        info.AddValue("x", vector.x);
        info.AddValue("y", vector.y);
    }

    /// <inheritdoc/>
    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
        var vector = (Vector2f) obj;
        vector.x = info.GetSingle("x");
        vector.y = info.GetSingle("y");
        return vector;
    }
}
