using System.Collections.Generic;
using System.Text;
using MonsterAdventure;
using UnityEditor;
using UnityEngine;

namespace MonsterAdventure.Editor
{
    class GizmosPlaceManager
    {
        [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
        static void DrawGizmoForMyScript(PlaceManager baseManager, GizmoType gizmoType)
        {
            if (!baseManager.drawGizmoPlace)
                return;

            Dictionary<PlaceType, List<Place>> basePerType = baseManager.GetPlacesPerType();

            if (basePerType == null)
                return;

            Texture texture;
            List<Place> bases;

            Rect rect = new Rect();
            rect.width = 10;
            rect.height = 10;
            Vector2 offset = rect.size/2;

            foreach (PlaceType baseType in basePerType.Keys)
            {
                bases = basePerType[baseType];

                foreach (Place currentPlace in bases)
                {
                    texture = baseManager.GetTextureIcon(baseType);

                    Vector2 position = currentPlace.transform.position;
                    position -= offset;

                    rect.position = position;

                    Gizmos.DrawGUITexture(rect, texture);
                }
            }
        }
    }
}
