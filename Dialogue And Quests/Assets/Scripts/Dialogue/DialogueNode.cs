using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] bool isPlayerSpeaking = false;
        [SerializeField] string text;
        [SerializeField] List<string> children = new List<string>();
        [SerializeField] Rect rect = new Rect(0, 0, 200, 100);

        public bool IsPlayerSpeaking => isPlayerSpeaking;
        public IEnumerable<string> Children => children;
        public string Text => text;

        public Rect Rect => rect;

#if UNITY_EDITOR

        public void SetIsPlayerSpeaking(bool isPlayerSpeaking)
		{
            Undo.RecordObject(this, "Change Dialogue Speaker");
            this.isPlayerSpeaking = isPlayerSpeaking;
            EditorUtility.SetDirty(this);
        }

        public void SetPosition(Vector2 position)
		{
            Undo.RecordObject(this, "Move Dialogue");
            rect.position = position;
            EditorUtility.SetDirty(this);
		}

        public void SetText(string text)
		{
            if (this.text == text)
                return;

            Undo.RecordObject(this, "Updated Dialogue Text");
            this.text = text;
            EditorUtility.SetDirty(this);
        }

        public void AddChild(string childID)
		{
            if (children.Contains(childID))
                return;

            Undo.RecordObject(this, "Added Dialogue Link");
            children.Add(childID);
            EditorUtility.SetDirty(this);
        }

        public void RemoveChild(string childID)
		{
            if (!children.Contains(childID))
                return;

            Undo.RecordObject(this, "Removed Dialogue Link");
            children.Remove(childID);
            EditorUtility.SetDirty(this);
        }
#endif
    }
}