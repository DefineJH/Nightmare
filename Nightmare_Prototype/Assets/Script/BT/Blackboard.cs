using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT_Key
{
    public enum KeyType
    {
        E_bool,
        E_int,
        E_float,
        E_vector2,
        E_gameobject
    }
};
[CreateAssetMenu()]
public class Blackboard : ScriptableObject
{
    public Dictionary<string, object> bb_keys = new Dictionary<string, object>();
    public Blackboard Clone()
    {
        Blackboard bBoard = Instantiate(this);
        return bBoard;
    }
    public bool GetValueAsBool(string str)
    {
        if (bb_keys.ContainsKey(str))
        {
            return (bool)bb_keys[str];
        }
        throw new System.Exception("Blackboard_Key doesnt exists");
    }

    public void SetValueAsBool(string str, bool val)
    {
        if (bb_keys.ContainsKey(str))
        {
            bb_keys[str] = val;
        }
        throw new System.Exception("Blackboard_Key doesnt exists");
    }

    public GameObject GetValueAsGameObject(string str)
    {
         if (bb_keys.ContainsKey(str))
        {
            return (GameObject)bb_keys[str];
        }
        throw new System.Exception("Blackboard_Key doesnt exists");
    }

    public void SetValueAsGameObject(string str, GameObject obj)
    {
        if (bb_keys.ContainsKey(str))
        {
             bb_keys[str] = obj;
        }
        throw new System.Exception("Blackboard_Key doesnt exists");
    }

    public Vector2 GetValueAsVector2(string str)
    {
        if (bb_keys.ContainsKey(str))
        {
            return (Vector2)bb_keys[str];
        }
        throw new System.Exception("Blackboard_Key doesnt exists");
    }
    public void GetValueAsVector2(string str, Vector2 val)
    {
        if (bb_keys.ContainsKey(str))
        {
            bb_keys[str] = val;
        }

        throw new System.Exception("Blackboard_Key doesnt exists");
    }
}
