using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    private Type type;
    private enum Type
    {
        Normal,
        Elite,
        Boss
    }
    
    private Vector3 Postion;

    public void SetEnemyType(int _typeNum)
    {
        switch (_typeNum)
        {
            case 0:
                this.type = Type.Normal;
                break;
            case 1:
                this.type = Type.Elite;
                break;
            case 2:
                this.type = Type.Boss;
                break;
            default:
                break;
        }
    }

    public string GetEnemyType()
    {
        return this.type.ToString();
    }

    public Vector3 SetPosition(Vector3 _position)
    {
        this.Postion = _position;

        return this.Postion;
    }

    public Vector3 GetPosition()
    {
        return this.Postion;
    }
}
