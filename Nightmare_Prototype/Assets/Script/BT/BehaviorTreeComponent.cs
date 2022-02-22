using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTreeComponent : MonoBehaviour
{
    public BT.BehaviorTree TreeObject;
    void Start()
    {
        TreeObject = TreeObject.Clone();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
            TreeObject.bBoard.SetValueAsBool("Temp", true);
        else
            TreeObject.bBoard.SetValueAsBool("Temp", false);
        TreeObject.Update(this);
    }

}
