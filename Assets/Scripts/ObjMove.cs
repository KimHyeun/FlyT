using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjMove : MonoBehaviour
{
    // 왼쪽으로 자연스럽게 트랜스폼 이동
    
    private void Update()
    {
        // 왼쪽으로 계속 이동
        transform.Translate(Vector3.left * GameManager.Instance.moveObjSpeed * Time.deltaTime);
    }
}
