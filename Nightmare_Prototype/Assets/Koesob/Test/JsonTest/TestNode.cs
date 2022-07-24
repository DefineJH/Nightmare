using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestJson
{
    [System.Serializable]
    public class TestNode : MonoBehaviour
    {
        private int level;
        private List<int> nextNodes;
        private bool isClear;
        private Vector2 position;
        private List<Enemy> enemies;

        public void NodeInit(int _level, Vector2 _nodePosition)
        {
            level = _level;
            position = _nodePosition;
            isClear = false;
            nextNodes = new List<int>();
            enemies = new List<Enemy>();
        }
    }
}

