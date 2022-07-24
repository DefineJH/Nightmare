using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace koesob
{
    public class BattleManager : MonoBehaviour
    {
        private static BattleManager instance = null;
        private StageNode currentStageNode;
        public delegate void BattleEnd((bool _isComplete, int _step) _stageInfo);
        public event BattleEnd battleEndDelegate;
        public static BattleManager Instance
        {
            get
            {
                if (instance == null)
                {
                    return null;
                }
                else
                {
                    return instance;
                }
            }
        }

        void Awake()
        {
            if (instance == null)
            {
                instance = this;

                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        public void LinkeToStageNode(StageNode _stageNode)
        {
            currentStageNode = _stageNode;
        }

        public void GenerateBattle(int _stageInfo)
        {
            Debug.Log(_stageInfo);
        }

        public void Complete()
        {
            Debug.Log("Complete");

            currentStageNode.Complete();
            StageManager.Instance.ClearStep(currentStageNode.Step);
            Debug.Log(StageManager.Instance.stageList.Count);
            battleEndDelegate(currentStageNode.StageInfo);
        }

        public void Uncomplete()
        {
            Debug.Log("UnComplete");
            battleEndDelegate(currentStageNode.StageInfo);
        }
    }
}

