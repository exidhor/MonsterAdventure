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
        private int _coord_x;
        private int _coord_y;

        public PlaceType _type;

        /// <summary>
        /// Initialize the object
        /// </summary>
        /// <param name="x">The abs coord in the chunk grid</param>
        /// <param name="y">The ord coord in the chunk grid</param>
        public void Construct(int x, int y)
        {
            _coord_x = x;
            _coord_y = y;
        }

        /// <summary>
        /// Return the abs coord in the chunk grid
        /// </summary>
        /// <returns>The abs coord in the chunk grid</returns>
        public int GetCoordX()
        {
            return _coord_x;
        }

        /// <summary>
        /// Return the abs coord in the chunk grid
        /// </summary>
        /// <returns>The abs coord in the chunk grid</returns>
        public int GetCoordY()
        {
            return _coord_y;
        }
    }
}