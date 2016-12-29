using UnityEngine;
using UnityEditor;
using System.Collections;

namespace MonsterAdventure.Editor
{
    /*
     * \brief   [ABSTRACT] This represent a part of available option.
     *          It provide a Foldout system.
     *          It need to override "void DrawContent()".
     */
    public abstract class Category
    {
        private bool _isInitialize = false;

        private bool showOptions = true;
        private string labelMenu;
        protected GizmosDrawer _gizmosDrawer;

        /*
         * \brief   Construct the Option.
         * \param   a_Label The title of the option.
         * \param   a_Window The window where the option will be.
         * \param   a_StartingValue if the option sart "hidden" (false)
         *          or not (true)
         */
        public Category(string label, bool startingHidden = false)
        {
            showOptions = !startingHidden;
            labelMenu = label;
        }


        /*
         * \brief   Draw the option and provide the foldout system.
         */
        public void Draw()
        {
            showOptions = EditorGUILayout.Foldout(showOptions,
                labelMenu);

            // change the style for the title.
            EditorStyles.foldout.fontSize = 14;
            EditorStyles.foldout.fontStyle = FontStyle.Bold;
   
            EditorGUI.indentLevel++;
            if (showOptions)
            {
                DrawContent();
            }
            EditorGUI.indentLevel--;
        }


        /*
         * \brief   [ABSTRACT PURE] Where the content
         *          has to be implement.
         *          [CARREFUL] This method doesn't need to be called
         *          because it is in the "void Draw()" method.
         */
        protected abstract void DrawContent();

        protected abstract bool TryToInit();

        private void Initialize()
        {
            if (TryToInit())
            {
                _gizmosDrawer = GameObject.FindGameObjectWithTag("GizmosDrawer").GetComponent<GizmosDrawer>();
                _isInitialize = true;
            }
        }

        public void Update()
        {
            if (!_isInitialize)
            {
                Initialize();
            }

            if (_isInitialize)
            {
                DrawGizmosContent();
                UpdateContent();
            }
        }

        protected abstract void UpdateContent();

        public void Reset()
        {
            _isInitialize = false;

            ResetContent();
        }

        protected abstract void ResetContent();

        public abstract void DrawGizmosContent();
    }
}
