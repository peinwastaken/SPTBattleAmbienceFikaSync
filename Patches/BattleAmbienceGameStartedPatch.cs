using Comfort.Common;
using Fika.Core.Coop.Utils;
using Fika.Core.Networking;
using HarmonyLib;
using SPT.Reflection.Patching;
using SPTBattleAmbience.Controllers;
using SPTBattleAmbience.Data;
using SPTBattleAmbience.Models;
using System.Reflection;
using SPTBattleAmbience.Patches;
using SPTBattleAmbienceFikaSync.Controllers;
using SPTBattleAmbienceFikaSync.Data;
using SPTBattleAmbienceFikaSync.Helpers;
using SPTBattleAmbienceFikaSync.Packets;
using System.Collections.Generic;
using UnityEngine;

namespace SPTBattleAmbienceFikaSync.Patches
{
    public class BattleAmbienceGameStartedPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(GameStartedPatch), "PatchPrefix");
        }

        [PatchPrefix]
        private static bool PatchPrefix()
        {
            if (FikaBackendUtils.IsServer)
            {
                Plugin.Logger.LogError("were server, its ok!");
                Singleton<FikaServer>.Instance.RegisterPacket<BattleSoundPacket>(AmbiencePacketHelper.OnReceivePacket);
                
                return true;
            }
            
            Plugin.Logger.LogError("were client, its not ok! destroy old controller");
            Singleton<FikaClient>.Instance.RegisterPacket<BattleSoundPacket>(AmbiencePacketHelper.OnReceivePacket);
            
            BattleAmbienceController controllerObject = BattleAmbienceController.Instance;
            if (controllerObject != null)
            {
                GameObject.Destroy(controllerObject.gameObject);
            }
            
            GameObject coopController = new GameObject("BattleAmbienceController");
            coopController.AddComponent<CoopBattleAmbienceController>();
            
            SPTBattleAmbience.Plugin.LoadAmbientSoundCategories();
            
            return false;
        }
    }
}
