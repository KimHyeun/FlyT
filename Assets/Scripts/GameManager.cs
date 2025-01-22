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
        // 싱글톤 인스턴스 설정
        if (Instance == null)
        {
            Instance = this; // 현재 인스턴스를 설정
            DontDestroyOnLoad(Instance);
            DontDestroyOnLoad(mainObj);
            DontDestroyOnLoad(endObj);
            DontDestroyOnLoad(timeObj);
        }

        else
        {
            Destroy(gameObject); // 중복된 인스턴스 제거
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
    public float scrollSpeed; // 배경 속도
    public float moveSpeed; // 캐릭터 속도
    public float moveObjSpeed; // 스폰 오브젝트 속도

    public float maxSpeedMultiplier = 1.5f; // 최대 속도 배율
    public float timeToMaxSpeed = 120f; // 최대 속도에 도달하는 데 걸리는 시간

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
            // 경과 시간에 따른 속도 증가
            float elapsedTime = Time.timeSinceLevelLoad; // 게임 시작 이후 경과 시간

            // 속도 배율 계산 (0에서 maxSpeedMultiplier까지)
            float speedMultiplier = Mathf.Lerp(1f, maxSpeedMultiplier, elapsedTime / timeToMaxSpeed);

            // 속도 적용
            scrollSpeed = 0.5f * speedMultiplier;
            moveSpeed = 5f * speedMultiplier;
            moveObjSpeed = 5f * speedMultiplier;
        }
       
    }

    private IEnumerator UpdateUITimer()
    {
        while (timeCheck)
        {
            // 경과 시간을 계산
            gameTime += Time.deltaTime;

            // 시간을 분:초 형식으로 변환
            int minutes = Mathf.FloorToInt(gameTime / 60f);
            int seconds = Mathf.FloorToInt(gameTime % 60f);

            // UI 갱신
            time.text = $"{minutes:D2}:{seconds:D2}";


            // 지정된 간격 동안 대기
            yield return null;


        }
    }

    public void StartButtonClick()
    {
        SoundManager.Instance.ClickSound();

        canvasGroup = waitObj.GetComponent<CanvasGroup>();

        SoundManager.Instance.PlayBGMChange(0); // bgm 전환

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
        // 대기 오브젝트를 활성화하고 불투명하게 만듦
        waitObj.SetActive(true);
        canvasGroup.alpha = 1f;  // 불투명 상태
        
        // 씬 로딩 시작
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName);

        // 씬 로딩이 끝날 때까지 기다림
        while (!asyncLoad.isDone)
        {
            yield return null; // 로딩이 완료될 때까지 계속 기다림
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


        float fadeDuration = 0.5f; // 서서히 투명해지는 시간
        float startAlpha = canvasGroup.alpha;

        // 서서히 알파 값을 줄여서 투명하게 만듦
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, t / fadeDuration);
            yield return null; // 각 프레임마다 잠시 기다려줌
        }

        canvasGroup.alpha = 0f; // 정확히 0으로 설정        

        // 투명해지면 대기 오브젝트 비활성화
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
