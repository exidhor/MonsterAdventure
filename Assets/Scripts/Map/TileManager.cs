using System;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterAdventure
{
    public class TileManager : MonoBehaviour
    {
        public Tile[] tilePrefabs;
        //public Zone ZonePrefab;
        public ZoneManager zoneManager;

        private List<List<Tile>> _tiles;
        private MapConfig _mapConfig;
        //private List<Zone> _biomes;

        //private GameObject _biomeGroup;
        //private GameObject _tileGroup; 

        public void Construct(int size, int tileSize)
        {
            //_biomeConfig = biomeConfig;

            //_biomeGroup = new GameObject();
            //_biomeGroup.name = "Biomes";
            //_biomeGroup.transform.parent = gameObject.transform;

            //_tileGroup = new GameObject();
            //_tileGroup.name = "Tiles";
            //_tileGroup.transform.parent = gameObject.transform;

            Vector2 mapSize;
            mapSize.x = size;
            mapSize.y = size; 

            _tiles = new List<List<Tile>>();

            //_biomes = new List<Zone>();

            Vector2 offset = (mapSize - new Vector2(1, 1)) / 2;

            for (int i = 0; i < mapSize.x; i++)
            {
                _tiles.Add(new List<Tile>());

                for (int j = 0; j < mapSize.y; j++)
                {
                    if (tilePrefabs.Length == 0)
                    {
                        Debug.LogError("No tilePrefabs set");
                    }

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
            tile.transform.parent = gameObject.transform;
            tile.name = prefab.name + " (" + x + ", " + y + ")";

            return tile;
        }

        /*private Zone InstantiateBiome(Zone prefab, BiomeType biomeType)
        {
            Zone zone = Instantiate<Zone>(prefab);
            zone.transform.parent = _biomeGroup.transform;
            zone.name = biomeType.ToString();

            return zone;
        }*/

        /*
        public void AssignBiome(int x, int y, float noiseValue)
        {
            BiomeType biomeType = _biomeConfig.GetBiomeType(noiseValue);
    
            Tile tile = _tiles[x][y];

            tile.SetBiomeType(biomeType);

            //FillBiome(x, y, biomeType);

            tile.GetComponent<SpriteRenderer>().color = _biomeConfig.GetColor(tile.GetBiomeType());
        }*/

        /*
        private void FillBiome(int x, int y, BiomeType biomeType)
        {
            BiomeType currentType = _tiles[x][y].GetBiomeType();

            bool biomeSet = false;

            // check for left
            if (x > 0 && currentType == _tiles[x - 1][y].GetBiomeType())
            {
                Zone zone = _tiles[x - 1][y].GetZone();
                _tiles[x][y].SetZone(zone);
                zone.Add(_tiles[x][y]);

                biomeSet = true;
            }

            // check for bot
            if (y > 0 && currentType == _tiles[x][y - 1].GetBiomeType())
            {
                Zone zone = _tiles[x][y - 1].GetZone();

                if (biomeSet)
                {
                    if (zone != _tiles[x][y].GetZone())
                    {
                        _tiles[x][y].GetZone().Absorb(zone);
                        _biomes.Remove(zone);
                        Destroy(zone.gameObject);
                    }
                }
                else
                {
                    _tiles[x][y].SetZone(zone);
                    zone.Add(_tiles[x][y]);
                }

                biomeSet = true;
            }

            if(!biomeSet)
            {
                // create a new Biome
                Zone newZone = InstantiateBiome(ZonePrefab, currentType);
                newZone.Construct(biomeType);
                _biomes.Add(newZone);

                // add the tile to the biome
                newZone.Add(_tiles[x][y]);

                // set the biome to the tile
                _tiles[x][y].SetZone(newZone);
            }
        }*/

        public List<List<Tile>> GetTiles()
        {
            return _tiles;
        }

        public void GenerateBases()
        {
            // todo

            // sort tiles by min distance to another biome type BY biome type
            // save this sort
            
            // copy lists
            // from biome specs, pull a random value, and remove around this value
            // repeat this process until the spec i reached or no value is available

            // repeat this process for each biome type
        }
    }
}

// todo : a biomeManager to manage access from distance and so and so ...