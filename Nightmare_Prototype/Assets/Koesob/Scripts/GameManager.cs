using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace koesob
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance = null;
        public int startCount;
        public int stepCount;
        private float height;
        private float width;
        private float xHalfScreenSize;
        private float yHalfScreenSize;

        public StageManager stageManagerPrefab;
        private StageManager stageManager;

        private BattleManager battleManager;
        private int currentSceneNumber;
        public static GameManager Instance
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

        // Start is called before the first frame update
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }


        void Update()
        {
            currentSceneNumber = SceneManager.GetActiveScene().buildIndex;

            switch (currentSceneNumber)
            {
                case 1:
                    StageToBattleScene();
                    break;
                // case 1:
                //     BattleToStageScene();
                //     break;
                default:
                    break;
            }
        }

        private void GenerateBattleManager()
        {
            if (!battleManager)
            {
                GameObject battleManagerObject = new GameObject();
                battleManagerObject.name = "Battle Manager";
                battleManagerObject.AddComponent<BattleManager>();
                battleManager = battleManagerObject.GetComponent<BattleManager>();
                battleManager.battleEndDelegate += StageEnd;
            }
            else
            {
                Debug.Log("Battle Manager 이미 존재");
            }
        }

        private void GenerateStageManager()
        {
            if (!stageManager)
            {
                stageManager = Instantiate(stageManagerPrefab);
                stageManager.gameObject.name = "Stage Manager";

                yHalfScreenSize = Camera.main.orthographicSize;
                xHalfScreenSize = yHalfScreenSize * Camera.main.aspect;

                // xHalf에다 step을 나눠서 간격 구하기, yHalf에다가 start를 나눠서 간격 구하기
                width = (xHalfScreenSize * 2) / stepCount;
                height = (yHalfScreenSize * 2) / startCount;

                stageManager.gameObject.transform.position = new Vector3(-xHalfScreenSize, -yHalfScreenSize, 0f);
                stageManager.GenerateStage(startCount, stepCount, height, width);
            }
            else
            {
                Debug.Log("Stage Manager 이미 존재");
                stageManager.ActivateStages();
            }
        }

        private void StageToBattleScene()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

                if (hit.collider != null)
                {
                    StageNode clickedStage = hit.collider.gameObject.GetComponent<StageNode>();
                    bool canClicked = clickedStage.CanClicked;

                    if (canClicked)
                    {
                        stageManager.InactivateStages();
                        battleManager.LinkeToStageNode(clickedStage);

                        // SceneManager.LoadScene("BattleScene");
                        SceneManager.LoadScene("ChooseHeroScene");

                        battleManager.GenerateBattle(clickedStage.StageInfo._step);
                    }
                }
            }
        }

        public void StageToStartScene()
        {
            Debug.Log("Button Clicked");
            stageManager.InactivateStages();
            SceneManager.LoadScene("StartScene");
        }

        private void StageEnd((bool _isCompleted, int _step) _stageInfo)
        {
            if (_stageInfo._step == stageManager.stageList.Count - 1 && _stageInfo._isCompleted == true)
            {
                SceneManager.LoadScene("EndScene");
            }
            else
            {
                BattleToStageScene();
            }
        }

        private void BattleToStageScene()
        {
            SceneManager.LoadScene("StageSelectScene");
            stageManager.ActivateStages();
        }

        public void GameStart()
        {
            SceneManager.LoadScene("StageSelectScene");
            GenerateStageManager();
            GenerateBattleManager();
        }

        public void EndToStart()
        {
            SceneManager.LoadScene("StartScene");
            stageManager = null;
        }
    }
}


