using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] string text;
        [SerializeField] List<string> children = new List<string>();
        [SerializeField] Rect rect = new Rect(0, 0, 200, 100);

        public IEnumerable<string> Children => children;
        public string Text => text;

        public Rect Rect => rect;

#if UNITY_EDITOR
        public void SetPosition(Vector2 position)
		{
            Undo.RecordObject(this, "Move Dialogue");
            rect.position = position;
		}

        public void SetText(string text)
		{
            if (this.text == text)
                return;

            Undo.RecordObject(this, "Updated Dialogue Text");
            this.text = text;
		}

        public void AddChild(string childID)
		{
            if (children.Contains(childID))
                return;

            Undo.RecordObject(this, "Added Dialogue Link");
            children.Add(childID);
		}

        public void RemoveChild(string childID)
		{
            if (!children.Contains(childID))
                return;

            Undo.RecordObject(this, "Removed Dialogue Link");
            children.Remove(childID);
		}
#endif
    }
}