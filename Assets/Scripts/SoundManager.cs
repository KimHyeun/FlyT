using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        // �̱��� �ν��Ͻ� ����
        if (Instance == null)
        {
            Instance = this; // ���� �ν��Ͻ��� ����
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject); // �ߺ��� �ν��Ͻ� ����
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
                // mainBgm -> playBgm ���� �ڿ������� ��ȯ
                StartCoroutine(CrossfadeBGM(playBgm));
            }
            else if (index == 1)
            {
                // playBgm -> mainBgm ���� �ڿ������� ��ȯ
                StartCoroutine(CrossfadeBGM(mainBgm));
            }
        }
    }

    IEnumerator CrossfadeBGM(AudioClip newBGM)
    {
        // ���� BGM�� ������ ������ �ٿ����� ��ü
        float fadeDuration = 0.5f; // ��ȯ�� �ɸ��� �ð�
        float startVolume = audioSourceBGM.volume;

        // ������ 0���� �ٿ����� BGM ��ȯ
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSourceBGM.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        audioSourceBGM.volume = 0; // ������ ��Ȯ�� 0���� ����
        audioSourceBGM.clip = newBGM; // ���ο� BGM ����
        audioSourceBGM.Play(); // ���ο� BGM ���

        // ���ο� BGM�� ������ ������ ������Ŵ
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSourceBGM.volume = Mathf.Lerp(0, startVolume, t / fadeDuration);
            yield return null;
        }

        audioSourceBGM.volume = startVolume; // ���������� ������ ���� ������ ����
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
        float fadeDuration = 1f; // ������ ���̴� �ð�
        float startVolume = audioSourceBGM.volume;

        // ������ 0���� �ٿ����� ����
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSourceBGM.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        audioSourceBGM.volume = 0; // ������ ��Ȯ�� 0���� ����
        audioSourceBGM.Stop(); // BGM ����
    }


    public void PlayBGMRestart()
    {
        StartCoroutine(FadeInBGM());
    }

    IEnumerator FadeInBGM()
    {
        float fadeDuration = 1f; // ������ �����ϴ� �ð�
        float startVolume = 0f;

        audioSourceBGM.Play(); // BGM ����
        audioSourceBGM.volume = startVolume; // ������ 0���� ����

        // ������ ������ ������Ŵ
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSourceBGM.volume = Mathf.Lerp(startVolume, 1, t / fadeDuration);
            yield return null;
        }

        audioSourceBGM.volume = 1; // ���������� ������ 1�� ����
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
