using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public enum BiomeType
    {
        None = 0,
        Green,
        Black,
        White,
        Blue,
    }

    public class Biome : MonoBehaviour
    {
        private BiomeType _type;
        private List<Tile> _tiles;


        private void Awake()
        {
            // nothing
        }

        public void Construct(BiomeType type)
        {
            _tiles = new List<Tile>();
            _type = type;
        }

        public static BiomeType GetBiomeType(float value)
        {
            if (value < 0.25)
            {
                return BiomeType.Blue;
            }

            if (value < 0.45)
            {
                return BiomeType.Green;
            }

            if (value < 0.571)
            {
                return BiomeType.White;
            }

            return BiomeType.Black;
        }

        public Color GetColor()
        {
            switch (_type)
            {
                case BiomeType.Black:
                      return Color.black;

                case BiomeType.Blue:
                    return Color.blue;

                case BiomeType.Green:
                    return Color.green;

                case BiomeType.White:
                    return Color.white;

                default:
                    return new Color(0.6f, 0.6f, 0.6f); // grey
            }
        }

        public static uint GetNumberOfBase(BiomeType type)
        {
            switch (type)
            {
                case BiomeType.Black:
                    return 8;

                case BiomeType.Blue:
                    return 9;

                case BiomeType.Green:
                    return 15;

                case BiomeType.White:
                    return 22;

                default:
                    Debug.Log("TypeBiome not set in the call \"GetNumberOfBase\"");
                    return 0; // grey
            }
        }

        public void Add(Tile tile)
        {
            _tiles.Add(tile);
        }

        public void Absorb(Biome biome)
        {
            _tiles.AddRange(biome._tiles);

            for (int i = 0; i < biome._tiles.Count; i++)
            {
                biome._tiles[i].SetBiome(this);
            }
        }

        public BiomeType GetType()
        {
            return _type;
        }

        public void DisplayCubes()
        {
            for (int i = 0; i < _tiles.Count; i++)
            {
                Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.8F);
                Gizmos.DrawCube(_tiles[i].transform.position, _tiles[i].transform.lossyScale);
            }
        }
    }
}
