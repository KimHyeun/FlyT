using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public LineManager lineManager;
   
    private int currentLineIndex = 1; // �ʱ� ���� �ε��� (0: ��, 1: �߰�, 2: �Ʒ�)
   

    int hp;
    public Slider slider;

 
    private void Start()
    {
        PlayerInitialized();
    }

    public void PlayerInitialized() // ����۽� ����
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
           hp = 100;
        slider.maxValue = hp;
        slider.value = hp;

        // 1�ʸ��� 10 �� hp �� �����ϴ� �ڷ�ƾ �߰� �� ����
        StartCoroutine(DecreaseHP());
    }

    IEnumerator DecreaseHP()
    {
        while (hp > 0)
        {
            yield return new WaitForSeconds(1f); // 1�� ���

            HpMinus(10);           
        }

    }



    // ü��
    void HpMinus(int num)
    {
        SoundManager.Instance.HitSound();

        hp -= num; // HP ����
        slider.value = hp; // �����̴� �� ������Ʈ

        // spriteRenderer ���������� ���� �ٲ���� ������� ���ư��� ��� �߰�
        StartCoroutine(FlashRed());

        if (hp <= 0)
        {
            hp = 0;
            slider.value = hp;
            // HP�� 0�� �Ǹ� �߰����� ó�� (��: �÷��̾� ��� ó�� ��)

            SoundManager.Instance.DeathSound();

            // Main ������ �̵�
            GameManager.Instance.EndRunning();
        }
    }

    IEnumerator FlashRed()
    {
        // ���������� ����
        spriteRenderer.color = Color.red;

        // 0.1�� �� ���� �������� ����
        yield return new WaitForSeconds(0.1f);

        // ���� �������� �ǵ�����
        spriteRenderer.color = Color.white;
    }


    void HpPlus(int num)
    {
        hp += num;
        slider.value = hp;

        if (hp > 100) hp = 100;
    }






    // �̵�
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ���콺 ���� ��ư Ŭ��
        {
            SoundManager.Instance.MoveSound();

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Ŭ���� ��ġ�� ���� ��ġ ��
            if (mousePosition.y > transform.position.y && currentLineIndex > 0)
            {
                // ���� �������� �̵�
                MoveToLine(currentLineIndex - 1);
            }
            else if (mousePosition.y < transform.position.y && currentLineIndex < lineManager.LineYValues.Length - 1)
            {
                // �Ʒ��� �������� �̵�
                MoveToLine(currentLineIndex + 1);
            }
        }
    }

    private void MoveToLine(int targetLineIndex)
    {
        currentLineIndex = targetLineIndex;
        Vector3 targetPosition = new Vector3(transform.position.x, lineManager.LineYValues[targetLineIndex], transform.position.z);

        StopCoroutine("SmoothMove"); // ���� �̵� ���� �ڷ�ƾ ����
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
        transform.position = end; // ��Ȯ�� ��ġ�� �̵�
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Food ���̾�, Enemy ���̾� ���� ó��

        // Food �� ��� ü���� 30 ȸ���ϰ� Food ������Ʈ ����
        if (collision.gameObject.layer == LayerMask.NameToLayer("Food"))
        {
            SoundManager.Instance.GetSound();

            HpPlus(30);

            // Food ������Ʈ ����
            Destroy(collision.gameObject);
        }

        // Enemy�� ��� ü���� 10 �����ϰ� Enemy ������Ʈ ����
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            

            HpMinus(10);

            // Enemy ������Ʈ ����
            Destroy(collision.gameObject);
        }
    }
}
