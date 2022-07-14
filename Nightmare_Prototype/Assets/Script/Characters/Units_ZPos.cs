using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

// 에디터에서 적용되는 스크립트
// y축에 따라 z축 이동 -> 앞에서 보이게

public class Units_ZPos : MonoBehaviour
{

    void Update()
    {

        transform.localPosition = new Vector3(transform.position.x, transform.position.y, transform.position.y * 0.1f);
    }
}
