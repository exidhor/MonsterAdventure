using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public static class MathHelper
    {
        public static Rect GetGlobalBounds(List<Vector2> points)
        {
            float maxX = float.MinValue;
            float minX = float.MaxValue;
            
            float maxY = float.MinValue;
            float minY = float.MaxValue;

            for (int i = 0; i < points.Count; i++)
            {
                if (minX > points[i].x)
                {
                    minX = points[i].x;
                }
                if (maxX < points[i].x)
                {
                    maxX = points[i].x;
                }

                if (minY > points[i].y)
                {
                    minY = points[i].y;
                }
                if (maxY < points[i].y)
                {
                    maxY = points[i].y;
                }
            }

            return new Rect(minX, minY, maxX - minX, maxY - minY);
        }
    }
}
