using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BlackboardKeyTypes : ScriptableObject
{
    public string Name;
    public BT_Key.KeyType Type;
    public object Value;

    public BlackboardKeyTypes Clone()
    {
        return Instantiate(this);
    }

   
}
