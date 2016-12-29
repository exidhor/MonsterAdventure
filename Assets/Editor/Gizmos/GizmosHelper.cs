using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.Editor
{
    public static class GizmosHelper
    {
        public static void DrawRect(Rect rect, Color color, float z = 0)
        {
            Gizmos.color = color;

            Vector2 topLeftCorner = new Vector2(rect.xMin, rect.yMin);
            Vector2 topRightCorner = new Vector2(rect.xMax, rect.yMin);
            Vector2 botRightCorner = new Vector2(rect.xMax, rect.yMax);
            Vector2 botLeftCorner = new Vector2(rect.xMin, rect.yMax);

            Gizmos.DrawLine(topLeftCorner, topRightCorner);
            Gizmos.DrawLine(topRightCorner, botRightCorner);
            Gizmos.DrawLine(botRightCorner, botLeftCorner);
            Gizmos.DrawLine(botLeftCorner, topLeftCorner);
        }
    }
}
