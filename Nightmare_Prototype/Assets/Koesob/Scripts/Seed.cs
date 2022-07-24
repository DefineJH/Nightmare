using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace koesob
{
    public partial class StageManager : MonoBehaviour
    {
        public class Seed
        {
            private int index;
            private int step;
            private Vector2 position;
            private List<int> nextStage;
            private Seed pointer;

            public Seed(int _index, int _step)
            {
                this.index = _index;
                this.step = _step;
                this.nextStage = new List<int>();

            }

            public string ToLog()
            {
                string log = string.Format("index : {0}, step : {1}, Position : x={2}, y={3}", index, step, position.x, position.y);
                log += ", ";
                string next = "Next Stage : ";
                string nextPointer = "Pointer : ";

                foreach (int nextIndex in this.nextStage)
                {
                    next += nextIndex;
                }
                next += ", ";

                if (this.pointer != null)
                {
                    nextPointer += this.pointer.index.ToString();
                }
                else
                {
                    nextPointer += "this";
                }

                return log + next + nextPointer;
            }

            public int GetIndex()
            {
                return this.index;
            }

            public int GetStep()
            {
                return this.step;
            }

            public Vector2 GetPosition()
            {
                return this.position;
            }

            public void SetPosition(float _height, float _width)
            {
                float xPosition = this.step * _width;
                float yPosition = this.index * _height;
                position = new Vector2(xPosition, yPosition);
            }

            public List<int> GetNextStage()
            {
                return this.nextStage;
            }

            public void SetNextStage(int _index)
            {
                if (!nextStage.Contains(_index))
                {
                    this.nextStage.Add(_index);
                }
            }

            public bool isMerged()
            {
                if (pointer == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            public void RandomizePosition(float _height, float _width)
            {
                float xPosition = this.step * _width + Random.Range(0f, _width / 2);
                float yPosition = this.index * _height + Random.Range(0f, _height / 2);
                position = new Vector2(xPosition, yPosition);
            }

            public void Merge(Seed _to)
            {
                this.pointer = _to;
                // to의 nextStage를 바꿔줘야 한다.
                foreach (int index in this.nextStage)
                {
                    if (!_to.nextStage.Contains(index))
                    {
                        _to.nextStage.Add(index);
                    }
                }
            }

            public int GetResultPointer()
            {
                int resultIndex = this.index;

                if (this.pointer != null)
                {
                    resultIndex = this.pointer.GetResultPointer();
                }

                return resultIndex;
            }
        }
    }
}