using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace MonsterAdventure.Editor
{
    public class SectorCategory : Category
    {
        private SectorManager _sectorManager;

        private bool _drawGrid;
        private string[] _resolutionOptions = null;
        private int _resolutionIndex = 0;

        public SectorCategory(EditorWindow window, bool startingHidden = false) 
            : base("Sector", startingHidden)
        {
            // nothing ? 
        }

        protected override void DrawContent()
        {
            _drawGrid = EditorGUILayout.Toggle("Draw grid", _drawGrid);

            if (_resolutionOptions != null)
            {
                _resolutionIndex = EditorGUILayout.Popup(_resolutionIndex, _resolutionOptions);
            }
        }

        protected override bool TryToInit()
        {
            _sectorManager = GameObject.FindGameObjectWithTag("SectorManager").GetComponent<SectorManager>();

            if (!_sectorManager.IsInitialized())
            {
                return false;
            }

            List<List<Sector>> sectors = _sectorManager.GetSectors();
            _resolutionOptions = new string[sectors.Count];

            for (int i = 0; i < sectors.Count; i++)
            {
                _resolutionOptions[i] = GetResolutionName(sectors[i].Count);
            }

            return true;
        }

        protected override void UpdateContent()
        {
            // todo
        }

        protected override void ResetContent()
        {
            _resolutionOptions = null;
        }

        public override void DrawGizmosContent()
        {
            if (_drawGrid)
            {
                _gizmosDrawer.Draw(DrawGrid);
            }
        }

        private string GetResolutionName(int count)
        {
            int name = (int)Math.Sqrt(count);

            return name + "x" + name;
        }

        private void DrawGrid()
        {
            Color gridColor = Color.gray;

            List<Sector> currentSectors = _sectorManager.GetSectors()[_resolutionIndex];

            foreach (Sector sector in currentSectors)
            {
                GizmosHelper.DrawRect(sector.GetBounds(), gridColor, 1);
            }
        }
    }
}
