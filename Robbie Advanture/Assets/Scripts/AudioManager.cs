using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    static AudioManager current;

    [Header("环境音")]
    public AudioClip ambientClip;
    public AudioClip musicClip;

    [Header("人物音效")]
    public AudioClip[] walkStepClip;
    public AudioClip[] crouchStepClip;
    public AudioClip jumpClip;
    public AudioClip selectedClip;

    public AudioClip jumpVoiceClip;

    public AudioClip playerDeathClip;
    public AudioClip playerDeathVoice;
    
    [Header("FX音效")]
    public AudioClip returnOrb;
    public AudioClip doorFX;
    public AudioClip startLevelClip;
    public AudioClip winClip;

    public AudioClip selectOrbClip;

    //用代码创建AudioSource
    AudioSource ambientSource;
    AudioSource musicSource;
    AudioSource fxSource;
    AudioSource playerSource;
    AudioSource voiceSource;
    private void Awake()
    {
        if(current != null)
        {
            Destroy(gameObject);
            return;
        }
        current = this;
        
        DontDestroyOnLoad(gameObject);

        ambientSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        fxSource = gameObject.AddComponent<AudioSource>();
        playerSource = gameObject.AddComponent<AudioSource>();
        voiceSource = gameObject.AddComponent<AudioSource>();

        StarLevelMusic();

    }
    void StarLevelMusic()
    {
        current.ambientSource.clip = current.ambientClip;
        current.ambientSource.loop = true;
        current.ambientSource.Play();

        current.musicSource.clip = current.musicClip;
        current.musicSource.loop = true;
        current.musicSource.Play();
    }
    public static void PlayFootStepAudio()
    {
        int index = Random.Range(0, current.walkStepClip.Length);

        current.playerSource.clip = current.walkStepClip[index];
        current.playerSource.Play();
    }
    public static void DoorAudio()
    {
        current.fxSource.clip = current.doorFX;
        current.fxSource.PlayDelayed(1f);
    }
    public static void PlayCrouchAudio()
    {
        int index = Random.Range(0, current.crouchStepClip.Length);

        current.playerSource.clip = current.crouchStepClip[index];
        current.playerSource.Play();
    }
    public static void PlayerJumpAudio()
    {
        current.playerSource.clip = current.jumpClip;
        current.playerSource.Play();

        current.voiceSource.clip = current.jumpVoiceClip;
        current.voiceSource.Play();
    }
    public static void PlayerDieAudio()
    {
        current.playerSource.clip = current.playerDeathClip;
        current.playerSource.Play();

        current.voiceSource.clip = current.playerDeathVoice;
        current.voiceSource.Play();

        current.fxSource.clip = current.returnOrb;
        current.fxSource.Play();
    }
    public static void SelectedOrb()
    {
        current.fxSource.clip = current.selectOrbClip;
        current.fxSource.Play();

        current.voiceSource.clip = current.selectedClip;
        current.voiceSource.Play();
    }
    public static void StartLevel()
    {
        current.fxSource.clip = current.startLevelClip;
        current.fxSource.Play();
    }
    public static void PlayerWin()
    {
        current.fxSource.clip = current.winClip;
        current.fxSource.Play();
    }
}
