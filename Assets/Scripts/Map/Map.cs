using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Delaunay.Geo;
using UnityEngine;

namespace MonsterAdventure
{
    public class Map : MonoBehaviour
    {
        public Background backgroundPrefab;

        private Background _background;
        private Rect _bounds;
        private RandomGenerator _random;
        private NoiseGenerator _noise;
        private VoronoiGenerator _voronoi;
        private BiomeConfig _biomeConfig;
        private Dictionary<BiomeType, List<Region>> _bases;

        public void Construct(Rect bounds, int tileSize, RandomGenerator random,
            NoiseGenerator noise, VoronoiGenerator voronoi, BiomeConfig biomeConfig)
        {
            _random = random;
            _voronoi = voronoi;
            _noise = noise;
            _biomeConfig = biomeConfig;

            _bases = new Dictionary<BiomeType, List<Region>>();

            _background = InstanciateBackground();

            InitSize(bounds, tileSize);
        }

        public void Generate()
        {
            _noise.Generate((int)_bounds.width, transform, _random);

            ApplyNoise();
            FinalizeBiomes();

            //GenerateBiomes();
            //GenerateDecorObstacles();
        }

        private Background InstanciateBackground()
        {
            Background background = Instantiate<Background>(backgroundPrefab);
            background.name = "Background";
            background.transform.parent = gameObject.transform;

            return background;
        }

        private void InitSize(Rect bounds, int tileSize)
        {
            _bounds = bounds;

            _background.Construct((int)_bounds.width, tileSize, _biomeConfig);
        }

        private void ApplyNoise()
        {
            for (int i = 0; i < _background.GetLength(0); i++)
            {
                for (int j = 0; j < _background.GetLength(1); j++)
                {
                    float sample = _noise.Get(i, j);

                    _background.AssignBiome(i, j, sample);
                }
            }
        }

