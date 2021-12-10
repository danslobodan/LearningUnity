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
        DialogueNode linkingNode = null;
        Vector2 scrollPosition;
        bool draggingCanvas = false;
        Vector2 draggingCanvasOffset;

        const float canvasSize = 4000f;
        const float backgroundSize = 50f;

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

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            var canvas = GUILayoutUtility.GetRect(canvasSize, canvasSize);
            var texture = Resources.Load("background") as Texture2D;
            var texCoords = new Rect(0, 0, canvasSize / backgroundSize, canvasSize / backgroundSize);
            GUI.DrawTextureWithTexCoords(canvas, texture, texCoords);

            selectedDialogue.Nodes.ToList().ForEach(node =>
            {
                DrawConnections(node);
            });
            selectedDialogue.Nodes.ToList().ForEach(node =>
            {
                DrawNode(node);
            });
            
            EditorGUILayout.EndScrollView();
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
                draggingNode = GetNodeAtPoint(RelativeMousePosition);
                if (draggingNode != null)
				{
                    draggingOffset = GetOffset(draggingNode.rect.position);
                    Selection.activeObject = draggingNode;
				}
                else
				{
                    draggingCanvas = true;
                    draggingCanvasOffset = RelativeMousePosition;
                    Selection.activeObject = selectedDialogue;
                }
            }
            else if (Event.current.type == EventType.MouseDrag && draggingNode != null)
            {
                OnDragNode();
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseDrag && draggingCanvas)
			{
                scrollPosition = draggingCanvasOffset - Event.current.mousePosition;
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseUp && draggingNode != null)
			{
                draggingCanvas = false;
                draggingNode = null;
			}
		}

        private Vector2 RelativeMousePosition
            => Event.current.mousePosition + scrollPosition;

        private DialogueNode GetNodeAtPoint(Vector2 position)
            => selectedDialogue.Nodes
                .LastOrDefault(node => 
                    node.rect.Contains(position));

        private Vector2 GetOffset(Vector2 position)
            => position - Event.current.mousePosition;

        private void OnDragNode()
		{
            Undo.RecordObject(selectedDialogue, "Move Dialogue");
            var mousePosition = Event.current.mousePosition;
            draggingNode.rect.position = mousePosition + draggingOffset;
		}

		private void DrawNode(DialogueNode node)
		{
            GUILayout.BeginArea(node.rect, nodeStyle);
			EditorGUI.BeginChangeCheck();

			var text = EditorGUILayout.TextField(node.text);

			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(selectedDialogue, "Updated Dialogue Text");
				node.text = text;
			}

            GUILayout.BeginHorizontal();

            if(GUILayout.Button("-"))
			{
                Undo.RecordObject(selectedDialogue, "Removed Dialogue Node");
                selectedDialogue.DeleteNode(node);
			}


            if (linkingNode == null)
            {
                if (GUILayout.Button("link"))
                {
                    linkingNode = node;
                }
            }
            else if (linkingNode == node)
            {
                if (GUILayout.Button("cancel"))
                {
                     linkingNode = null;
                }
            }
            else if (linkingNode.Children.Contains(node.name))
			{
                if (GUILayout.Button("unlink"))
                {
                    Undo.RecordObject(selectedDialogue, "Removed Dialogue Link");
                    linkingNode.Children.Remove(node.name);
                    linkingNode = null;
                }
            }
            else { 
                if (GUILayout.Button("child"))
                {
                    Undo.RecordObject(selectedDialogue, "Added Dialogue Link");
                    linkingNode.Children.Add(node.name);
                    linkingNode = null;
                }
			}

            if (GUILayout.Button("+"))
			{
                Undo.RecordObject(selectedDialogue, "Added Dialogue Node");
                selectedDialogue.CreateNode(node);
			}

            GUILayout.EndHorizontal();

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