using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace MonsterAdventure.Editor
{
    public class GizmosTileManager
    {
        [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
        static void DrawGizmoForMyScript(TileManager tileManager, GizmoType gizmoType)
        {
            if (!tileManager.drawDistance)
                return;

            List<List<Tile>> tiles = tileManager.GetTiles();

            for (int i = 0; i < tiles.Count; i++)
            {
                for (int j = 0; j < tiles[i].Count; j++)
                {
                    Handles.Label(tiles[i][j].transform.position, tiles[i][j].GetDistanceToLimit().ToString());
                }
            }
        }
    }
}
