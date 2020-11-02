using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour {

    [HideInInspector]
    public UnityEvent CurrentScoreUpdatedEvent; // Para ser disparado quando a pontuação atual for atualizada

    [HideInInspector]
    public UnityEvent BestScoreUpdatedEvent;    // Para ser disparado quando a pontuação atual for atualizada

    public static ScoreManager Instance;        

    [SerializeField]
    private GameObject[] scorePrefabs;          //  Cria um texto de pontuação com base na pontuação adicionada 

    private bool displayNewHighScore = false;   // Exibe novo texto de pontuação alta apenas uma vez

    public int CurrentScore
    {
        get
        {
            return currentScore;
        }
        set
        {
            currentScore = value;

            CurrentScoreUpdatedEvent.Invoke();      // Pontuação atual evento atualizado aqui
            
        }
    }

    public int BestScore
    {
        set
        {
            bestScore = value;

            BestScoreUpdatedEvent.Invoke();         // Pontuação top evento atualizado aqui

            PlayerSettings.SetBestScore(bestScore); // Salva a melhor pontuação no playerprefs
        }
        get
        {
            return bestScore;
        }
    }

    private int currentScore;                // Pontuação atual do usuário
    private int bestScore;                   // Busca de PlayerPrefs, se existir

    void Awake()
    {
        if (Instance == null)
            Instance = this;

        else if (Instance != this)
            Destroy(gameObject);
                                            
        DontDestroyOnLoad(gameObject);           

        CurrentScoreUpdatedEvent = new UnityEvent();
        BestScoreUpdatedEvent = new UnityEvent();

        InitScoreManager();
    }

    void InitScoreManager()
    {
        // .. Inicializa valores do gerenciador de pontuação
        currentScore = 0;

        // .. Obtem a última melhor pontuação salva
        bestScore = PlayerSettings.GetBestScore();

        // .. Instancia o evento gameOver para atualizar a melhor pontuação
        GameManager.Instance.GameOverEvent.AddListener(OnGameOver);

        // .. Usado para atualizar a pontuação quando a snake come a fruta
        GameManager.Instance.FruitAteEvent.AddListener(OnFruitAte);

        // .. Usado para redefinir a pontuação atual ao retornar ao menu principal        
        GameManager.Instance.ResetEvent.AddListener(OnReset);
    }

    /// <summary>
    /// invocado quando a snake come uma fruta
    /// </summary>
    private void OnFruitAte(int scoreAddition, Vector3 position)
    {
        // .. Aumenta a pontuação do jogador
        CurrentScore += scoreAddition;

        // .. Exibe um novo texto de pontuação top se obtivermos uma nova pontuação top        
        if (CurrentScore > BestScore && !displayNewHighScore && BestScore != 0)
        {
            displayNewHighScore = true;

            Instantiate(ScoringTextResolution(0), new Vector3(position.x, position.y, 5f), Quaternion.identity);

            // .. Inicia sfx
            SoundManager.Instance.PlaySoundEffect(SoundEffectName.NEW_HIGH_SCORE, 1f);
        }
        // .. Cria um texto de pontuação flutuante com base no valor da adição de pontuação        
        else
        {
            Instantiate(ScoringTextResolution(scoreAddition), new Vector3(position.x, position.y, 5f), Quaternion.identity);
        }
    }

    /// <summary>
    /// Invocado pelo GameOverEvent, usado para atualizar a melhor pontuação    
    /// </summary>
    private void OnGameOver()
    {
        // .. Nova pontuação Top        
        if (currentScore > bestScore)
        {
            // .. Oculta a pontuação atual e exibir apenas a melhor pontuação            
            BestScore = CurrentScore;
        }
    }

    private void OnReset()
    {
        CurrentScore = 0;
    }

    /// <summary>
    /// Obtenha o prefab correspondente com base na pontuação    
    /// </summary>
    /// <param name="score">Pontuação adicionada</param>
    /// <returns>O prefab de texto de pontuação correspondente</returns>
    private GameObject ScoringTextResolution(int score)
    {
        GameObject scorePrefab = null;

        switch(score)
        {
            case 5:
                scorePrefab = scorePrefabs[0];
                break;
            case 10:
                scorePrefab = scorePrefabs[1];
                break;
            case 20:
                scorePrefab = scorePrefabs[2];
                break;
            case 50:
                scorePrefab = scorePrefabs[3];
                break;
            case 0:
                scorePrefab = scorePrefabs[4];
                break;
        }

        return scorePrefab;
    }
}
