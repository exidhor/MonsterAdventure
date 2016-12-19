using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class SimplexNoiseEntry : MonoBehaviour
    {
        [Range(1, 1000)]
        public uint largestFeature = 50;
        [Range(0.1f, 1f)]
        public double WhiteDetails = 0.2f;
        [Range(0.1f, 5)]
        public double zoom = 1f;
        [Range(0.1f, 3)]
        public double zoomAcceleration = 2f;
        [Range(0.1f, 2)]
        public double contrast = 1f;
        [Range(-100, 100)]
        public double colorOffset = 0f;
        [Range(-100, 100)]
        public double frequency = 0f;
    }
}
