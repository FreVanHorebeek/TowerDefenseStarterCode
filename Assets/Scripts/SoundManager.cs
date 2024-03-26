using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource menuMusic;
    public AudioSource gameMusic;
    public GameObject audioSourcePrefab;
    public AudioClip[] uiSounds;
    public AudioClip[] towerSounds;
    public AudioClip[] fxSounds;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartMenuMusic()
    {
        menuMusic.Play();
        gameMusic.Stop();
    }

    public void StartGameMusic()
    {
        gameMusic.Play();
        menuMusic.Stop();
    }

    public void PlayUISound()
    {
        int index = Random.Range(0, uiSounds.Length);

        GameObject soundGameObject = Instantiate(audioSourcePrefab, transform.position, Quaternion.identity);
        AudioSource audioSource = soundGameObject.GetComponent<AudioSource>();

        audioSource.clip = uiSounds[index];
        audioSource.Play();

        Destroy(soundGameObject, uiSounds[index].length);
    }

    public void PlayTowerSound(TowerType towerType)
    {
        int index = (int)towerType - 1;
        if (index >= 0 && index < towerSounds.Length)
        {
            GameObject soundGameObject = Instantiate(audioSourcePrefab, transform.position, Quaternion.identity);
            AudioSource audioSource = soundGameObject.GetComponent<AudioSource>();

            audioSource.clip = towerSounds[index];
            audioSource.Play();

            Destroy(soundGameObject, towerSounds[index].length);
        }
    }

    public void PlayFXSound(FXEvent fxEvent)
    {
        AudioClip clip = GetFXSound(fxEvent);
        if (clip != null)
        {
            GameObject soundGameObject = Instantiate(audioSourcePrefab, transform.position, Quaternion.identity);
            AudioSource audioSource = soundGameObject.GetComponent<AudioSource>();

            audioSource.clip = clip;
            audioSource.Play();

            Destroy(soundGameObject, clip.length);
        }
    }

    private AudioClip GetFXSound(FXEvent fxEvent)
    {
        switch (fxEvent)
        {
            case FXEvent.GameStart:
                return fxSounds[0];
            case FXEvent.WaveStart:
                return fxSounds[1];
            case FXEvent.EnemyReachedEnd:
                return fxSounds[2];
            case FXEvent.GameWin:
                return fxSounds[3];
            case FXEvent.GameLose:
                return fxSounds[4];
            case FXEvent.TowerBuilt:
                return fxSounds[5];
            default:
                return null;
        }
    }
}

public enum TowerType
{
    Archer = 1,
    Sword = 2,
    Wizard = 3
}

public enum FXEvent
{
    GameStart,
    WaveStart,
    EnemyReachedEnd,
    GameWin,
    GameLose,
    TowerBuilt
}

