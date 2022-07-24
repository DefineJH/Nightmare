using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace koesob
{
    public class StageNode : MonoBehaviour
    {
        private int step;
        private bool isMerged;
        private bool canClicked;
        private bool isCompleted;
        private bool isPassPoint;
        private List<StageNode> nextStages;

        // Values for interaction with mouse
        private float distance;
        private float distanceThreshold;
        private Vector2 originPosition;

        // Values for spawning path prefab
        private float time;
        public float timeThreshold;
        public GameObject pathObject;
        private List<GameObject> pathObjects;
        private List<GameObject> inactiveObjects;

        // Value for instatiating lane prefab
        public GameObject lanePrefab;

        // Value for checking OnEnabled and frame; after Start() 
        private bool isFirstUpdate;

        public void Initialize(int _step)
        {
            this.step = _step;
            this.isMerged = false;
            this.canClicked = false;
            this.isCompleted = false;
            this.isPassPoint = false;

            this.nextStages = new List<StageNode>();
            this.pathObjects = new List<GameObject>();
            this.inactiveObjects = new List<GameObject>();

            this.distanceThreshold = GetSpriteSize(this.gameObject).x * 0.5f;

            if (this.step == 0)
            {
                canClicked = true;
            }
        }

        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        void Start()
        {
            this.isFirstUpdate = true;
            this.originPosition = this.Position;
        }

        void Update()
        {
            distance = (GetMouseWorldPosition() - originPosition).magnitude;

            if (isFirstUpdate)
            {
                InstantiateLaneObject();
                isFirstUpdate = false;
            }

            if (canClicked)
            {
                InteractWithMouse(Time.deltaTime);
            }

            time += Time.deltaTime;
            if (time >= timeThreshold)
            {
                CheckPathCondition();
                time = 0f;
            }
        }

        void OnDisable()
        {
            pathObjects.Clear();
            isFirstUpdate = true;
        }

        // Property를 활용한 접근 제한
        // 클래스 외부에서는 읽을 수만 있고, 클래스 내부에서는 step을 직접 할당함으로서 캡슐화
        public int Step
        {
            get { return this.step; }
        }

        public bool IsMerged
        {
            get { return this.isMerged; }
            set { this.isMerged = value; }
        }

        public bool IsCompleted
        {
            get { return this.isCompleted; }
        }

        public bool CanClicked
        {
            get { return this.canClicked; }
            set { this.canClicked = value; }
        }

        public bool IsPassPoint
        {
            get { return this.isPassPoint; }
        }

        public (bool _isComplete, int _step) StageInfo
        {
            get { return (this.isCompleted, this.step); }
        }

        public Vector2 Position
        {
            get { return this.gameObject.transform.position; }
        }

        public void AddNextStage(StageNode _nextStage)
        {
            if (!this.nextStages.Contains(_nextStage))
            {
                nextStages.Add(_nextStage);
            }
        }

        private void CheckPathCondition()
        {
            if (isPassPoint)
            {
                foreach (StageNode nextStage in nextStages)
                {
                    if ((nextStage.IsCompleted == false && nextStage.CanClicked == true) || nextStage.IsPassPoint == true)
                    {
                        InstantiatePathPrefab(nextStage);
                    }
                }
            }
        }
        private void InstantiatePathPrefab(StageNode _nextStageNode)
        {
            inactiveObjects = FindInactive(pathObjects, inactiveObjects);

            if (inactiveObjects.Count > 0)
            {
                inactiveObjects[inactiveObjects.Count - 1].gameObject.transform.position = this.gameObject.transform.position;
                inactiveObjects[inactiveObjects.Count - 1].SetActive(true);
                inactiveObjects.RemoveAt(inactiveObjects.Count - 1);
            }
            else
            {
                GameObject path = Instantiate(pathObject, this.gameObject.transform.position, Quaternion.identity);
                path.transform.localScale = this.transform.lossyScale * 0.5f;
                pathObjects.Add(path);

                PathObject pathObjectComponent = path.GetComponent<PathObject>();
                pathObjectComponent.SetTarget(_nextStageNode);
            }
        }

        private List<GameObject> FindInactive(List<GameObject> _gameObjects, List<GameObject> _inactiveObjects)
        {
            _inactiveObjects.Clear();

            if (_gameObjects.Count > 0)
            {
                foreach (GameObject gameObject in _gameObjects)
                {
                    if (gameObject == null)
                    {
                        Debug.Log("GameObject Null");
                    }
                    else
                    {
                        if (gameObject.activeSelf == false)
                        {
                            _inactiveObjects.Add(gameObject);
                        }
                    }
                }
            }

            return _inactiveObjects;
        }
        public void InstantiateLaneObject()
        {
            foreach (StageNode nextStage in nextStages)
            {
                // 시작 위치와 목표 위치 받아옴
                Vector2 startPosition = this.Position;
                Vector2 targetPosition = nextStage.Position;
                Vector2 instantiatePosition = (targetPosition + startPosition) / 2;
                Quaternion angle;
                Vector3 scale;

                // lanePrefab의 크기 가져오기
                Vector3 spriteSize = this.GetSpriteSize(lanePrefab);

                // Rotation 설정
                // Scale 설정

                // 각도는 몇인가?
                float radianAngle = Mathf.Atan2(targetPosition.y - startPosition.y, targetPosition.x - startPosition.x);
                float degreeAngle = 180 / Mathf.PI * radianAngle;

                // 빗변의 길이 구하기
                float length = (targetPosition.x - startPosition.x) / Mathf.Cos(radianAngle);
                // 목표 scale = 빗변의 길이 / 현재 길이
                // 더 긴 쪽을 기준으로 목표 scale 셋

                // x축으로 더 긴 오브젝트라면
                if (spriteSize.x > spriteSize.y)
                {
                    // 각도 그대로 z축 사용
                    angle = Quaternion.Euler(0f, 0f, degreeAngle);
                    scale = new Vector3(length / spriteSize.x, 1f, 1f);
                }
                //y 축으로 더 긴 오브젝트라면
                else
                {
                    // -(pi/2 - angle) 값으로 z축 회전 90도
                    angle = Quaternion.Euler(0f, 0f, -(90f - degreeAngle));
                    scale = new Vector3(1f, length / spriteSize.y, 1f);
                }

                // 중간 지점에서 Instantiate
                GameObject lane = Instantiate(lanePrefab, instantiatePosition, angle);
                lane.gameObject.transform.localScale = scale;
            }
        }

        private Vector2 GetMouseWorldPosition()
        {
            Vector2 mouseScreenPosition = Input.mousePosition;
            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

            return mouseWorldPosition;
        }

        private void InteractWithMouse(float _deltaTime)
        {
            if (distance < distanceThreshold)
            {
                this.gameObject.transform.position = Vector2.MoveTowards(this.transform.position, GetMouseWorldPosition(), 3f * _deltaTime);
            }
            else
            {
                this.gameObject.transform.position = originPosition;
            }
        }

        public void Complete()
        {
            this.isCompleted = true;
            this.isPassPoint = true;
            foreach (StageNode stage in nextStages)
            {
                stage.CanClicked = true;
            }
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

        public string ToLog()
        {
            string log = "Step : ";

            log += this.step + ", Path On : ";
            log += this.isPassPoint;
            log += ", Next Stages : ";

            foreach (StageNode stage in nextStages)
            {
                log += stage.gameObject.name;
                log += ", ";
            }

            return log;
        }
    }
}