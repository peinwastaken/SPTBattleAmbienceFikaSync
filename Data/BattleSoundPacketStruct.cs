using System.Collections.Generic;
using UnityEngine;

namespace SPTBattleAmbienceFikaSync.Data
{
    public struct BattleSoundPacketStruct
    {
        public string Category;
        public string SoundType;
        public string FileName;
        public Vector3 Position;
        public int Rolloff;
        public float Volume;
    }
}
