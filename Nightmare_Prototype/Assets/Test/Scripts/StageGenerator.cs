using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        resultStageList = SetBossStage(resultStageList);

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
                // 애초에 이것도 캡슐화를 엄청 깬거
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
            // 처음과 끝 처리
            if (!(step[0].step == 0 || step[0].step == _stageList.Count - 1))
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

    public List<List<StageNode>> SetBossStage(List<List<StageNode>> _stageList)
    {
        int lastStep = _stageList.Count - 1;
        int targetIndex = Random.Range(0, startNumber);
        StageNode targetNode = _stageList[lastStep][targetIndex];

        foreach (StageNode stage in _stageList[lastStep])
        {
            if (stage.index != targetIndex)
            {
                MergeNode(stage, targetNode, _stageList);
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