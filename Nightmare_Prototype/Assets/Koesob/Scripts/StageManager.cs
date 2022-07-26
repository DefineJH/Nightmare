using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace koesob
{
    public partial class StageManager : MonoBehaviour
    {
        [System.Serializable]
        public class Stages
        {
            public List<StageNode> stages;

            public Stages()
            {
                stages = new List<StageNode>();
            }
        }
    }

    [System.Serializable]
    public partial class StageManager : MonoBehaviour
    {
        private static StageManager instance = null;
        private List<List<Seed>> seedList;
        public List<Stages> stageList;
        public StageNode stageNodeObject;
        private float stageNodeScale;
        public static StageManager Instance
        {
            get
            {
                if (instance == null)
                {
                    return null;
                }
                else
                {
                    return instance;
                }
            }
        }

        void Awake()
        {
            if (instance == null)
            {
                instance = this;

                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Debug.Log("Destroyed");
                Destroy(this.gameObject);
            }
        }

        public void GenerateStage(int _startCount, int _stepCount, float _height, float _width)
        {
            seedList = InitializeSeed(_startCount, _stepCount);
            seedList = RandomizeSeed(seedList);
            seedList = RandomizePosition(seedList, _height, _width);

            stageNodeScale = (_width * 0.25f) / GetSpriteSize(stageNodeObject.gameObject).x;

            stageList = GenerateNode(seedList);
            stageList = SetPath(stageList, seedList);
        }

        private List<List<Seed>> InitializeSeed(int _startCount, int _stepCount)
        {
            List<List<Seed>> resultList = new List<List<Seed>>();

            for (int step = 0; step < _stepCount; step++)
            {
                List<Seed> stepList = new List<Seed>();

                for (int index = 0; index < _startCount; index++)
                {
                    Seed initSeed = new Seed(index, step);
                    stepList.Add(initSeed);
                }

                resultList.Add(stepList);
            }

            return resultList;
        }

        private List<List<Seed>> RandomizeSeed(List<List<Seed>> _seedList)
        {
            foreach (List<Seed> stepList in _seedList)
            {
                if (stepList[0].GetStep() != _seedList.Count - 1)
                {
                    // 시작과 끝 사이에 있는 단계 처리
                    if (stepList[0].GetStep() != 0)
                    {
                        foreach (Seed seed in stepList)
                        {
                            seed.SetNextStage(seed.GetIndex());
                        }

                        int count = Random.Range(0, stepList.Count - 1);
                        List<int> randomIndex = GetRandomIndex(stepList.Count - 1, count);

                        foreach (int index in randomIndex)
                        {
                            int toIndex = stepList[index + 1].GetResultPointer();
                            stepList[index].Merge(stepList[toIndex]);
                        }
                    }
                    // 시작 단계 처리
                    else
                    {
                        foreach (Seed seed in stepList)
                        {
                            seed.SetNextStage(seed.GetIndex());
                        }
                    }
                }
                // 마지막 단계 = 보스 스테이지 처리
                else
                {
                    int count = stepList.Count - 1;
                    List<int> randomIndex = GetRandomIndex(stepList.Count - 1, count);

                    foreach (int index in randomIndex)
                    {
                        int toIndex = stepList[index + 1].GetResultPointer();
                        stepList[index].Merge(stepList[toIndex]);
                    }
                }
            }

            return _seedList;
        }

        private List<List<Seed>> RandomizePosition(List<List<Seed>> _seedList, float _height, float _width)
        {
            foreach (List<Seed> stepList in _seedList)
            {
                foreach (Seed seed in stepList)
                {
                    seed.SetPosition(_height, _width);
                    seed.RandomizePosition(_height, _width);
                }
            }

            return _seedList;
        }

        private List<Stages> GenerateNode(List<List<Seed>> _seedList)
        {
            List<Stages> resultList = new List<Stages>();

            foreach (List<Seed> stepList in _seedList)
            {
                Stages stageStepList = new Stages();

                foreach (Seed seed in stepList)
                {

                    Vector2 position = seed.GetPosition() + new Vector2(this.transform.position.x, this.transform.position.y);
                    int step = seed.GetStep();
                    int index = seed.GetIndex();

                    StageNode stageNode = Instantiate(stageNodeObject, position, Quaternion.identity, this.transform);
                    // Seed로부터 StageNode 정보 불러오기
                    stageNode.name = string.Format("Step:{0} Index:{1}", step, index);
                    stageNode.gameObject.transform.localScale = new Vector3(stageNodeScale, stageNodeScale, 1f);
                    // Seed를 이용해서 StageNode 초기화
                    stageNode.Initialize(step);

                    stageStepList.stages.Add(stageNode);

                    if (seed.isMerged())
                    {
                        stageNode.IsMerged = seed.isMerged();
                        stageNode.gameObject.SetActive(false);
                    }
                }

                resultList.Add(stageStepList);
            }

            return resultList;
        }

        private List<Stages> SetPath(List<Stages> _stageList, List<List<Seed>> _seedList)
        {
            foreach (List<Seed> stepList in _seedList)
            {
                foreach (Seed seed in stepList)
                {
                    if (!seed.isMerged())
                    {
                        int currentStep = seed.GetStep();
                        int currentIndex = seed.GetIndex();
                        int nextStep = currentStep + 1;

                        // seed의 nextStage를 살펴본다.
                        foreach (int targetIndex in seed.GetNextStage())
                        {
                            // 있다면, seed.step+1 영역에서 해당 seed 확인, getResultPointer로 최종 인덱스를 받아온다.
                            int resultIndex = _seedList[nextStep][targetIndex].GetResultPointer();

                            // 동일 step, index의 stageNode를 찾아서 위에서 구한 최종 index를 더해준다.
                            _stageList[currentStep].stages[currentIndex].AddNextStage(_stageList[nextStep].stages[resultIndex]);
                        }
                    }
                }
            }

            return _stageList;
        }

        private Vector3 GetSpriteSize(GameObject _sprite)
        {
            Vector3 worldSize = Vector3.zero;

            if (_sprite.GetComponent<SpriteRenderer>())
            {
                Vector2 spriteSize = _sprite.GetComponent<SpriteRenderer>().sprite.rect.size;
                Vector2 localSpriteSize = spriteSize / _sprite.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
                worldSize = localSpriteSize;
                worldSize.x *= _sprite.transform.lossyScale.x;
                worldSize.y *= _sprite.transform.lossyScale.y;
            }
            else
            {
                Debug.Log("SpriteRenderer Null");
            }

            return worldSize;
        }

        public void InactivateStages()
        {
            foreach (Stages stages in stageList)
            {
                foreach (StageNode stage in stages.stages)
                {
                    stage.gameObject.SetActive(false);
                }
            }
        }

        public void ActivateStages()
        {
            foreach (Stages stages in stageList)
            {
                foreach (StageNode stage in stages.stages)
                {
                    if (!stage.IsMerged)
                    {
                        stage.gameObject.SetActive(true);
                    }
                }
            }
        }

        public void ClearStep(int _step)
        {
            foreach (Stages stages in stageList)
            {
                foreach (StageNode stage in stages.stages)
                {
                    if (stage.Step == _step)
                    {
                        stage.CanClicked = false;
                    }
                }
            }
        }

        private void SeedLog(List<List<Seed>> _seedList)
        {
            foreach (List<Seed> stepList in _seedList)
            {
                foreach (Seed seed in stepList)
                {
                    Debug.Log(seed.ToLog());
                }
            }
        }

        private void NodeLog(List<List<StageNode>> _stageList)
        {
            foreach (List<StageNode> stepList in _stageList)
            {
                foreach (StageNode stage in stepList)
                {
                    Debug.Log(stage.ToLog());
                }
            }
        }

        // 크기와 갯수를 주면, 해당 크기 내에서 갯수만큼 랜덤하게 
        private List<int> GetRandomIndex(int _size, int _count)
        {
            List<int> randomIndex = new List<int>();

            for (int i = 0; i < _count; i++)
            {
                int index = Random.Range(0, _size);
                while (randomIndex.Contains(index))
                {
                    index = Random.Range(0, _size);
                }
                randomIndex.Add(index);
            }

            return randomIndex;
        }
    }
}