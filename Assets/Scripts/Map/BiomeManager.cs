using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class BiomeManager : MonoBehaviour
    {
        public Biome biomePrefab; 

        public NoiseGenerator noiseGenerator;
        public MapConfig MapConfig;

        private Dictionary<BiomeType, Biome> _biomes;

        public void Construct()
        {
            _biomes = new Dictionary<BiomeType, Biome>();

            foreach (BiomeType biomeType in Enum.GetValues(typeof(BiomeType)))
            {
                Biome biome = InstantiateBiome(biomeType);

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
        }

        private void ApplyNoise(List<List<Tile>> tiles)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                for (int j = 0; j < tiles[0].Count; j++)
                {
                    float sample = noiseGenerator.Get(i, j);
                    BiomeType type = MapConfig.GetBiomeType(sample);

                    AssignBiomeTypeToTile(tiles[i][j], type);
                }
            }
        }

        private void AssignBiomeTypeToTile(Tile tile, BiomeType type)
        {
            tile.SetBiomeType(type);
            tile.GetComponent<SpriteRenderer>().color = MapConfig.GetColor(tile.GetBiomeType());
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
    }
}