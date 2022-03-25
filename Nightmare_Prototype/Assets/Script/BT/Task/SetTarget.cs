using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BT
{
    public class SetTarget : TaskNode
    {
        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate(BehaviorTreeComponent owner_comp)
        {
            List<Units> targetList = null;
            if (owner_comp.gameObject.transform.tag == "Heros")
            {
                targetList = BattleManager.instance.GetMonstersList();
            }
            else
            {
                targetList = BattleManager.instance.GetHerosList();
            }

            float dis = float.MaxValue;

            for (int i = 0; i < targetList.Count; i++)
            {
                if (!bBoard.GetValueAsBool("IsDead"))
                {
                    float tmpDis = ((Vector2)targetList[i].transform.localPosition - (Vector2)owner_comp.gameObject.transform.localPosition).sqrMagnitude;
                    if (tmpDis < dis)
                    {
                        dis = tmpDis;
                        bBoard.SetValueAsGameObject("targetObj", targetList[i].gameObject);
                    }
                }
            }
            return State.Succeeded;
        }
    }
}
