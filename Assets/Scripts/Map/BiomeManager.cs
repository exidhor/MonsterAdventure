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
        public Sprite[] blueSprites; 

        public Color greenColor = Color.green;
        public float greenRatio = 0.2f;
        public Sprite[] greenSprites;

        public Color whiteColor = Color.white;
        public float whiteRatio = 0.12f;
        public Sprite[] whiteSprites;

        public Color blackColor = Color.black;
        public float blackRatio = 0.47f;
        public Sprite[] blackSprites;

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

        public void Generate(List<List<Chunk>> chunks, int mapSize, RandomGenerator random)
        {
            noiseGenerator.Generate(mapSize, transform, random);

            ApplyNoise(chunks);

            SortPerBiomes(chunks);

            OrganizeBiomes(chunks);
        }

        public List<Chunk> GetChunkFromMinDistance(BiomeType biomeType, int minDistance)
        {
            Biome targetBiome = _biomes[biomeType];

            return targetBiome.GetChunksFromMinDistance(minDistance);
        }

        private void ApplyNoise(List<List<Chunk>> chunks)
        {
            for (int i = 0; i < chunks.Count; i++)
            {
                for (int j = 0; j < chunks[0].Count; j++)
                {
                    float sample = noiseGenerator.Get(i, j);
                    BiomeType type = GetBiomeType(sample);

                    AssignBiomeTypeToChunk(chunks[i][j], type);
                    chunks[i][j].Generate(greenSprites[0]);
                }
            }
        }

        private void AssignBiomeTypeToChunk(Chunk chunk, BiomeType type)
        {
            chunk.SetBiomeType(type);
            //chunk.GetComponent<SpriteRenderer>().color = GetColor(chunk.GetBiomeType());
        }

        private void SortPerBiomes(List<List<Chunk>> chunks)
        {
            for (int i = 0; i < chunks.Count; i++)
            {
                for (int j = 0; j < chunks[0].Count; j++)
                {
                    Chunk currentChunk = chunks[i][j];
                    BiomeType type = currentChunk.GetBiomeType();

                    if (type == BiomeType.None)
                    {
                        Debug.Log("Trying to add a None biome type in the BiomeManager\n"
                                  + "Position : " + i + ", " + j);
                    }
                    else
                    {
                        _biomes[type].Add(currentChunk);
                    }
                }
            }
        }

        private void OrganizeBiomes(List<List<Chunk>> chunks)
        {
            foreach (Biome biome in _biomes.Values)
            {
                biome.Organize(chunks);
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