using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class NoiseGenerator : MonoBehaviour
    {
        public float frequency = 1f;

        [Range(2, 512)]
        public int resolution = 256;

        [Range(1, 8)]
        public int octaves = 1;

        [Range(1f, 4f)]
        public float lacunarity = 2f;

        [Range(0f, 1f)]
        public float persistence = 0.5f;

        [Range(1, 3)]
        public int dimensions = 3;

        public NoiseMethodType type;

        public Gradient coloring;

        private Noise _noise;

        private float _stepSize;
        //private int   _halfSize;
        private float _halfSize;

        private Vector3 _point00;
        private Vector3 _point10;
        private Vector3 _point01;
        private Vector3 _point11;

        private void Awake()
        {
            
        }

        public void Construct()
        {
            _noise = new Noise();
        }

        public void Generate(int size, Transform transform, RandomGenerator random)
        {
            _noise.InitWithSeed(random);
            //_halfSize = size / 2;
            _halfSize = 0.5f;
            _stepSize = (float)_halfSize / resolution;

            _point00 = transform.TransformPoint(new Vector3(-_halfSize, -_halfSize));
            _point10 = transform.TransformPoint(new Vector3(_halfSize, -_halfSize));
            _point01 = transform.TransformPoint(new Vector3(-_halfSize, _halfSize));
            _point11 = transform.TransformPoint(new Vector3(_halfSize, _halfSize));
        }

        public float Get(int x, int y)
        {
            Vector3 point0 = Vector3.Lerp(_point00, _point01, (y + _halfSize) * _stepSize);
            Vector3 point1 = Vector3.Lerp(_point10, _point11, (y + _halfSize) * _stepSize);
           
            Vector3 point = Vector3.Lerp(point0, point1, (x + _halfSize) * _stepSize);
            float sample = Noise.Sum(_noise.methods[(int)type][dimensions - 1], 
                                         point, 
                                         frequency, 
                                         octaves, 
                                         lacunarity, 
                                         persistence).value;

            // scale and offset the noise value to fit in 0 - 1f range
            sample = sample * 0.5f + 0.5f;

            return sample;
        }
    }
}
