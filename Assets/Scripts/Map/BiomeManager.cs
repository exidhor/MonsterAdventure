using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class BiomeManager : MonoBehaviour
    {
        public Color blueColor = Color.blue;
        public float blueRatio = 0.25f;

        public Color greenColor = Color.green;
        public float greenRatio = 0.2f;

        public Color whiteColor = Color.white;
        public float whiteRatio = 0.12f;

        public Color blackColor = Color.black;
        public float blackRatio = 0.47f;

        public Biome biomePrefab; 

        public NoiseGenerator noiseGenerator;

        private float blackRatioNormalized;
        private float blueRatioNormalized;
        private float greenRatioNormalized;
        private float whiteRatioNormalized;

        private Dictionary<BiomeType, Biome> _biomes;

        public void Construct()
        {
            // normalize the ratio value
            float sum = blackRatio + blueRatio + greenRatio + whiteRatio;

            blackRatioNormalized = blackRatio / sum;
            blueRatioNormalized = blueRatio / sum;
            greenRatioNormalized = greenRatio / sum;
            whiteRatioNormalized = whiteRatio / sum;

            _biomes = new Dictionary<BiomeType, Biome>();

            foreach (BiomeType biomeType in Enum.GetValues(typeof(BiomeType)))
            {
                Biome biome = InstantiateBiome(biomeType);
                biome.type = biomeType;
                _biomes.Add(biomeType, biome);
            }

            noiseGenerator.Construct();
        }

        private Biome InstantiateBiome(BiomeType type)
        {
            Biome biome = Instantiate<Biome>(biomePrefab);
            biome.transform.parent = gameObject.transform;
            biome.name = type.ToString();

            return biome;
        }

        public void Generate(List<List<Tile>> tiles, int mapSize, RandomGenerator random)
        {
            noiseGenerator.Generate(mapSize, transform, random);

            ApplyNoise(tiles);

            SortPerBiomes(tiles);

            OrganizeBiomes(tiles);
        }

        public List<Tile> GetTilesFromMinDistance(BiomeType biomeType, int minDistance)
        {
            Biome targetBiome = _biomes[biomeType];

            return targetBiome.GetTilesFromMinDistance(minDistance);
        }

        private void ApplyNoise(List<List<Tile>> tiles)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                for (int j = 0; j < tiles[0].Count; j++)
                {
                    float sample = noiseGenerator.Get(i, j);
                    BiomeType type = GetBiomeType(sample);

                    AssignBiomeTypeToTile(tiles[i][j], type);
                }
            }
        }

        private void AssignBiomeTypeToTile(Tile tile, BiomeType type)
        {
            tile.SetBiomeType(type);
            tile.GetComponent<SpriteRenderer>().color = GetColor(tile.GetBiomeType());
        }

        private void SortPerBiomes(List<List<Tile>> tiles)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                for (int j = 0; j < tiles[0].Count; j++)
                {
                    Tile currentTile = tiles[i][j];
                    BiomeType type = currentTile.GetBiomeType();

                    if (type == BiomeType.None)
                    {
                        Debug.Log("Trying to add a None biome type in the BiomeManager\n"
                                  + "Position : " + i + ", " + j);
                    }
                    else
                    {
                        _biomes[type].Add(currentTile);
                    }
                }
            }
        }

        private void OrganizeBiomes(List<List<Tile>> tiles)
        {
            foreach (Biome biome in _biomes.Values)
            {
                biome.Organize(tiles);
            }
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
    }
}