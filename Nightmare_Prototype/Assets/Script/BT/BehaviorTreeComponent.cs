using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTreeComponent : MonoBehaviour
{
    public BT.BehaviorTree TreeObject;
    public Blackboard bBoard;
    void Start()
    {
        TreeObject = TreeObject.Clone();
        bBoard = bBoard.Clone();
        TreeObject.BindBlackBoard(bBoard);
    }

    // Update is called once per frame
    void Update()
    {
        TreeObject.Update();
    }
}
