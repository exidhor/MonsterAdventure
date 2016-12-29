using UnityEditor;
using UnityEngine;

namespace MonsterAdventure.Editor
{
    public class GeneratorWindow : EditorWindow
    {
        private static GeneratorWindow _instance;

        private SectorCategory _sectorCategory;
        private Vector2 _positionForScrollView;

        private bool _gameIsRunning = false;

        [MenuItem("Window/Generator")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            _instance = (GeneratorWindow) EditorWindow.GetWindow(typeof(GeneratorWindow));
            _instance.titleContent = new GUIContent("Generator");

            _instance.Show();
        }

        /// <summary>
        /// Construct and initialize parameters.
        /// This function is called when the object is loaded.
        /// </summary>
        private void OnEnable()
        {
            _sectorCategory = new SectorCategory(_instance);
        }

        /// <summary>
        /// Draw the interface into Unity.
        /// </summary>
        private void OnGUI()
        {
            EditorGUILayout.Separator();

            _positionForScrollView = EditorGUILayout.BeginScrollView(_positionForScrollView);
            {
                _sectorCategory.Draw();
            }
            EditorGUILayout.EndScrollView();
        }

        private void Update()
        {
            if (EditorApplication.isPlaying && !EditorApplication.isPaused && !_gameIsRunning)
            {
                _gameIsRunning = true;
            }
            else if (!EditorApplication.isPlaying)
            {
                _gameIsRunning = false;
                _sectorCategory.Reset();
            }

            if (_gameIsRunning)
            {
                _sectorCategory.Update();
            }
        }

        private void OnDrawGizmos()
        {
            _sectorCategory.DrawGizmosContent();
        }
    }
}