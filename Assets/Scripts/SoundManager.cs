using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (Instance == null)
        {
            Instance = this; // 현재 인스턴스를 설정
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject); // 중복된 인스턴스 제거
        }

    }





    public AudioSource audioSourceBGM;
    public AudioSource audioSourceFX;
    public AudioClip mainBgm;
    public AudioClip playBgm;

    public AudioClip move;
    public AudioClip click;
    public AudioClip getFood;
    public AudioClip hit;
    public AudioClip death;


    private void Start()
    {
        PlayBGM();
    }

    public void PlayBGM()
    {
        audioSourceBGM.clip = mainBgm;

        audioSourceBGM.Play();
    }


    public void PlayBGMChange(int index)
    {
        if (audioSourceBGM.isPlaying)
        {
            if (index == 0)
            {
                // mainBgm -> playBgm 으로 자연스럽게 전환
                StartCoroutine(CrossfadeBGM(playBgm));
            }
            else if (index == 1)
            {
                // playBgm -> mainBgm 으로 자연스럽게 전환
                StartCoroutine(CrossfadeBGM(mainBgm));
            }
        }
    }

    IEnumerator CrossfadeBGM(AudioClip newBGM)
    {
        // 현재 BGM의 볼륨을 서서히 줄여가며 교체
        float fadeDuration = 0.5f; // 전환에 걸리는 시간
        float startVolume = audioSourceBGM.volume;

        // 볼륨을 0으로 줄여가며 BGM 전환
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSourceBGM.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        audioSourceBGM.volume = 0; // 볼륨을 정확히 0으로 설정
        audioSourceBGM.clip = newBGM; // 새로운 BGM 설정
        audioSourceBGM.Play(); // 새로운 BGM 재생

        // 새로운 BGM의 볼륨을 서서히 증가시킴
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSourceBGM.volume = Mathf.Lerp(0, startVolume, t / fadeDuration);
            yield return null;
        }

        audioSourceBGM.volume = startVolume; // 최종적으로 볼륨을 원래 값으로 설정
    }

    public void PlayBGMStop()
    {
        if (audioSourceBGM.isPlaying)
        {
            StartCoroutine(FadeOutBGM());
        }
    }

    IEnumerator FadeOutBGM()
    {
        float fadeDuration = 1f; // 서서히 줄이는 시간
        float startVolume = audioSourceBGM.volume;

        // 볼륨을 0으로 줄여가며 멈춤
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSourceBGM.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        audioSourceBGM.volume = 0; // 볼륨을 정확히 0으로 설정
        audioSourceBGM.Stop(); // BGM 멈춤
    }


    public void PlayBGMRestart()
    {
        StartCoroutine(FadeInBGM());
    }

    IEnumerator FadeInBGM()
    {
        float fadeDuration = 1f; // 서서히 증가하는 시간
        float startVolume = 0f;

        audioSourceBGM.Play(); // BGM 시작
        audioSourceBGM.volume = startVolume; // 볼륨을 0으로 설정

        // 볼륨을 서서히 증가시킴
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSourceBGM.volume = Mathf.Lerp(startVolume, 1, t / fadeDuration);
            yield return null;
        }

        audioSourceBGM.volume = 1; // 최종적으로 볼륨을 1로 설정
    }



    public void MoveSound()
    {
        audioSourceFX.PlayOneShot(move);
    }

    public void ClickSound()
    {
        audioSourceFX.PlayOneShot(click);
    }

    public void GetSound()
    {
        audioSourceFX.PlayOneShot(getFood);
    }


    public void HitSound()
    {
        audioSourceFX.PlayOneShot(hit);
    }



    public void DeathSound()
    {
        audioSourceFX.PlayOneShot(death);
    }

}
