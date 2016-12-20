using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class Map : MonoBehaviour
    {
        public Background backgroundPrefab;

        private Background _background;
        private Rect _limits;
        private RandomGenerator _random;
        private NoiseGenerator _noise;

        public void Construct(int mapSize, int tileSize, RandomGenerator random,
            NoiseGenerator noise)
        {
            _random = random;

            _background = InstanciateBackground();

            InitSize(mapSize, tileSize);

            _noise = noise;

            //_noise = new SimplexNoise(simplexNoiseEntry, _random);
        }

        public void Generate()
        {
            _noise.Generate((int)_limits.width, transform, _random);

            ApplyNoise();

            GenerateBiomes();
            GenerateDecorObstacles();
        }

        private Background InstanciateBackground()
        {
            Background background = Instantiate<Background>(backgroundPrefab);
            background.name = "Background";
            background.transform.parent = gameObject.transform;

            return background;
        }

        private void InitSize(int mapSize, int tileSize)
        {
            Vector2 size;
            size.x = mapSize;
            size.y = mapSize;

            _limits = new Rect(new Vector2(), size);

            _background.Construct(mapSize, tileSize);
        }

        private void ApplyNoise()
        {
            for (int i = 0; i < _background.GetLength(0); i++)
            {
                for (int j = 0; j < _background.GetLength(1); j++)
                {
                    float sample = _noise.Get(i, j);

                    //Color color = _noise.coloring.Evaluate(sample);

                    //_background.Get(i, j).GetComponent<SpriteRenderer>().color = color;
                    _background.AssignBiome(i, j, sample);
                }
            }
        }

        private void GenerateBiomes()
        {
            // todo
        }

        private void GenerateDecorObstacles()
        {
            // todo
        }

        
    }
}
