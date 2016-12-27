using UnityEditor;
using UnityEngine;

namespace MonsterAdventure.Editor
{
    public class GeneratorWindow : EditorWindow
    {
        private static GeneratorWindow _instance;

        [MenuItem("Window/Generator")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            _instance = (GeneratorWindow)EditorWindow.GetWindow(typeof(GeneratorWindow));
            _instance.titleContent = new GUIContent("Generator");

            _instance.Show();
        }

        private void OnGUI()
        {
            // todo
        }
    }
}
