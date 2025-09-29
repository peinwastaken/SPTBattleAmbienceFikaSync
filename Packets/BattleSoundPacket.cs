using Fika.Core.Networking;
using LiteNetLib.Utils;
using SPTBattleAmbienceFikaSync.Data;
using UnityEngine;

namespace SPTBattleAmbienceFikaSync.Packets
{
    public class BattleSoundPacket : INetSerializable
    {
        public BattleSoundPacketStruct PacketData;
        
        public void Serialize(NetDataWriter writer)
        {
            writer.Put(PacketData.Category);
            writer.Put(PacketData.SoundType);
            writer.Put(PacketData.FileName);
            writer.Put(PacketData.Position);
            writer.Put(PacketData.Rolloff);
            writer.Put(PacketData.Volume);
        }

        public void Deserialize(NetDataReader reader)
        {
            PacketData = new BattleSoundPacketStruct()
            {
                Category = reader.GetString(),
                SoundType = reader.GetString(),
                FileName = reader.GetString(),
                Position = reader.GetVector3(),
                Rolloff = reader.GetInt(),
                Volume = reader.GetFloat()
            };
        }
    }
}
