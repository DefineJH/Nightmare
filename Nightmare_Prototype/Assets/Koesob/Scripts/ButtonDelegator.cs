using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace koesob
{
    public class ButtonDelegator : MonoBehaviour
    {
        public void StartToStage()
        {
            GameManager.Instance.GameStart();
        }
        public void StageToStart()
        {
            GameManager.Instance.StageToStartScene();
        }
        public void CompleteClicked()
        {
            BattleManager.Instance.Complete();

        }

        public void UnCompleteClicked()
        {
            BattleManager.Instance.Uncomplete();
        }

        public void EndToStart()
        {
            GameManager.Instance.EndToStart();
        }
    }
}

