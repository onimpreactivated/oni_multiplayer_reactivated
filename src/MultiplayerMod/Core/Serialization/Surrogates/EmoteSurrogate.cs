using Klei.AI;
using System.Runtime.Serialization;

namespace MultiplayerMod.Core.Serialization.Surrogates;

internal class EmoteSurrogate : ISerializationSurrogate, ISurrogateType
{
    public Type Type => typeof(Emote);

    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
        var emote = (Emote) obj;
        info.AddValue("id", emote.Id);
    }

    public object SetObjectData(
        object obj,
        SerializationInfo info,
        StreamingContext context,
        ISurrogateSelector selector
    )
    {
        var id = info.GetString("id");
        return Db.Get().Emotes.Minion.Get(id);
    }
}
