using BepInEx;
using BepInEx.Logging;
using SPTBattleAmbienceFikaSync.Patches;

namespace SPTBattleAmbienceFikaSync
{
    [BepInPlugin("com.pein.sptbattleambiencefikasync", "SPTBattleAmbienceFikaSync", "1.0.0")]
    [BepInDependency("com.fika.core", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.pein.battleambience", BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin
    {
        public static new ManualLogSource Logger;
        
        private void Awake()
        {
            Logger = base.Logger;
            
            new BattleAmbienceGameStartedPatch().Enable();
            new PlayAmbiencePatch().Enable();
        }
    }
}