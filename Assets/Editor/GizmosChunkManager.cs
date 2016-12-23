using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace MonsterAdventure.Editor
{
    public class GizmosChunkManager
    {
        [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
        static void DrawGizmoForMyScript(ChunkManager chunkManager, GizmoType gizmoType)
        {
            if (!chunkManager.drawDistance)
                return;

            List<List<Chunk>> chunks = chunkManager.GetChunks();

            if (chunks == null)
                return;

            for (int i = 0; i < chunks.Count; i++)
            {
                for (int j = 0; j < chunks[i].Count; j++)
                {
                    Handles.Label(chunks[i][j].transform.position, chunks[i][j].GetDistanceToLimit().ToString());
                }
            }
        }
    }
}
