using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    public class AttackTarget : TaskNode
    {
        protected override void OnStart()
        {
            Debug.Log("Start attack");
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate(BehaviorTreeComponent owner_comp)
        {
            owner_comp.gameObject.GetComponent<Movement>().TempAttack();
            return State.Succeeded;
        }
    }

}
