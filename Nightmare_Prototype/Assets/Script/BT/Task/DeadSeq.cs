﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BT
{
    public class DeadSeq : TaskNode
    {
        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate(BehaviorTreeComponent owner_comp)
        {
            return State.Succeeded;
        }
    }

}

