using Delaunay.Geo;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterAdventure.Editor
{
    public class GizmoMap
    {
        [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
        static void DrawGizmoForVoronoi(Map map, GizmoType gizmoType)
        {
            Gizmos.color = new Color(0.5f,0.5f,0.5f,0.8f);

            List<LineSegment> segments = map.GetSegments();

            foreach (LineSegment line in segments)
            {
                Vector3 firstPoint = new Vector2(line.p0.Value.x, line.p0.Value.y);
                Vector3 secondPoint = new Vector2(line.p1.Value.x, line.p1.Value.y);
                Gizmos.DrawLine(firstPoint, secondPoint);
            }
        }

        [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
        static void DrawGizmoForBases(Map map, GizmoType gizmoType)
        {
            map.DrawIconsForBases();
        }
    }
}
