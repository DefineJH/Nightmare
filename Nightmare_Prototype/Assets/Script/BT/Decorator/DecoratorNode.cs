using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    public abstract class DecoratorNode : Node
    {
        [HideInInspector] public Node Child;
        Blackboard bBoard;
        public override Node Clone()
        {
            DecoratorNode node = Instantiate(this);
            node.Child = Child.Clone();
            return node;
        }
    }
}
