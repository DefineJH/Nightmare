using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestJson
{
    public class TestStageManager : MonoBehaviour
    {
        public int levelNumber;
        public int selectionNumber;
        public TestNode nodeObject;

        private void Start()
        {
            GenerateStage(selectionNumber, levelNumber);
        }

        private void GenerateStage(int _selectionNumber, int _levelNumber)
        {
            for (int index = 0; index < _selectionNumber; index++)
            {
                for (int level = 0; level < _levelNumber; level++)
                {
                    Vector2 nodePosition = new Vector2(level * 10, index * 10);
                    TestNode stageNode = Instantiate(nodeObject, nodePosition, Quaternion.identity, this.transform);
                    stageNode.NodeInit(level, nodePosition);
                }
            }
        }
    }

}
