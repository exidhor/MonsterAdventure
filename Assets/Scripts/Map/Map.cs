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
        private SimplexNoise _noise;
        private RandomGenerator _random;

        public void Construct(Vector2 mapSize, Vector2 tileSize,
            SimplexNoiseEntry simplexNoiseEntry, RandomGenerator random)
        {
            _random = random;

            _background = InstanciateBackground();

            InitSize(mapSize, tileSize);

            _noise = new SimplexNoise(simplexNoiseEntry, _random);
        }

        public void Generate()
        {
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

        private void InitSize(Vector2 mapSize, Vector2 tileSize)
        {
            _limits = new Rect(new Vector2(), mapSize);

            _background.Construct(mapSize, tileSize);
        }

        private void ApplyNoise()
        {
            for (uint i = 0; i < _background.GetLength(0); i++)
            {
                for (uint j = 0; j < _background.GetLength(1); j++)
                {
                    float grayscale = (float)_noise.GetNoise((int)i, (int)j);

                    Color color = new Color(grayscale, grayscale, grayscale, 1);

                    _background.Get(i, j).GetComponent<SpriteRenderer>().color = color;
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
