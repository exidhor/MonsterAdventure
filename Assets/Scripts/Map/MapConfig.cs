using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class MapConfig : MonoBehaviour
    {
        public Color blueColor;
        public Texture blueIcon;
        public float blueRatio;
        public uint blueBases;

        public Color greenColor;
        public Texture greenIcon;
        public float greenRatio;
        public uint greenBases;

        public Color whiteColor;
        public Texture whiteIcon;
        public float whiteRatio;
        public uint whiteBases;

        public Color blackColor;
        public Texture blackIcon;
        public float blackRatio;
        public uint blackBases;

        private float blackRatioNormalized;
        private float blueRatioNormalized;
        private float greenRatioNormalized;
        private float whiteRatioNormalized;

        private void Awake()
        {
            // normalize the ratio value
            float sum = blackRatio + blueRatio + greenRatio + whiteRatio;

            blackRatioNormalized = blackRatio/sum;
            blueRatioNormalized = blueRatio/sum;
            greenRatioNormalized = greenRatio/sum;
            whiteRatioNormalized = whiteRatio/sum;
        }

        public Color GetColor(BiomeType type)
        {
            switch (type)
            {
                case BiomeType.Black:
                    return blackColor;

                case BiomeType.Blue:
                    return blueColor;

                case BiomeType.Green:
                    return greenColor;

                case BiomeType.White:
                    return whiteColor;

                default:
                    return new Color(0.6f, 0.6f, 0.6f, 1f); // grey by default
            }
        }

        public BiomeType GetBiomeType(float value)
        {
            float ratio = blueRatioNormalized;

            if (value < ratio)
            {
                return BiomeType.Blue;
            }

            ratio += greenRatioNormalized;

            if (value < ratio)
            {
                return BiomeType.Green;
            }

            ratio += whiteRatioNormalized;

            if (value < ratio)
            {
                return BiomeType.White;
            }

            return BiomeType.Black;
        }

        public uint GetNumberOfBase(BiomeType type)
        {
            switch (type)
            {
                case BiomeType.Black:
                    return blackBases;

                case BiomeType.Blue:
                    return blueBases;

                case BiomeType.Green:
                    return greenBases;

                case BiomeType.White:
                    return whiteBases;

                default:
                    Debug.Log("TypeBiome not set in the call \"GetNumberOfBase\"");
                    return 0;
            }
        }

        public Texture GetIcon(BiomeType type)
        {
            switch (type)
            {
                case BiomeType.Black:
                    return blackIcon;

                case BiomeType.Blue:
                    return blueIcon;

                case BiomeType.Green:
                    return greenIcon;

                case BiomeType.White:
                    return whiteIcon;

                default:
                    Debug.Log("TypeBiome not set in the call \"GetIcon\"");
                    return null;
            }
        }

    }
}
