using System.Runtime.Serialization;

namespace MultiplayerMod.Core.Serialization.Surrogates;

internal class KAnimFileSurrogate : ISerializationSurrogate, ISurrogateType
{
    public Type Type => typeof(KAnimFile);

    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
        var anim = (KAnimFile) obj;
        info.AddValue("name", anim.name);
    }

    public object SetObjectData(
        object obj,
        SerializationInfo info,
        StreamingContext context,
        ISurrogateSelector selector
    )
    {
        var name = info.GetString("name");
        return Assets.GetAnim(name);
    }

}
