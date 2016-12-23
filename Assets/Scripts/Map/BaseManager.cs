using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace MonsterAdventure
{
    public class BaseManager : MonoBehaviour
    {
        public Base basePrefab;

        public Texture blopIcon;
        public uint numberOfBlopBase = 9;
        public uint blopMinDist = 10;

        public Texture judgeIcon;
        public uint numberofJudgeBase = 3;
        public uint judgeMinDist = 5;

        public Texture monsterIcon;
        public uint numberOfMonsterBase = 4;
        public uint monsterMinDist = 5;

        public bool drawGizmoBase = true;

        public BiomeManager biomeManager;
        public RandomGenerator randomGenerator;

        private Dictionary<BaseType, List<Base>> _basesPerType;

        public void Construct()
        {
            _basesPerType = new Dictionary<BaseType, List<Base>>();

            foreach (BaseType biomeType in Enum.GetValues(typeof(BaseType)))
            {
                _basesPerType.Add(biomeType, new List<Base>());
            }
        }

        public void Generate()
        {
            foreach (BaseType baseType in _basesPerType.Keys)
            {
                GenerateBases(baseType);
            }
        }

        private void GenerateBases(BaseType baseType)
        {
            uint numberOfBaseToGenerate = GetNumberOfBase(baseType);
            uint minDist = GetMinDist(baseType);
            BiomeType biomeType = GetBiomeTypeAccordingTo(baseType);

            List<Chunk> foundedChunks = biomeManager.GetChunkFromMinDistance(biomeType, (int)minDist);

            while (numberOfBaseToGenerate > 0 && foundedChunks.Count > 0)
            {
                // find the chunk
                int randomIndex = randomGenerator.Next(foundedChunks.Count - 1);
                Chunk chunk = foundedChunks[randomIndex];

                // create the base
                Base newBase = InstantiateBase(baseType);
                newBase.Construct(chunk.GetX(), chunk.GetY());
                newBase.transform.position = chunk.transform.position;
                _basesPerType[baseType].Add(newBase);

                // remove the tile and the other at the min dist
                foundedChunks.RemoveAt(randomIndex);
                RemoveChunks(ref foundedChunks, chunk, (int)minDist);
                numberOfBaseToGenerate--;
            }
        }

        private Base InstantiateBase(BaseType baseType)
        {
            Base baseResult = Instantiate<Base>(basePrefab);
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

        public Texture GetTextureIcon(BaseType baseType)
        {
            switch (baseType)
            {
                case BaseType.Blop:
                    return blopIcon;

                case BaseType.Judge:
                    return judgeIcon;

                case BaseType.Monster:
                    return monsterIcon;
            }

            return null;
        }

        private uint GetNumberOfBase(BaseType baseType)
        {
            switch (baseType)
            {
                case BaseType.Blop:
                    return numberOfBlopBase;

                case BaseType.Judge:
                    return numberofJudgeBase;

                case BaseType.Monster:
                    return numberOfMonsterBase;
            }

            return 0;
        }

        private uint GetMinDist(BaseType baseType)
        {
            switch (baseType)
            {
                case BaseType.Blop:
                    return blopMinDist;

                case BaseType.Judge:
                    return judgeMinDist;

                case BaseType.Monster:
                    return monsterMinDist;
            }

            return 0;
        }

        public static BiomeType GetBiomeTypeAccordingTo(BaseType baseType)
        {
            switch (baseType)
            {
                case BaseType.Blop:
                    return BiomeType.Green;

                case BaseType.Judge:
                    return BiomeType.Black;

                case BaseType.Monster:
                    return BiomeType.White;
            }

            return BiomeType.None;
        }

        public Dictionary<BaseType, List<Base>> GetBasesPerType()
        {
            return _basesPerType;
        }
    }
}