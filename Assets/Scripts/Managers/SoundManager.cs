// Este scrip define as configurações de audio 
using UnityEngine;
using UnityEngine.UI;

// .. Armazene todos os nomes de efeitos sonoros para se referir facilmente a eles fora do escopo da classe
public enum SoundEffectName
{
    HIT,
    BUTTON_CLICK,
    COLLECT_COINS,
    COLLECT_FRUIT,
    EAT_FRUIT,
    NEW_HIGH_SCORE,
    SPAWN_BOMB
};

public enum PlayingMusicType
{
    NONE = -1,
    MENU = 0,
    IN_GAME = 0,
    POWER_UP_ACTIVE = 1,
};

[System.Serializable]
public struct SoundEffect
{
    public SoundEffectName effectName;
    public AudioClip effectAudio;
}

//Componentes necessários desta classe
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static int isMusicMuted;
    public static int isSoundEffectsMuted;
    public static PlayingMusicType musicType;

    public AudioClip[] MusicAudios;

    [Header("Defina os nomes sfx na enumeração SoundEffectName em SoundManager.cs.")]
    public SoundEffect[] soundEffectAudios;

    public static SoundManager Instance = null;

    public GameObject soundButton;
    public GameObject musicButton;

    private AudioSource audioSrc;      // A fonte de áudio anexada
    private float startingVol;         // Armazena o volume inicial da fonte de áudio

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)   
        {
            Destroy(gameObject);

            throw new UnityException("Gerenciador de jogos duplicado!!");
        }

        DontDestroyOnLoad(gameObject);     // Faz sobreviver a mudanças de cena

        audioSrc = GetComponent<AudioSource>();
        musicType = PlayingMusicType.NONE;

        // .. Obtem os estados de música e efeitos sonoros salvos anteriormente
        isMusicMuted = PlayerSettings.GetMusicState();
        isSoundEffectsMuted = PlayerSettings.GetSoundEffectsState();

        startingVol = audioSrc.volume;
    }

    void Start()
    {
        if (isMusicMuted == 1)
            musicButton.GetComponent<Toggle>().isOn = true;
        else
            musicButton.GetComponent<Toggle>().isOn = false;

        if (isSoundEffectsMuted == 1)
            soundButton.GetComponent<Toggle>().isOn = false;
        else
            soundButton.GetComponent<Toggle>().isOn = true;

        PlayMenuMusic();
    }

    /// <summary>
    /// Alterna o estado dos efeitos sonoros para On / Off
    /// </summary>
    public void ToggleMuteSoundEffects()
    {
        if (isSoundEffectsMuted == 0)
        {
            isSoundEffectsMuted = 1;
        }
        else
        {
            isSoundEffectsMuted = 0;
        }

        // .. Salva o estado dos efeitos sonoros        
        PlayerSettings.SetSoundEffectsState(isSoundEffectsMuted);
    }

    public void PlaySoundEffect(SoundEffectName effectName, float vol = -1)
    {
        if (isSoundEffectsMuted == 0)   // Se os efeitos sonoros não estiverem silenciados, inicie            
        {
            for (int i = 0; i < soundEffectAudios.Length; i++)
            {
                if (soundEffectAudios[i].effectName == effectName)
                {
                    // Define o volume da fonte de áudio, se especificado, se não definir o volume padrão
                     vol = (vol != -1f) ? vol : startingVol;

                    audioSrc.PlayOneShot(soundEffectAudios[i].effectAudio, vol);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Alterna o estado da música para On / Off    
    /// </summary>
    public void ToggleMuteMusic()
    {
        if (isMusicMuted == 0)
        {
            musicType = PlayingMusicType.NONE;
            isMusicMuted = 1;
            audioSrc.Stop();
        }
        else
        {
            isMusicMuted = 0;
            PlayMenuMusic();
        }

        // Salva o novo estado da música
        PlayerSettings.SetMusicState(isMusicMuted);
    }
    
    public void PlayMenuMusic()
    {
        if (musicType != PlayingMusicType.MENU)
        {
            if ((MusicAudios.Length > 0) && (isMusicMuted == 0))
            {
                audioSrc.clip = MusicAudios[(int)PlayingMusicType.MENU];
                audioSrc.loop = true;
                audioSrc.Play();
                musicType = PlayingMusicType.MENU;
            }
        }
    }

    public void PlayMusic(PlayingMusicType type)
    {
        if ((MusicAudios.Length > 0) && (isMusicMuted == 0))
        {
            audioSrc.clip = MusicAudios[(int)type];
            audioSrc.loop = true;
            audioSrc.Play();
            musicType = type;
        }
    }

    /// <summary>
    /// Silencia a fonte de áudio    
    /// </summary>
    public void StopMusic()
    {
        musicType = PlayingMusicType.NONE;
        audioSrc.Stop();
    }
}