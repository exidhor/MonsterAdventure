using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    [Serializable]
    public class Region
    {
        public int left;
        public int bot;
        public int width;
        public int height;

        public int right
        {
            get { return left + width; }
        }

        public int top
        {
            get { return bot + height; }
        }

        public Vector2 center
        {
            get
            {
                return new Vector2(left + (float)width / 2f,
                                   bot + (float)height / 2f);
            }
        }

        private List<BiomeType> _biomeTypes;

        public static explicit operator Rect(Region region)
        {
            return new Rect(region.left, region.bot, region.width, region.height);
        }

        public Region()
        {
            left = 0;
            bot = 0;
            width = 0;
            height = 0;

            _biomeTypes = new List<BiomeType>();
        }

        public Region(Rect rect)
        {
            left = (int) rect.x;
            bot = (int) rect.y;
            width = (int) rect.width;
            height = (int) rect.height;

            _biomeTypes = new List<BiomeType>();
        }

        public void AddBiomeType(BiomeType biomeType)
        {
            if (!_biomeTypes.Contains(biomeType))
                _biomeTypes.Add(biomeType);
        }

        public int GetBiomeTypeCount()
        {
            return _biomeTypes.Count;
        }

        public BiomeType GetFirstBiomeType()
        {
            if (_biomeTypes.Count == 0)
            {
                return BiomeType.None;
            }

            return _biomeTypes[0];
        }

        public bool HasBiomeType(BiomeType type)
        {
            return _biomeTypes.Contains(type);
        }
    }
}