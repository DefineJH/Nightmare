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
            Blackboard bb = owner_comp.TreeObject.bBoard;
            Units targetObj = bb.GetValueAsGameObject("targetObj").GetComponent<Units>();
            float dmg = bb.GetValueAsFloat("Damage");
            Debug.Log(owner_comp.gameObject.name + " Attack " + targetObj.gameObject.name + " with " + dmg);

            owner_comp.gameObject.GetComponent<Units>().PlayAttackAnimation();
            targetObj.GetDamage(dmg);
            return State.Succeeded;
        }
    }

}
