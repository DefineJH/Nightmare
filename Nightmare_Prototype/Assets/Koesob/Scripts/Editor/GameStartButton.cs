using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace koesob
{
    [CustomEditor(typeof(GameManager))]
    public class GameStartButton : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GameManager gameManager = (GameManager)target;
            if (GUILayout.Button("Game Start"))
            {
                gameManager.GameStart();
            }
        }
    }
}


