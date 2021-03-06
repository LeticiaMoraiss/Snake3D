﻿// Esta classe é responsável por todos os dados referentes às configurações do player, por exemplo salvar / carregar as melhores músicas, sons, etc.
using UnityEngine;

public class PlayerSettings
{
    // .. PlayerPrefs keys
    private static string bestScoreKey = "bestScore";
    private static string musicMutedKey = "isMusicMuted";
    private static string soundsMutedKey = "isSoundEffectsMuted";

    public static int GetBestScore()
    {
        return PlayerPrefs.GetInt(bestScoreKey, 0);
    }

    public static int GetMusicState()
    {
        return PlayerPrefs.GetInt(musicMutedKey, 0);
    }

    public static int GetSoundEffectsState()
    {
        return PlayerPrefs.GetInt(soundsMutedKey, 0);
    }

    public static void SetBestScore(int score)
    {
        PlayerPrefs.SetInt(bestScoreKey, score);
    }

    public static void SetSoundEffectsState(int state)
    {
        PlayerPrefs.SetInt(soundsMutedKey, state);
    }

    public static void SetMusicState(int state)
    {
        PlayerPrefs.SetInt(musicMutedKey, state);
    }
}
