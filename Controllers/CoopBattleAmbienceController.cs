using SPTBattleAmbience.Helpers;
using SPTBattleAmbience.Models.Sounds;
using UnityEngine;

namespace SPTBattleAmbienceFikaSync.Controllers
{
    public class CoopBattleAmbienceController : MonoBehaviour
    {
        public static CoopBattleAmbienceController Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public bool TryGetClip(string category, string soundType, string fileName, out AudioClip clip)
        {
            clip = null;

            if (!AmbientHelper.AmbientSoundCategories.TryGetValue(category, out AmbientSoundCategory soundCategory))
            {
                Plugin.Logger.LogWarning($"Sound category not found! {category}");
                return false;
            }

            if (!soundCategory.SoundTypes.TryGetValue(soundType, out AmbientSounds ambientSounds))
            {
                Plugin.Logger.LogWarning($"Sound type not found! {soundType}");
                return false;
            }

            return ambientSounds.AudioClips.TryGetValue(fileName, out clip);
        }
    }
}
