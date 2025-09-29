using Comfort.Common;
using Fika.Core.Coop.Components;
using Fika.Core.Coop.Players;
using Fika.Core.Coop.Utils;
using Fika.Core.Networking;
using HarmonyLib;
using LiteNetLib;
using SPT.Reflection.Patching;
using SPTBattleAmbience.Config;
using SPTBattleAmbience.Data;
using SPTBattleAmbience.Helpers;
using SPTBattleAmbience.Managers;
using SPTBattleAmbience.Models;
using SPTBattleAmbience.Utility;
using SPTBattleAmbienceFikaSync.Data;
using SPTBattleAmbienceFikaSync.Packets;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SPTBattleAmbienceFikaSync.Patches
{
    public class PlayAmbiencePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(AmbientHelper), nameof(AmbientHelper.PlayAmbienceSound));
        }
        
        [PatchPostfix]
        private static void PatchPostfix(
            AmbientHelper __instance,
            Vector3 position,
            ClipInfo clipInfo,
            int rolloff,
            float volume
        )
        {
            if (FikaBackendUtils.IsServer)
            {
                FikaServer server = Singleton<FikaServer>.Instance;

                CoopHandler.TryGetCoopHandler(out CoopHandler coopHandler);
                List<CoopPlayer> players = coopHandler.HumanPlayers;
                Vector3 averagePosition = Vector3.zero;
                int playerCount = players.Count;
                
                foreach (CoopPlayer player in players)
                {
                    averagePosition += player.Position;
                }
                
                averagePosition /= playerCount;

                MapConfigBase mapConfig = ConfigHelper.GetMapConfig(GameWorldHelper.GetCurrentMapId());
                Vector3 mapCenter = mapConfig.MapCenter.Value;
                float mapRadius = mapConfig.MapRadius.Value;
                Vector3 dirToPlayerFlat = (averagePosition - mapCenter).WithY(0).normalized;
                Vector3 soundPosition = mapCenter + dirToPlayerFlat * mapRadius;
                
                BattleSoundPacket packet = new BattleSoundPacket()
                {
                    PacketData = new BattleSoundPacketStruct()
                    {
                        Category = clipInfo.Category,
                        SoundType = clipInfo.SoundType,
                        FileName = clipInfo.ClipName,
                        Position = soundPosition,
                        Rolloff = rolloff,
                        Volume = volume
                    }
                };
                
                server.SendDataToAll(ref packet, DeliveryMethod.Unreliable);
            }
        }
    }
}
