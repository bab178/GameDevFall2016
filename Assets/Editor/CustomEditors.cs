using Assets.Scripts;
using UnityEditor;

namespace Assets.EditorScripts
{
    [CustomEditor(typeof(PlayerController))]
    public class PlayerControllerEditor : Editor
    {
        Editor cachedEditor;
        bool cachedEditorNeedsRefresh = true;

        void OnEnable()
        {
            cachedEditor = null;
            cachedEditorNeedsRefresh = true;
        }

        public override void OnInspectorGUI()
        {
            PlayerController pc = (PlayerController)target;

            if (cachedEditorNeedsRefresh)
            {
                cachedEditor = Editor.CreateEditor(pc.PlayerStats);

                //Ensuring this is only run once.
                cachedEditorNeedsRefresh = false;
            }

            if(cachedEditor != null)
            {
                base.OnInspectorGUI();
                cachedEditor.DrawDefaultInspector();
            }
        }
    }

    [CustomEditor(typeof(HumanEnemyController))]
    public class HumanEnemyControllerEditor : Editor
    {
        Editor cachedEditor;
        bool cachedEditorNeedsRefresh = true;

        void OnEnable()
        {
            cachedEditor = null;
            cachedEditorNeedsRefresh = true;
        }

        public override void OnInspectorGUI()
        {
            HumanEnemyController hec = (HumanEnemyController)target;

            if (cachedEditorNeedsRefresh && hec != null)
            {
                cachedEditor = Editor.CreateEditor(hec.EnemyStats);

                //Ensuring this is only run once.
                cachedEditorNeedsRefresh = false;
            }

            if (cachedEditor != null)
            {
                base.OnInspectorGUI();
                cachedEditor.DrawDefaultInspector();
            }
        }
    }
}