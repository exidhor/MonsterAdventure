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

        public void Generate(List<List<Chunk>> chunks)
        {
            _zones.Clear();

            for (int i = 0; i < chunks.Count; i++)
            {
                for (int j = 0; j < chunks[0].Count; j++)
                {
                    FindZone(i, j, chunks);
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

        private void FindZone(int x, int y, List<List<Chunk>> chunks)
        {
            BiomeType currentType = chunks[x][y].GetBiomeType();

            bool zoneSet = false;

            // check for left
            if (x > 0 && currentType == chunks[x - 1][y].GetBiomeType())
            {
                Zone zone = chunks[x - 1][y].GetZone();
                chunks[x][y].SetZone(zone);
                zone.Add(chunks[x][y]);

                zoneSet = true;
            }

            // check for bot
            if (y > 0 && currentType == chunks[x][y - 1].GetBiomeType())
            {
                Zone zone = chunks[x][y - 1].GetZone();

                if (zoneSet)
                {
                    if (zone != chunks[x][y].GetZone())
                    {
                        chunks[x][y].GetZone().Absorb(zone);
                        _zones.Remove(zone);
                        Destroy(zone.gameObject);
                    }
                }
                else
                {
                    chunks[x][y].SetZone(zone);
                    zone.Add(chunks[x][y]);
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
                newZone.Add(chunks[x][y]);

                // set the zone to the tile
                chunks[x][y].SetZone(newZone);
            }
        }
    }
}
