using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Json
{
    [System.Serializable]
    public class Node
    {
        public Vector3 position;
        public int level;
        public bool isClear;
        public List<int> nextNodes = new List<int>();

        public Node(bool isSet)
        {
            if (isSet)
            {
                position = Vector3.one;
                level = Random.Range(0, 5);
                isClear = false;

                for (int index = 0; index < 3; index++)
                {
                    int nextNodeIndex = Random.Range(0, 5);
                    nextNodes.Add(nextNodeIndex);
                }
            }
            else
            {
                position = Vector3.one;
                level = Random.Range(0, 5);
                isClear = false;
            }
        }
    }

    [System.Serializable]
    public class StageData
    {
        public List<Node> stageNodes = new List<Node>();
        public StageData(bool isSet)
        {
            if (isSet)
            {
                for(int index=0; index<5; index++)
                {
                    Node stageNode = new Node(true);
                    stageNodes.Add(stageNode);
                }
            }
        }
    }

    public class StageManager : MonoBehaviour
    {
        private void Start()
        {
            StageData stageData = new StageData(true);
            string jsonData = ObjectToJson(stageData);
            string saveDirectory = Application.dataPath + "/Test/JsonTest";
            CreateJsonFile(saveDirectory, "SaveData", jsonData);
        }

        string ObjectToJson(object _obj)
        {
            return JsonUtility.ToJson(_obj, true);
        }

        T JsonToObject<T>(string _jsonData)
        {
            return JsonUtility.FromJson<T>(_jsonData);
        }

        void CreateJsonFile(string _creatPath, string _fileName, string _jsonData)
        {
            FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", _creatPath, _fileName), FileMode.Create);
            byte[] data = Encoding.UTF8.GetBytes(_jsonData);
            fileStream.Write(data, 0, data.Length);
            fileStream.Close();
        }

        T LoadJsonFile<T>(string _loadPath, string _fileName)
        {
            FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", _loadPath, _fileName), FileMode.Open);
            byte[] data = new byte[fileStream.Length];
            fileStream.Read(data, 0, data.Length);
            fileStream.Close();
            string jsonData = Encoding.UTF8.GetString(data);
            return JsonUtility.FromJson<T>(jsonData);
        }

    }
}
