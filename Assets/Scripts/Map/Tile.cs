using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class Tile : MonoBehaviour
    {
        private BiomeType _biomeType;
        private Zone _zone;

        private void Awake()
        {
            _biomeType = BiomeType.None;
            _zone = null;
        }

        public void SetBiomeType(BiomeType biomeType)
        {
            _biomeType = biomeType;
        }

        public void SetZone(Zone zone)
        {
            _zone = zone;
        }

        public BiomeType GetBiomeType()
        {
            return _biomeType;
        }

        public Zone GetZone()
        {
            return _zone;
        }
    }
}
