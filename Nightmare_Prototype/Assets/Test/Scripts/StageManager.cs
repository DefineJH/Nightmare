using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class StageManager : MonoBehaviour
{
    public int startNumber;
    public int stepNumber;
    public float verticalLength;
    public float horiozontalLength;
    public GameObject stageNode;
    public GameObject pathObject;
    public List<List<StageNode>> stageList;

    void Awake()
    {
        StageGenerator stageGenerator = new StageGenerator(startNumber, stepNumber, verticalLength, horiozontalLength);

        stageList = stageGenerator.GenerateStageList();
    }

    void Start()
    {
        StageViewer stageViewer = this.gameObject.AddComponent<StageViewer>();
        stageViewer.Initialize(stageNode, pathObject);

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
    public Vector2 position;
    public List<int> preNode;
    public List<int> postNode;
    public List<int> pointer;

    public StageNode(int _step, int _index, int _type, bool _isClear, bool _canChoose, Vector2 _position)
    {
        this.step = _step;
        this.index = _index;
        this.type = _type;
        this.isClear = _isClear;
        this.canChoose = _canChoose;
        this.position = _position;
        this.preNode = new List<int>();
        this.postNode = new List<int>();
        this.pointer = new List<int>();
    }

    public void SetIndex(int _index)
    {
        this.index = _index;
    }

    // 테스트용 함수
    public string Debug()
    {
        string preList = "[";
        string postList = "[";
        string pointerList = "[";

        for (int index = 0; index < this.preNode.Count; index++)
        {
            preList += this.preNode[index];
            if (index != this.preNode.Count - 1)
            {
                preList += ",";
            }
        }

        for (int index = 0; index < this.postNode.Count; index++)
        {
            postList += this.postNode[index];
            if (index != this.postNode.Count - 1)
            {
                postList += ",";
            }
        }

        if (this.pointer.Count != 0)
        {
            pointerList += this.pointer[0];
        }

        preList += "]";
        postList += "]";
        pointerList += "]";

        string DebugLog = string.Format("step:{0} index:{1} type:{2}, clear:{3}, choose:{4}, pre:{5}, post:{6}, pointer:{7}, position:{8},{9}",
        this.step, this.index, this.type, this.isClear, this.canChoose, preList, postList, pointerList, this.position.x, this.position.y);

        return DebugLog;
    }
}