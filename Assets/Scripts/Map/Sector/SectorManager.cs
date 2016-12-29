using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    [RequireComponent(typeof(SectorData))]
    public class SectorManager : MonoBehaviour
    {
        private SectorData _sectorData;

        private List<List<Sector>> _sectorPerLevel;

        private bool _isInitialized = false;

        private void Awake()
        {
            _sectorData = GetComponent<SectorData>();
        }

        public void Construct(Rect mapBounds)
        {
            // construct the sector list
            _sectorPerLevel = new List<List<Sector>>();
            
            // init with the first sector (the big parent)
            int currentLevel = 0;

            _sectorPerLevel.Add(new List<Sector>());

            _sectorPerLevel[currentLevel].Add(new Sector((uint)currentLevel, mapBounds));  
            
            // Start the division
            _sectorPerLevel[currentLevel][0].Divide(_sectorData.resolution);
            
            // retrieve then store all the sectors created by the division
            List<Sector[]> childrenStack = new List<Sector[]>();
            List<Sector[]> childrenStack_next = new List<Sector[]>();
             
            childrenStack.Add(_sectorPerLevel[currentLevel][0].GetChildren());
            currentLevel++;

            while (childrenStack.Count > 0)
            {
                // we create a new list to store the current children
                _sectorPerLevel.Add(new List<Sector>());

                // we add the stack to the sector list
                foreach (Sector[] children in childrenStack)
                {
                    foreach (Sector child in children)
                    {
                        _sectorPerLevel[currentLevel].Add(child);

                        if (currentLevel < _sectorData.resolution)
                        {
                            childrenStack_next.Add(child.GetChildren());
                        }
                    } 
                }

                // we swap the stacks
                childrenStack = new List<Sector[]>(childrenStack_next);
                childrenStack_next.Clear();

                // we increase the level
                currentLevel++;
            }

            _isInitialized = true;

            Debug.Log("SectorManager constructed");
        }

        public List<List<Sector>> GetSectors()
        {
            return _sectorPerLevel;
        }

        public bool IsInitialized()
        {
            return _isInitialized;
        }
    }
}
