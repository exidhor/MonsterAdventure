using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    /// <summary>
    /// Represent a determined place, like cities
    /// </summary>
    public class Place : MonoBehaviour
    {
        private Coords _coords;

        public PlaceType _type;

        /// <summary>
        /// Initialize the object
        /// </summary>
        /// <param name="x">The abs coord in the chunk grid</param>
        /// <param name="y">The ord coord in the chunk grid</param>
        public void Construct(Coords coords)
        {
            _coords = coords;
        }

        /// <summary>
        /// Return the position in the chunk grid
        /// </summary>
        /// <returns>The position in the chunk grid</returns>
        public Coords GetCoords()
        {
            return _coords;
        }
    }
}