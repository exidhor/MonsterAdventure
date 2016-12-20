using System.Runtime.Remoting.Channels;
using UnityEditor;
using UnityEngine;

namespace MonsterAdventure.Editor
{
    public class GizmoBiome
    {

        [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
        static void DrawGizmoForMyScript(Biome biome, GizmoType gizmoType)
        {
            biome.DisplayCubes();
        }

    }
}
