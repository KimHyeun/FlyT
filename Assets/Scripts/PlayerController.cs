using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public LineManager lineManager;
   
    private int currentLineIndex = 1; // 초기 라인 인덱스 (0: 위, 1: 중간, 2: 아래)
   

    int hp;
    public Slider slider;

 
    private void Start()
    {
        PlayerInitialized();
    }

    public void PlayerInitialized() // 재시작시 실행
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
           hp = 100;
        slider.maxValue = hp;
        slider.value = hp;

        // 1초마다 10 씩 hp 가 감소하는 코루틴 추가 및 실행
        StartCoroutine(DecreaseHP());
    }

    IEnumerator DecreaseHP()
    {
        while (hp > 0)
        {
            yield return new WaitForSeconds(1f); // 1초 대기

            HpMinus(10);           
        }

    }



    // 체력
    void HpMinus(int num)
    {
        SoundManager.Instance.HitSound();

        hp -= num; // HP 감소
        slider.value = hp; // 슬라이더 값 업데이트

        // spriteRenderer 빨간색으로 순간 바뀌었다 원래대로 돌아가는 기능 추가
        StartCoroutine(FlashRed());

        if (hp <= 0)
        {
            hp = 0;
            slider.value = hp;
            // HP가 0이 되면 추가적인 처리 (예: 플레이어 사망 처리 등)

            SoundManager.Instance.DeathSound();

            // Main 씬으로 이동
            GameManager.Instance.EndRunning();
        }
    }

    IEnumerator FlashRed()
    {
        // 빨간색으로 변경
        spriteRenderer.color = Color.red;

        // 0.1초 후 원래 색상으로 복원
        yield return new WaitForSeconds(0.1f);

        // 원래 색상으로 되돌리기
        spriteRenderer.color = Color.white;
    }


    void HpPlus(int num)
    {
        hp += num;
        slider.value = hp;

        if (hp > 100) hp = 100;
    }






    // 이동
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭
        {
            SoundManager.Instance.MoveSound();

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 클릭한 위치와 현재 위치 비교
            if (mousePosition.y > transform.position.y && currentLineIndex > 0)
            {
                // 위쪽 라인으로 이동
                MoveToLine(currentLineIndex - 1);
            }
            else if (mousePosition.y < transform.position.y && currentLineIndex < lineManager.LineYValues.Length - 1)
            {
                // 아래쪽 라인으로 이동
                MoveToLine(currentLineIndex + 1);
            }
        }
    }

    private void MoveToLine(int targetLineIndex)
    {
        currentLineIndex = targetLineIndex;
        Vector3 targetPosition = new Vector3(transform.position.x, lineManager.LineYValues[targetLineIndex], transform.position.z);

        StopCoroutine("SmoothMove"); // 기존 이동 중인 코루틴 정지
        StartCoroutine(SmoothMove(transform.position, targetPosition));
    }

    private IEnumerator SmoothMove(Vector3 start, Vector3 end)
    {
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(start, end, elapsedTime);
            elapsedTime += Time.deltaTime * GameManager.Instance.moveSpeed;
            yield return null;
        }
        transform.position = end; // 정확한 위치로 이동
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Food 레이어, Enemy 레이어 별도 처리

        // Food 의 경우 체력을 30 회복하고 Food 오브젝트 제거
        if (collision.gameObject.layer == LayerMask.NameToLayer("Food"))
        {
            SoundManager.Instance.GetSound();

            HpPlus(30);

            // Food 오브젝트 제거
            Destroy(collision.gameObject);
        }

        // Enemy의 경우 체력을 10 감소하고 Enemy 오브젝트 제거
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            

            HpMinus(10);

            // Enemy 오브젝트 제거
            Destroy(collision.gameObject);
        }
    }
}
