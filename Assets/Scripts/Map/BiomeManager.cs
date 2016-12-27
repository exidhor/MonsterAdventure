using System;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterAdventure
{
    /// <summary>
    /// Construct and manage all the different <see cref="Biome" /> in the map.
    /// The <see cref="Biome" /> construction is done from the <see cref="NoiseGenerator" />.
    /// </summary>
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

        /// <summary>
        /// Construct the different <see cref="Biome" /> and construct
        /// the <see cref="NoiseGenerator" />
        /// </summary>
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

        /// <summary>
        /// Instantiate the <see cref="Biome" /> in the scene from the
        /// biome prefab.
        /// </summary>
        /// <param name="type">The BiomeType of the Biome</param>
        /// <returns>The new instantiated Biome</returns>
        private Biome InstantiateBiome(BiomeType type)
        {
            Biome biome = Instantiate<Biome>(biomePrefab);
            biome.transform.parent = gameObject.transform;
            biome.name = type.ToString();

            return biome;
        }

        /// <summary>
        /// Apply the noise value from the <see cref="NoiseGenerator" /> to the
        /// chunk grid to generate the different <see cref="Biome" />.
        /// </summary>
        /// <param name="chunks">The chunks list to fill</param>
        /// <param name="mapSize">The width/height of the map</param>
        /// <param name="random">The random generator</param>
        public void Generate(List<List<Chunk>> chunks, int mapSize, RandomGenerator random)
        {
            noiseGenerator.Generate(mapSize, transform, random);

            ApplyNoise(chunks);

            SortPerBiomes(chunks);

            OrganizeBiomes(chunks);
        }

        /// <summary>
        /// Retrieve all the <see cref="Chunk" /> at the specific distance
        /// </summary>
        /// <param name="biomeType">The target BiomeType of the founded chunks</param>
        /// <param name="minDistance">The minimal distance to another biome</param>
        /// <returns>The list of founded chunks</returns>
        public List<Chunk> GetChunkFromMinDistance(BiomeType biomeType, int minDistance)
        {
            Biome targetBiome = _biomes[biomeType];

            return targetBiome.GetChunksFromMinDistance(minDistance);
        }

        /// <summary>
        /// Blend the noise value with the <see cref="Chunk" /> to create <see cref="Biome" />
        /// </summary>
        /// <param name="chunks">The chunk grid</param>
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

        /// <summary>
        /// Set the <see cref="BiomeType" /> to the target <see cref="Chunk" />
        /// </summary>
        /// <param name="chunk">The targeted chunk</param>
        /// <param name="type">The BiomeType</param>
        private void AssignBiomeTypeToChunk(Chunk chunk, BiomeType type)
        {
            chunk.SetBiomeType(type);
            //chunk.GetComponent<SpriteRenderer>().color = GetColor(chunk.GetBiomeType());
        }

        /// <summary>
        /// Sort every <see cref="Chunk" /> per <see cref="BiomeType" />
        /// </summary>
        /// <param name="chunks">The chunk grid</param>
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

        /// <summary>
        /// Organize every <see cref="Biome" />
        /// </summary>
        /// <param name="chunks">The chunk grid</param>
        private void OrganizeBiomes(List<List<Chunk>> chunks)
        {
            foreach (Biome biome in _biomes.Values)
            {
                biome.Organize(chunks);
            }
        }

        /// <summary>
        /// Find the <see cref="Color" /> of a <see cref="Chunk" /> by the <see cref="BiomeType" />
        /// </summary>
        /// <param name="type">The BiomeType</param>
        /// <returns>The Color for the Chunk</returns>
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

        /// <summary>
        /// Find the <see cref="BiomeType" /> of a <see cref="Chunk" /> from the noise value
        /// </summary>
        /// <param name="value">The noise value</param>
        /// <returns>The BiomeType</returns>
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