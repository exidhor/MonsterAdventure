using System.Runtime.Remoting.Channels;
using UnityEditor;
using UnityEngine;

namespace MonsterAdventure.Editor
{
    public class GizmoZone
    {

        [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
        static void DrawGizmoForMyScript(Zone zone, GizmoType gizmoType)
        {
            zone.DisplayCubes();
        }

    }
}
