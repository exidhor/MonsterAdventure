using System.Collections.Generic;
using System.Text;
using MonsterAdventure;
using UnityEditor;
using UnityEngine;

namespace MonsterAdventure.Editor
{
    class GizmosBaseManager
    {
        [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
        static void DrawGizmoForMyScript(BaseManager baseManager, GizmoType gizmoType)
        {
            if (!baseManager.drawGizmoBase)
                return;

            Dictionary<BaseType, List<Base>> basePerType = baseManager.GetBasesPerType();

            if (basePerType == null)
                return;

            Texture texture;
            List<Base> bases;

            Rect rect = new Rect();
            rect.width = 100;
            rect.height = 100;
            Vector2 offset = rect.size/2;

            foreach (BaseType baseType in basePerType.Keys)
            {
                bases = basePerType[baseType];

                foreach (Base currentBase in bases)
                {
                    texture = baseManager.GetTextureIcon(baseType);

                    Vector2 position = currentBase.transform.position;
                    position -= offset;

                    rect.position = position;

                    Gizmos.DrawGUITexture(rect, texture);
                }
            }
        }
    }
}
