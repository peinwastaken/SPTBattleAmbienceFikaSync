using SPTBattleAmbience.Data;
using SPTBattleAmbience.Helpers;
using SPTBattleAmbienceFikaSync.Controllers;
using SPTBattleAmbienceFikaSync.Data;
using SPTBattleAmbienceFikaSync.Packets;
using UnityEngine;

namespace SPTBattleAmbienceFikaSync.Helpers
{
    public static class AmbiencePacketHelper
    {
        public static void OnReceivePacket(BattleSoundPacket packet)
        {
            CoopBattleAmbienceController instance = CoopBattleAmbienceController.Instance;
            
            Plugin.Logger.LogWarning("RECEIVED PACKET!!!");
            BattleSoundPacketStruct packetData = packet.PacketData;
            
            string category = packetData.Category;
            string soundType = packetData.SoundType;
            string fileName = packetData.FileName;
            Vector3 position = packetData.Position;
            int rolloff = packetData.Rolloff;
            float volume = packetData.Volume;
            
            Plugin.Logger.LogInfo($"PLAYING AMBIENCE AT POSITION: {position}");

            if (!instance.TryGetClip(category, soundType, fileName, out AudioClip clip))
            {
                Plugin.Logger.LogWarning($"Clip not found! {fileName}");
                return;
            }
            
            Plugin.Logger.LogInfo($"{category} | {soundType} | {fileName} | {position} | {rolloff} | {volume}");

            ClipInfo clipInfo = new ClipInfo()
            {
                AudioClip = clip,
                Category = category,
                SoundType = soundType,
                ClipName = fileName
            };

            AmbientHelper.PlayAmbienceSound(position, clipInfo, rolloff, volume);
        }
    }
}
