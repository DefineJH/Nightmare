using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    public class MoveTo : TaskNode
    {
        public float Acceptable_Radius = .0f;

        [HideInInspector] public int keyIdx = 0;
        [HideInInspector] public string keyName;

        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate(BehaviorTreeComponent owner_comp)
        {
            var key = bBoard.bb_keys.Find(n => n.Name == keyName);
            Vector2 movePos = new Vector2();
            switch(key.Type)
            {
                case BT_Key.KeyType.E_vector2:
                    movePos = (Vector2)key.Value;
                    break;
                case BT_Key.KeyType.E_gameobject:
                    {
                        movePos = ((GameObject)key.Value).transform.position;
                    }
                    break;
                default:
                    break;
            }
            
            var pathManager = FindObjectOfType<Path.PathManager>();

            Path.Node start = pathManager.GetClosestNode(owner_comp.gameObject.transform.position);
            Path.Node end = pathManager.GetClosestNode(movePos);

            var path = pathManager.FindPath(start, end);

            var moveComp = owner_comp.gameObject.GetComponent<Movement>();

            moveComp.SetPath(path,Acceptable_Radius);
            return State.Succeeded;




        }
    }

}
