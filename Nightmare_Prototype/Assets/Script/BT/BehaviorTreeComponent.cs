﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTreeComponent : MonoBehaviour
{
    public BT.BehaviorTree TreeObject;
    void Start()
    {
        TreeObject = TreeObject.Clone();
        TreeObject.bBoard.SetValueAsBool("CanAttack", false);
        TreeObject.bBoard.SetValueAsGameObject("targetObj", GameObject.Find("HealthPotion"));
    }

    // Update is called once per frame
    void Update()
    {
        TreeObject.UpdateTree(this);


    }

}
