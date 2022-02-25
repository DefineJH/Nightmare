using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
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

public class StageGenerator
{
    private int startNumber;
    private int stepNumber;
    private float verticalLength;
    private float horiozontalLength;
    List<List<StageNode>> resultStageList;

    public StageGenerator(int _startNumber, int _stepNumber, float _verticalLength, float _horizontalLength)
    {
        this.startNumber = _startNumber;
        this.stepNumber = _stepNumber;
        this.verticalLength = _verticalLength;
        this.horiozontalLength = _horizontalLength;
        this.resultStageList = new List<List<StageNode>>();
    }

    public List<List<StageNode>> GenerateStageList()
    {
        resultStageList = InitializeList(resultStageList);
        resultStageList = RandomizeList(resultStageList);
        resultStageList = SetRandomPosition(resultStageList);

        return resultStageList;
    }

    // _startNumber x _stepNumeber 만큼 Node 만들기
    public List<List<StageNode>> InitializeList(List<List<StageNode>> _stageList)
    {
        _stageList.Clear();

        for (int step = 0; step < stepNumber; step++)
        {
            List<StageNode> stepList = new List<StageNode>();

            for (int index = 0; index < startNumber; index++)
            {
                StageNode stage = new StageNode(step, index, 0, false, true, Vector3.zero);
                stage.preNode.Add(index);
                stage.postNode.Add(index);

                stepList.Add(stage);
            }

            _stageList.Add(stepList);
        }

        return _stageList;
    }

    public List<List<StageNode>> RandomizeList(List<List<StageNode>> _stageList)
    {
        foreach (List<StageNode> step in _stageList)
        {
            int count = Random.Range(0, step.Count - 1);
            // 마지막 Index는 나오면 안되기 때문에 step.Count에서 -1
            List<int> randomIndex = GetRandomIndex(step.Count - 1, count);

            // randomIndex에서 순서대로 index를 뽑아서 해당 index에 node를 하나 더 큰 index에 노드로 바꿔주는 처리
            foreach (int index in randomIndex)
            {
                int toIndex = GetResultPointer(step[index + 1], _stageList);
                MergeNode(step[index], step[toIndex], _stageList);
            }
        }
        return _stageList;
    }

    public List<List<StageNode>> SetRandomPosition(List<List<StageNode>> _stageList)
    {
        for (int step = 0; step < stepNumber; step++)
        {
            for (int index = 0; index < startNumber; index++)
            {
                float minHorizontal = step * horiozontalLength;
                float maxHorizontal = minHorizontal + horiozontalLength;
                float minVertical = index * verticalLength;
                float maxVertical = minVertical + verticalLength;
                StageNode node = _stageList[step][index];

                node.position = new Vector2(Random.Range(minHorizontal, maxHorizontal), Random.Range(minVertical, maxVertical));
                _stageList[step][index] = node;
            }
        }

        return _stageList;
    }

    private List<int> GetRandomIndex(int _listSize, int _count)
    {
        List<int> randomIndex = new List<int>();

        for (int i = 0; i < _count; i++)
        {
            int index = Random.Range(0, _listSize);
            while (randomIndex.Contains(index))
            {
                index = Random.Range(0, _listSize);
            }
            randomIndex.Add(index);
        }

        return randomIndex;
    }

    public void MergeNode(StageNode _from, StageNode _to, List<List<StageNode>> _stageList)
    {
        int fromIndex = _from.index;
        int fromStep = _from.step;
        int preStep = fromStep - 1;
        int postStep = fromStep + 1;

        List<int> fromPreList = _from.preNode;
        List<int> fromPostList = _from.postNode;

        int toIndex = _to.index;

        if (preStep >= 0)
        {
            foreach (int preIndex in fromPreList)
            {
                // preNode
                _stageList[preStep][preIndex].postNode.Remove(fromIndex);
                if (!_stageList[preStep][preIndex].postNode.Contains(toIndex))
                {
                    _stageList[preStep][preIndex].postNode.Add(toIndex);
                }

                // toNode로 가서, 중복되지 않는다면, preIndex를 preNode에 추가
                if (!_to.preNode.Contains(preIndex))
                {
                    _to.preNode.Add(preIndex);
                }
            }
        }

        if (postStep < _stageList.Count)
        {
            foreach (int postIndex in fromPostList)
            {
                // postNode
                _stageList[postStep][postIndex].preNode.Remove(fromIndex);
                if (!_stageList[postStep][postIndex].preNode.Contains(toIndex))
                {
                    _stageList[postStep][postIndex].preNode.Add(toIndex);
                }

                if (!_to.postNode.Contains(postIndex))
                {
                    _to.postNode.Add(postIndex);
                }
            }
        }

        _from.pointer.Clear();
        _from.pointer.Add(toIndex);
    }

    public int GetResultPointer(StageNode _node, List<List<StageNode>> _stageList)
    {
        int resultIndex = _node.index;
        int step = _node.step;

        if (_node.pointer.Count != 0)
        {
            int pointer = _node.pointer[0];
            resultIndex = GetResultPointer(_stageList[step][pointer], _stageList);
        }

        return resultIndex;
    }
}

public class StageViewer : MonoBehaviour
{
    private GameObject stageNodeObject;
    private GameObject pathObject;

    public void Initialize(GameObject _stageNodeObject, GameObject _pathObject)
    {
        this.stageNodeObject = _stageNodeObject;
        this.pathObject = _pathObject;
    }

    public void LogNodeObject(StageNode _stageNode)
    {
        Debug.Log(_stageNode.Debug());
    }

    public void GenerateViewer(List<List<StageNode>> _stageList)
    {
        if (_stageList.Count != 0)
        {
            int stepCount = _stageList.Count;
            for (int step = 0; step < stepCount; step++)
            {
                int total = _stageList[step].Count;

                for (int index = 0; index < total; index++)
                {
                    StageNode stageNode = _stageList[step][index];

                    // 이 코드만 바꾸면서 확인하면 된다.
                    Debug.Log(stageNode.Debug());
                    
                    if (stageNode.pointer.Count == 0)
                    {
                        GameObject NodeObject = Instantiate(stageNodeObject, stageNode.position, Quaternion.identity);
                        GeneratePath(stageNode, _stageList);
                    }
                }
            }
        }
    }

    public void GeneratePath(StageNode _stageNode, List<List<StageNode>> _stageList)
    {
        int step = _stageNode.step;
        Vector2 fromPosition = _stageNode.position;

        foreach (int index in _stageNode.postNode)
        {
            if (step + 1 < _stageList.Count)
            {
                Vector2 toPosition = _stageList[step + 1][index].position;
                Vector2 middle = (fromPosition + toPosition) / 2;

                float adjacent = toPosition.x - fromPosition.x;
                float opposite = toPosition.y - fromPosition.y;
                float hypotenuse = (toPosition - fromPosition).magnitude;

                float angle = Mathf.Atan2(opposite, adjacent) * 180 / Mathf.PI;

                GameObject path = Instantiate(pathObject, middle, Quaternion.Euler(0, 0, angle));
                path.transform.localScale = new Vector3(hypotenuse * 8, 10, 1);
            }
        }
    }
}
