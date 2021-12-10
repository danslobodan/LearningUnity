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
        DialogueNode draggingNode = null;
        Vector2 draggingOffset;

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

        private void OnGUI()
        {
            if (selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No Dialogue Selected");
                return;
            }

            ProcessEvents();

            selectedDialogue.Nodes.ToList().ForEach(node =>
            {
                DrawConnections(node);
            });
            selectedDialogue.Nodes.ToList().ForEach(node =>
            {
                DrawNode(node);
            });
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

        private void ProcessEvents()
		{
            if (Event.current.type == EventType.MouseDown && draggingNode == null)
			{
                draggingNode = GetNodeAtPoint(Event.current.mousePosition);
                draggingOffset = draggingNode != null
                    ? GetOffset(draggingNode)
                    : Vector2.zero;
            }
            else if (Event.current.type == EventType.MouseDrag && draggingNode != null)
            {
                OnDragNode();
            }
            else if (Event.current.type == EventType.MouseUp && draggingNode != null)
                draggingNode = null;
		}

        private DialogueNode GetNodeAtPoint(Vector2 mousePosition)
            => selectedDialogue.Nodes
                .LastOrDefault(node => 
                    node.rect.Contains(mousePosition));

        private Vector2 GetOffset(DialogueNode node)
            => node.rect.position - Event.current.mousePosition;

        private void OnDragNode()
		{
            Undo.RecordObject(selectedDialogue, "Move Dialogue");
            var mousePosition = Event.current.mousePosition;
            draggingNode.rect.position = mousePosition + draggingOffset;
            GUI.changed = true;
		}

		private void DrawNode(DialogueNode node)
		{
            GUILayout.BeginArea(node.rect, nodeStyle);
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

        private void DrawConnections(DialogueNode node)
        {
            selectedDialogue.GetChildren(node)
                .ToList()
                .ForEach(child =>
                {
                    Vector2 startPosition = new Vector2(node.rect.center.x, node.rect.yMax);
                    Vector2 endPosition = new Vector2(child.rect.center.x, child.rect.yMin);
                    Vector2 controlPointOffset = endPosition - startPosition;
                    controlPointOffset.x = 0;
                    controlPointOffset.y *= 0.8f;
                    Handles.DrawBezier(
                        startPosition, endPosition, 
                        startPosition + controlPointOffset, 
                        endPosition - controlPointOffset, 
                        Color.white, null, 4f);

                });
        }

    }
}