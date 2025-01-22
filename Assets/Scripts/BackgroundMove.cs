using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    private Material material; // 배경의 Material 참조
    private Vector2 offset;    // 텍스처 오프셋
   // float scrollSpeed; // 스크롤 속도 (조절 가능)

    private void Start()
    {
        // SpriteRenderer에서 Material을 가져옴
        material = GetComponent<SpriteRenderer>().material;

        // 초기 오프셋 설정
        offset = material.mainTextureOffset;

        
    }

    private void Update()
    {
        // 시간에 따라 x 방향으로 오프셋 이동
        offset.x += GameManager.Instance.scrollSpeed * Time.deltaTime;

        // Material의 Offset 업데이트
        material.mainTextureOffset = offset;
    }
}
