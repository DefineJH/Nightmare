using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace koesob
{
    public class PathObject : MonoBehaviour
    {
        private Vector2 startPoint;
        private Vector2 finishLine;
        private StageNode targetNode;

        void Update()
        {
            MoveTo();
            Finished();
        }

        public void SetTarget(StageNode _targetNode)
        {
            targetNode = _targetNode;
        }

        private void MoveTo()
        {
            Rigidbody2D pathRigid = this.gameObject.GetComponent<Rigidbody2D>();


            // nextStageNode로 향하는 Vector를 구한다.
            Vector2 directionVector = this.targetNode.gameObject.transform.position - this.gameObject.transform.position;
            directionVector = directionVector.normalized * 2f;

            pathRigid.velocity = directionVector;
        }

        private void Finished()
        {
            finishLine = targetNode.gameObject.transform.position;

            if (this.gameObject.transform.position.x >= finishLine.x)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}


