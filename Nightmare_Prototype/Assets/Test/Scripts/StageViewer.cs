using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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