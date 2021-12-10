using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPG.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        Dialogue selectedDialogue = null;
        GUIStyle nodeStyle;

		private void OnEnable()
		{
            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            nodeStyle.padding = new RectOffset(20, 20, 20, 20);
            nodeStyle.border = new RectOffset(12, 12, 12, 12);
		}

		private void OnSelectionChange()
        {
            UpdateSelected();
        }

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow<DialogueEditor>("Dialogue Editor");
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            var dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
            if (dialogue == null)
                return false;

            ShowEditorWindow();

            return true;
        }

        private void UpdateSelected()
        {
            var dialogue = Selection.activeObject as Dialogue;
            if (dialogue == null)
                return;

            selectedDialogue = dialogue;
            Repaint();
        }

        private void OnGUI() {
            if (selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No Dialogue Selected");
                return;
            }

			selectedDialogue.Nodes.ToList().ForEach(node =>
			{
				OnGUINode(node);
			});
        }

		private void OnGUINode(DialogueNode node)
		{
            GUILayout.BeginArea(node.position, nodeStyle);
			EditorGUI.BeginChangeCheck();

			EditorGUILayout.LabelField("Node: ");
			var uniqueID = EditorGUILayout.TextField(node.uniqueID);
			var text = EditorGUILayout.TextField(node.text);

			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(selectedDialogue, "Update Dialogue Text.");
				node.text = text;
				node.uniqueID = uniqueID;
			}
            GUILayout.EndArea();
		}

	}
}