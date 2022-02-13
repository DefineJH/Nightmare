using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    public abstract class ServiceNode : Node
    {
        [HideInInspector] public Node Child;
        Blackboard bBoard;
        public override Node Clone()
        {
            ServiceNode node = Instantiate(this);
            node.Child = Child.Clone();
            return node;
        }
    }

}
