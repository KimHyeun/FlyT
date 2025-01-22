using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // �̱��� �ν��Ͻ� ����
        if (Instance == null)
        {
            Instance = this; // ���� �ν��Ͻ��� ����
            DontDestroyOnLoad(Instance);
            DontDestroyOnLoad(mainObj);
            DontDestroyOnLoad(endObj);
            DontDestroyOnLoad(timeObj);
        }

        else
        {
            Destroy(gameObject); // �ߺ��� �ν��Ͻ� ����
        }

        
    }


    public GameObject mainObj;
    public GameObject waitObj;
    public GameObject endObj;
    public GameObject timeObj;
    bool timeCheck;
    float gameTime;
    public TMP_Text time;
    CanvasGroup canvasGroup;

    bool speedCheck;
    public float scrollSpeed; // ��� �ӵ�
    public float moveSpeed; // ĳ���� �ӵ�
    public float moveObjSpeed; // ���� ������Ʈ �ӵ�

    public float maxSpeedMultiplier = 1.5f; // �ִ� �ӵ� ����
    public float timeToMaxSpeed = 120f; // �ִ� �ӵ��� �����ϴ� �� �ɸ��� �ð�

    private void Start()
    {
        SpeedSet();
    }

    void SpeedSet()
    {
        scrollSpeed = 0.5f;
        moveSpeed = 5f;
        moveObjSpeed = 5f;
    }

    private void Update()
    {
        if (speedCheck)
        {
            // ��� �ð��� ���� �ӵ� ����
            float elapsedTime = Time.timeSinceLevelLoad; // ���� ���� ���� ��� �ð�

            // �ӵ� ���� ��� (0���� maxSpeedMultiplier����)
            float speedMultiplier = Mathf.Lerp(1f, maxSpeedMultiplier, elapsedTime / timeToMaxSpeed);

            // �ӵ� ����
            scrollSpeed = 0.5f * speedMultiplier;
            moveSpeed = 5f * speedMultiplier;
            moveObjSpeed = 5f * speedMultiplier;
        }
       
    }

    private IEnumerator UpdateUITimer()
    {
        while (timeCheck)
        {
            // ��� �ð��� ���
            gameTime += Time.deltaTime;

            // �ð��� ��:�� �������� ��ȯ
            int minutes = Mathf.FloorToInt(gameTime / 60f);
            int seconds = Mathf.FloorToInt(gameTime % 60f);

            // UI ����
            time.text = $"{minutes:D2}:{seconds:D2}";


            // ������ ���� ���� ���
            yield return null;


        }
    }

    public void StartButtonClick()
    {
        SoundManager.Instance.ClickSound();

        canvasGroup = waitObj.GetComponent<CanvasGroup>();

        SoundManager.Instance.PlayBGMChange(0); // bgm ��ȯ

        DontDestroyOnLoad(waitObj);

        mainObj.SetActive(false);
        timeObj.SetActive(true);
        timeCheck = true;
        speedCheck = true;

        StartCoroutine(UpdateUITimer());

        StartCoroutine(SceneRunning("Car"));  
    }


    IEnumerator SceneRunning(string SceneName)
    {
        // ��� ������Ʈ�� Ȱ��ȭ�ϰ� �������ϰ� ����
        waitObj.SetActive(true);
        canvasGroup.alpha = 1f;  // ������ ����
        
        // �� �ε� ����
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName);

        // �� �ε��� ���� ������ ��ٸ�
        while (!asyncLoad.isDone)
        {
            yield return null; // �ε��� �Ϸ�� ������ ��� ��ٸ�
        }

        StartCoroutine(FadeOut(SceneName));
    }

    IEnumerator FadeOut(string sceneName)
    {
        if (sceneName == "Main")
        {
            mainObj.SetActive(true);
            endObj.SetActive(true);
            timeObj.SetActive(false);
            gameTime = 0;
            timeCheck = false;
            speedCheck = false;
            SpeedSet();
        }


        float fadeDuration = 0.5f; // ������ ���������� �ð�
        float startAlpha = canvasGroup.alpha;

        // ������ ���� ���� �ٿ��� �����ϰ� ����
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, t / fadeDuration);
            yield return null; // �� �����Ӹ��� ��� ��ٷ���
        }

        canvasGroup.alpha = 0f; // ��Ȯ�� 0���� ����        

        // ���������� ��� ������Ʈ ��Ȱ��ȭ
        waitObj.SetActive(false);

       
     
    }



    public void EndRunning()
    {
        SoundManager.Instance.PlayBGMChange(1);

        StartCoroutine(SceneRunning("Main"));
    }


    public void ReButtunClick()
    {
        SoundManager.Instance.ClickSound();
           endObj.SetActive(false);
    }
}
