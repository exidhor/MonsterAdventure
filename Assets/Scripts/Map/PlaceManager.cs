using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace MonsterAdventure
{
    /// <summary>
    /// Manage and construct the places with given rules.
    /// </summary>
    public class PlaceManager : MonoBehaviour
    {
        public Place basePrefab;

        public Texture blopIcon;
        public uint numberOfBlopPlace = 9;
        public uint blopMinDistToLimit = 5;
        public uint blopMinGap = 10;

        public Texture judgeIcon;
        public uint numberofJudgePlace = 3;
        public uint judgeMinDistToLimit = 5;
        public uint judgeMinGap = 5;

        public Texture monsterIcon;
        public uint numberOfMonsterPlace = 4;
        public uint monsterMinDistToLimit = 5;
        public uint monsterMinGap = 5;

        public bool drawGizmoPlace = true;

        public BiomeManager biomeManager;
        public RandomGenerator randomGenerator;

        private Dictionary<PlaceType, List<Place>> _basesPerType;

        public void Construct()
        {
            _basesPerType = new Dictionary<PlaceType, List<Place>>();

            foreach (PlaceType biomeType in Enum.GetValues(typeof(PlaceType)))
            {
                _basesPerType.Add(biomeType, new List<Place>());
            }
        }

        public void Generate()
        {
            foreach (PlaceType baseType in _basesPerType.Keys)
            {
                GeneratePlaces(baseType);
            }
        }

        private void GeneratePlaces(PlaceType baseType)
        {
            uint numberOfPlaceToGenerate = GetNumberOfPlace(baseType);
            uint minDist = GetMinDistToLimit(baseType);
            uint minGap = GetMinGap(baseType);
            BiomeType biomeType = GetBiomeTypeAccordingTo(baseType);

            List<Chunk> foundedChunks = biomeManager.GetChunkFromMinDistance(biomeType, (int)minDist);

            while (numberOfPlaceToGenerate > 0 && foundedChunks.Count > 0)
            {
                // find the chunk
                int randomIndex = randomGenerator.Next(foundedChunks.Count - 1);
                Chunk chunk = foundedChunks[randomIndex];

                // create the base
                Place newPlace = InstantiatePlace(baseType);
                newPlace.Construct(chunk.GetX(), chunk.GetY());
                newPlace.transform.position = chunk.transform.position;
                _basesPerType[baseType].Add(newPlace);

                // remove the tile and the other at the min dist
                foundedChunks.RemoveAt(randomIndex);
                RemoveChunks(ref foundedChunks, chunk, (int)minGap);
                numberOfPlaceToGenerate--;
            }
        }

        private Place InstantiatePlace(PlaceType baseType)
        {
            Place baseResult = Instantiate<Place>(basePrefab);
            baseResult.transform.parent = gameObject.transform;
            baseResult.name = baseType.ToString();

            return baseResult;
        }

        private void RemoveChunks(ref List<Chunk> chunks, Chunk chunk, int dist)
        {
            for (int i = 0; i < chunks.Count; i++)
            {
                Chunk currentChunk =  chunks[i];

                if (currentChunk != chunk
                    && currentChunk.GetDistanceFrom(chunk) < dist)
                {
                    chunks.RemoveAt(i);
                    i--;
                }
            }
        }

        public Texture GetTextureIcon(PlaceType baseType)
        {
            switch (baseType)
            {
                case PlaceType.Blop:
                    return blopIcon;

                case PlaceType.Judge:
                    return judgeIcon;

                case PlaceType.Monster:
                    return monsterIcon;
            }

            return null;
        }

        private uint GetNumberOfPlace(PlaceType baseType)
        {
            switch (baseType)
            {
                case PlaceType.Blop:
                    return numberOfBlopPlace;

                case PlaceType.Judge:
                    return numberofJudgePlace;

                case PlaceType.Monster:
                    return numberOfMonsterPlace;
            }

            return 0;
        }

        private uint GetMinGap(PlaceType baseType)
        {
            switch (baseType)
            {
                case PlaceType.Blop:
                    return blopMinGap;

                case PlaceType.Judge:
                    return judgeMinGap;

                case PlaceType.Monster:
                    return monsterMinGap;
            }

            return 0;
        }

        private uint GetMinDistToLimit(PlaceType baseType)
        {
            switch (baseType)
            {
                case PlaceType.Blop:
                    return blopMinDistToLimit;

                case PlaceType.Judge:
                    return judgeMinDistToLimit;

                case PlaceType.Monster:
                    return monsterMinDistToLimit;
            }

            return 0;
        }

        public static BiomeType GetBiomeTypeAccordingTo(PlaceType baseType)
        {
            switch (baseType)
            {
                case PlaceType.Blop:
                    return BiomeType.Green;

                case PlaceType.Judge:
                    return BiomeType.Black;

                case PlaceType.Monster:
                    return BiomeType.White;
            }

            return BiomeType.None;
        }

        public Dictionary<PlaceType, List<Place>> GetPlacesPerType()
        {
            return _basesPerType;
        }
    }
}