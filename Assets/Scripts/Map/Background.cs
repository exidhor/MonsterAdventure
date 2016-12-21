﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterAdventure
{
    public class Background : MonoBehaviour
    {
        public Tile[] tilePrefabs;
        public Biome biomePrefab;

        private List<List<Tile>> _tiles;
        private BiomeConfig _biomeConfig;
        private List<Biome> _biomes;

        private GameObject _biomeGroup;
        private GameObject _tileGroup; 

        public void Construct(int size, int tileSize, BiomeConfig biomeConfig)
        {
            _biomeConfig = biomeConfig;

            _biomeGroup = new GameObject();
            _biomeGroup.name = "Biomes";
            _biomeGroup.transform.parent = gameObject.transform;

            _tileGroup = new GameObject();
            _tileGroup.name = "Tiles";
            _tileGroup.transform.parent = gameObject.transform;

            Vector2 mapSize;
            mapSize.x = size;
            mapSize.y = size; 

            _tiles = new List<List<Tile>>();

            _biomes = new List<Biome>();

            Vector2 offset = (mapSize - new Vector2(1, 1)) / 2;

            for (int i = 0; i < mapSize.x; i++)
            {
                _tiles.Add(new List<Tile>());

                for (int j = 0; j < mapSize.y; j++)
                {
                    Tile prefab = tilePrefabs[0];

                    _tiles[i].Add(InstantiateTile(prefab, i, j));

                    float x = i*tileSize - offset.x;
                    float y = j*tileSize - offset.y;

                    _tiles[i][j].transform.position = new Vector2(x, y);
                }
            }
        }

        public Tile Get(int x, int y)
        {
            if (0 <= x && x < _tiles.Count
                && 0 <= y && y < _tiles[0].Count)
            {
                return _tiles[(int)x][(int)y];
            }

            return null;
        }

        public uint GetLength(uint index)
        {
            if (index == 0)
                return (uint) _tiles.Count;

            return (uint) _tiles[0].Count;
        }

        private Tile InstantiateTile(Tile prefab, int x, int y)
        {
            Tile tile = Instantiate<Tile>(prefab);
            tile.transform.parent = _tileGroup.transform;
            tile.name = prefab.name + " (" + x + ", " + y + ")";

            return tile;
        }

        private Biome InstantiateBiome(Biome prefab, BiomeType biomeType)
        {
            Biome biome = Instantiate<Biome>(prefab);
            biome.transform.parent = _biomeGroup.transform;
            biome.name = biomeType.ToString();

            return biome;
        }

        public void AssignBiome(int x, int y, float noiseValue)
        {
            BiomeType biomeType = _biomeConfig.GetBiomeType(noiseValue);
    
            Tile tile = _tiles[x][y];

            tile.SetBiomeType(biomeType);

            FillBiome(x, y, biomeType);

            tile.GetComponent<SpriteRenderer>().color = _biomeConfig.GetColor(tile.GetBiomeType());
        }

        private void FillBiome(int x, int y, BiomeType biomeType)
        {
            BiomeType currentType = _tiles[x][y].GetBiomeType();

            bool biomeSet = false;

            // check for left
            if (x > 0 && currentType == _tiles[x - 1][y].GetBiomeType())
            {
                Biome biome = _tiles[x - 1][y].GetBiome();
                _tiles[x][y].SetBiome(biome);
                biome.Add(_tiles[x][y]);

                biomeSet = true;
            }

            // check for bot
            if (y > 0 && currentType == _tiles[x][y - 1].GetBiomeType())
            {
                Biome biome = _tiles[x][y - 1].GetBiome();

                if (biomeSet)
                {
                    if (biome != _tiles[x][y].GetBiome())
                    {
                        _tiles[x][y].GetBiome().Absorb(biome);
                        _biomes.Remove(biome);
                        Destroy(biome.gameObject);
                    }
                }
                else
                {
                    _tiles[x][y].SetBiome(biome);
                    biome.Add(_tiles[x][y]);
                }

                biomeSet = true;
            }

            if(!biomeSet)
            {
                // create a new Biome
                Biome newBiome = InstantiateBiome(biomePrefab, currentType);
                newBiome.Construct(biomeType);
                _biomes.Add(newBiome);

                // add the tile to the biome
                newBiome.Add(_tiles[x][y]);

                // set the biome to the tile
                _tiles[x][y].SetBiome(newBiome);
            }
        }
    }
}