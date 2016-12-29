using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace MonsterAdventure
{
    public class GizmosDrawer : MonoBehaviour
    {
        public delegate void GizmosFunction();

        private List<GizmosFunction> _gizmosFunctions = new List<GizmosFunction>();

        private void OnDrawGizmos()
        {
            foreach (GizmosFunction gizmoFunction in _gizmosFunctions)
            {
                gizmoFunction();
            }

            _gizmosFunctions.Clear();
        }

        public void Draw(GizmosFunction gizmosFunction)
        {
            _gizmosFunctions.Add(gizmosFunction);
        }
    }
}
