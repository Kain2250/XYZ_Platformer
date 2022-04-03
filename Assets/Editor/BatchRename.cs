using UnityEditor;
using Object = UnityEngine.Object;

namespace Editor
{
    public class BatchRename : ScriptableWizard
    {
        public string _baseName = "MyObject_";

        public int _startNumber = 0;

        public int _increment = 1;

        [MenuItem("Edit/ Batch Rename...")]
        static void CreateWizard()
        {
            ScriptableWizard.DisplayWizard("Batch Rename", typeof(BatchRename), "Rename");
        }

        private void OnEnable()
        {
            UpdateSelectionHelper();
        }

        private void OnSelectionChange()
        {
            UpdateSelectionHelper();
        }

        void UpdateSelectionHelper()
        {
            helpString = "";

            if (Selection.objects != null)
                helpString = "Number of objects selected: " + Selection.objects.Length;
        }

        private void OnWizardCreate()
        {
            if (Selection.objects == null)
                return;

            int postFix = _startNumber;

            foreach (Object obj in Selection.objects)
            {
                obj.name = _baseName + postFix;
                postFix += _increment;
            }
        }
    }
}