        private void FinalizeBiomes()
        {
            List<Region> regions = new List<Region>();

            // construct regions from voronoi cellulas
            for (int i = 0; i < _voronoi.voronoi.regions.Count; i++)
            {
                Region region = new Region(MathHelper.GetGlobalBounds(_voronoi.voronoi.regions[i]));
                regions.Add(region);
            }

            // find all the colors in the region
            for (int i = 0; i < regions.Count; i++)
            {
                List<Tile> tiles = GetTileIn(regions[i]);

                foreach (Tile tile in tiles)
                {
                    regions[i].AddBiomeType(tile.GetBiomeType());   
                }
            }

            // Sort the region in 2 categories :
            //  - Contains only one type of Biomes
            //  - Contains multiples type of Biomes
            List<Region> oneBiomeTypeRegions = new List<Region>();
            List<Region> multipleBiomeTypeRegions = new List<Region>();

            foreach (Region region in regions)
            {
                if (region.GetBiomeTypeCount() == 1)
                {
                    oneBiomeTypeRegions.Add(region);
                }
                else
                {
                    multipleBiomeTypeRegions.Add(region);
                }
            }
            
            // we dont need this anymore
            regions.Clear();

            // construct list of region by biomeType
            Dictionary<BiomeType, List<Region>> finalBases = new Dictionary<BiomeType, List<Region>>();
            Dictionary<BiomeType, List<Region>> availableRegions = new Dictionary<BiomeType, List<Region>>();

            foreach (BiomeType biomeType in Enum.GetValues(typeof(BiomeType)))
            {
                if (biomeType == BiomeType.None)
                    continue;

                finalBases.Add(biomeType, new List<Region>());
                availableRegions.Add(biomeType, new List<Region>());
            }

            // Fill this new list with obvious regions
            foreach (Region region in oneBiomeTypeRegions)
            {
                finalBases[region.GetFirstBiomeType()].Add(region);
            }

            // verification of the number of regions
            foreach (BiomeType biomeType in finalBases.Keys)
            {
                if (biomeType == BiomeType.None)
                    continue;

                while (finalBases[biomeType].Count > _biomeConfig.GetNumberOfBase(biomeType))
                {
                    int randomIndex = _random.Next(finalBases[biomeType].Count - 1);
                    
                    finalBases[biomeType].RemoveAt(randomIndex);
                }
            }
            
            // we clear the oneBiomeTypeRegions to be able to fill it again
            oneBiomeTypeRegions.Clear();

            // we then fill the available regions
            foreach (Region region in multipleBiomeTypeRegions)
            {
                foreach (BiomeType type in availableRegions.Keys)
                {
                    if (type == BiomeType.None)
                        continue;

                    if (region.HasBiomeType(type))
                    {
                        availableRegions[type].Add(region);
                    }
                }
            }

            // we dont need this anymore
            multipleBiomeTypeRegions.Clear();
            
            // we work with the required number of base
            Dictionary <BiomeType, uint> numberOfRegionsNeeded = new Dictionary<BiomeType, uint>();

            foreach (BiomeType biomeType in Enum.GetValues(typeof(BiomeType)))
            {
                if(biomeType == BiomeType.None)
                    continue;

                uint neededNumber = _biomeConfig.GetNumberOfBase(BiomeType.Black) - (uint)finalBases[biomeType].Count;

                if (neededNumber > 0)
                {
                    numberOfRegionsNeeded.Add(biomeType, neededNumber);
                }
            }

            while (numberOfRegionsNeeded.Count > 0)
            {
                // Find the priority (the most difficult to complete)
                // i.e. the biggest number of bases missing
                uint biggestNumberBaseMissing = 0;
                BiomeType biggestTypeBaseMissing = BiomeType.None;;

                foreach (BiomeType type in numberOfRegionsNeeded.Keys)
                {
                    if (type == BiomeType.None)
                        continue;

                    if (biggestNumberBaseMissing < numberOfRegionsNeeded[type])
                    {
                        biggestNumberBaseMissing = numberOfRegionsNeeded[type];
                        biggestTypeBaseMissing = type;
                    }
                }

                uint currentNumberBaseMissing = biggestNumberBaseMissing;
                BiomeType currentType = biggestTypeBaseMissing;

                // try to add the missing values
                while (currentNumberBaseMissing > 0)
                {
                    if (availableRegions[currentType].Count > 0)
                    {
                        int randomIndex = _random.Next(availableRegions[currentType].Count - 1);
                        Region region = availableRegions[currentType][randomIndex];
                        finalBases[currentType].Add(region);
                        
                        currentNumberBaseMissing--;

                        // remove the region
                        foreach (BiomeType type in availableRegions.Keys)
                        {
                            availableRegions[type].Remove(region);
                        }
                    }
                    else
                    {
                        // create another base ?
                        currentNumberBaseMissing = 0;
                    }
                }

                // remove the type we did
                numberOfRegionsNeeded.Remove(currentType);
            }

            _bases = new Dictionary<BiomeType, List<Region>>(finalBases);
        }

        private List<Tile> GetTileIn(Region region)
        {
            List<Tile> tiles = new List<Tile>();

            for (int i = region.left - (int)_bounds.x; i < region.right - _bounds.x; i++)
            {
                for (int j = region.bot - (int)_bounds.y; j < region.top - _bounds.y; j++)
                {
                    tiles.Add(_background.Get(i, j));
                }
            }

            return tiles;
        }

        public List<LineSegment> GetSegments()
        {
            return _voronoi.voronoi.graph;
        }

        public Dictionary<BiomeType, List<Region>> GetBases()
        {
            return _bases;
        }

        public void DrawIconsForBases()
        {
            foreach (BiomeType biomeType in _bases.Keys)
            {
                Texture icon = _biomeConfig.GetIcon(biomeType);
                //Vector2 iconSize  = new Vector2(icon.width, icon.height);
                Vector2 iconSize = new Vector2(5, 5);

                foreach (Region region in _bases[biomeType])
                {
                    Vector2 position = region.center;
                    position -= iconSize/2;

                    Rect canvas = new Rect(position, iconSize);

                    Gizmos.DrawGUITexture(canvas, icon);
                }
            }
        }
    }
}
