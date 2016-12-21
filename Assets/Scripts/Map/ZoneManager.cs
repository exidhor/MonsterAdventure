using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class ZoneManager : MonoBehaviour
    {
        public Zone zonePrefab;

        private List<Zone> _zones;

        public void Construct()
        {
            _zones = new List<Zone>();
        }

        public void Generate(List<List<Tile>> tiles)
        {
            _zones.Clear();

            for (int i = 0; i < tiles.Count; i++)
            {
                for (int j = 0; j < tiles[0].Count; j++)
                {
                    FindZone(i, j, tiles);
                }
            }
        }

        private Zone InstantiateZone(BiomeType type)
        {
            Zone zone = Instantiate<Zone>(zonePrefab);
            zone.transform.parent = gameObject.transform;
            zone.name = type.ToString();

            return zone;
        }

        private void FindZone(int x, int y, List<List<Tile>> tiles)
        {
            BiomeType currentType = tiles[x][y].GetBiomeType();

            bool zoneSet = false;

            // check for left
            if (x > 0 && currentType == tiles[x - 1][y].GetBiomeType())
            {
                Zone zone = tiles[x - 1][y].GetZone();
                tiles[x][y].SetZone(zone);
                zone.Add(tiles[x][y]);

                zoneSet = true;
            }

            // check for bot
            if (y > 0 && currentType == tiles[x][y - 1].GetBiomeType())
            {
                Zone zone = tiles[x][y - 1].GetZone();

                if (zoneSet)
                {
                    if (zone != tiles[x][y].GetZone())
                    {
                        tiles[x][y].GetZone().Absorb(zone);
                        _zones.Remove(zone);
                        Destroy(zone.gameObject);
                    }
                }
                else
                {
                    tiles[x][y].SetZone(zone);
                    zone.Add(tiles[x][y]);
                }

                zoneSet = true;
            }

            if (!zoneSet)
            {
                // create a new Zone
                Zone newZone = InstantiateZone(currentType);
                newZone.Construct(currentType);
                _zones.Add(newZone);

                // add the tile to the zone
                newZone.Add(tiles[x][y]);

                // set the zone to the tile
                tiles[x][y].SetZone(newZone);
            }
        }
    }
}
