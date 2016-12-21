using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class BaseManager : MonoBehaviour
    {
        private Dictionary<BiomeType, List<Base>> _basesPerBiome;

        public void Construct()
        {
            _basesPerBiome = new Dictionary<BiomeType, List<Base>>();

            foreach (BiomeType biomeType in Enum.GetValues(typeof(BiomeType)))
            {
                _basesPerBiome.Add(biomeType, new List<Base>());
            }
        }
    }
}
