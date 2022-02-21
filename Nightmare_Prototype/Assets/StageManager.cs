using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public int startNumber;
    public int stepNumber;
    public List<List<StageNode>> stageList;

    void Awake()
    {
        StageGenerator stageGenerator = new StageGenerator(startNumber, stepNumber);
        
        stageList = stageGenerator.GenerateStageList();
    }

    void Start()
    {
        StageViewer stageViewer = new StageViewer();

        stageViewer.GenerateViewer(stageList);
    }
}

[SerializeField]
public struct StageNode
{
    // 이 노드가 몇 단계에 존재하는지
    public int step;
    // 이 노드가 이 단계에서 몇 번째 노드인지
    public int index;
    // 이 노드가 전투인지, 마을인지, 기타 등등
    public int type;
    // 노드의 클리어 여부
    public bool isClear;
    // 선택 가능 여부
    public bool canChoose;
    // 이 노드의 지도 상 위치
    public Vector3 position;
    public List<int> preNode;
    public List<int> postNode;

    public StageNode(int _step, int _index, int _type, bool _isClear, bool _canChoose, Vector3 _position)
    {
        this.step = _step;
        this.index = _index;
        this.type = _type;
        this.isClear = _isClear;
        this.canChoose = _canChoose;
        this.position = _position;
        this.preNode = new List<int>();
        this.postNode = new List<int>();
    }

    // 테스트용 함수
    public string Debug()
    {
        string preList = "[";
        string postList = "[";

        for(int index=0; index<this.preNode.Count; index++)
        {
            preList += this.preNode[index];
        }

        for(int index=0; index<this.postNode.Count; index++)
        {
            postList += this.postNode[index];
        }

        preList += "]";
        postList += "]";

        string DebugLog = string.Format("step:{0} index:{1} type:{2}, clear:{3}, choose:{4}, pre:{5}, post:{6}",
        this.step, this.index, this.type, this.isClear, this.canChoose, preList, postList);

        return DebugLog;
    }
}

public class StageGenerator
{
    private int startNumber;
    private int stepNumber;

    public StageGenerator(int _startNumber, int _stepNumber)
    {
        startNumber = _startNumber;
        stepNumber = _stepNumber;
    }

    public List<List<StageNode>> GenerateStageList()
    {
        List<List<StageNode>> resultStageList = new List<List<StageNode>>();

        resultStageList = Initialize();

        return resultStageList;
    }

    // _startNumber x _stepNumeber 만큼 Node 만들기
    public List<List<StageNode>> Initialize()
    {
        List<List<StageNode>> initialStageList = new List<List<StageNode>>();

        for(int step=0; step<stepNumber; step++)
        {
            List<StageNode> stepList = new List<StageNode>();
            
            for(int index=0; index<startNumber; index++)
            {
                StageNode stage = new StageNode(step, index, 0, false, true, Vector3.zero);
                stage.preNode.Add(index);
                stage.postNode.Add(index);

                stepList.Add(stage);
            }

            initialStageList.Add(stepList);
        }

        return initialStageList;
    }
}

public class StageViewer
{
    public void LogNodeObject(StageNode _stageNode)
    {
        Debug.Log(_stageNode.Debug());
    }

    public void GenerateViewer(List<List<StageNode>> _stageList)
    {
        if(_stageList.Count != 0)
        {
            int stepCount = _stageList.Count;
            for(int step=0; step<stepCount; step++)
            {
                int total = _stageList[step].Count;
                
                for(int index=0; index<total; index++)
                {
                    StageNode debugNode = _stageList[step][index];

                    // 이 코드만 바꾸면서 확인하면 된다.
                    Debug.Log(debugNode.Debug());
                }
            }
        }        
    }
}
