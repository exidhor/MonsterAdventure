using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace MonsterAdventure.Editor
{
    public class GizmosChunkManager
    {
        [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
        static void DrawGizmoForMyScript(ChunkManager chunkManager, GizmoType gizmoType)
        {
            List<List<Chunk>> chunks = chunkManager.GetChunks();

            if (chunks == null)
                return;

            if (chunkManager.drawGrid)
            {
                for (int i = 0; i < chunks.Count; i++)
                {
                    for (int j = 0; j < chunks[i].Count; j++)
                    {
                        Gizmos.DrawWireCube(chunks[i][j].transform.position, chunks[i][j].transform.lossyScale);
                    }
                }
            }

            if (chunkManager.drawColor)
            {
                for (int i = 0; i < chunks.Count; i++)
                {
                    for (int j = 0; j < chunks[i].Count; j++)
                    {
                        Gizmos.color = chunkManager.biomeManger.GetColor(chunks[i][j].GetBiomeType());
                        Gizmos.DrawCube(chunks[i][j].transform.position, chunks[i][j].transform.lossyScale);
                    }
                }
            }

            if (chunkManager.drawDistance)
            {
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
}
