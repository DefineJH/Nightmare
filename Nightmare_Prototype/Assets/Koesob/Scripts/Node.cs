using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace koesob
{
    public class Node : MonoBehaviour
    {
        private int step;
        private List<int> directionList;

        public void Initiate(int _step, List<int> _directionList)
        {
            step = _step;
            directionList = _directionList;
        }
    }
}


