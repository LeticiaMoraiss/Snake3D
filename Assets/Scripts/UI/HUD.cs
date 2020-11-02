using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField]
    private Text currentScoreText;

    [SerializeField]
    private Text bestScoreText;

    private Animator anim;           // Componente animador de cache
    private int slideInHash;         // Parâmetros do animador de cache
    private int slideOutHash;

    void Start()
    {
        anim = GetComponent<Animator>();
        slideInHash = Animator.StringToHash("SlideIn");
        slideOutHash = Animator.StringToHash("SlideOut");

        // .. Instancia os eventos de atualização de pontuação
        ScoreManager.Instance.CurrentScoreUpdatedEvent.AddListener(OnCurrentScoreUpdated);
        ScoreManager.Instance.BestScoreUpdatedEvent.AddListener(OnBestScoreUpdated);

        // .. Define o texto top de pontuação no início
        bestScoreText.text = ScoreManager.Instance.BestScore.ToString();
    }

    /// <summary>
    /// Atualiza a interface do usuário da pontuação atual quando seu valor for alterado    
    /// </summary>
    private void OnCurrentScoreUpdated()
    {
        currentScoreText.text = ScoreManager.Instance.CurrentScore.ToString();
    }

    /// <summary>
    /// Atualiza a interface do usuário com melhor pontuação quando seu valor for alterado    
    /// </summary>
    private void OnBestScoreUpdated()
    {
        bestScoreText.text = ScoreManager.Instance.BestScore.ToString();
    }

    public void OnPause()
    {
        if (GameManager.Instance.gameState == GameState.Playing)
        {
            // .. Pause/Continue
            Time.timeScale = (Time.timeScale == 1f) ? 0f : 1f;
        }
    }

    /// <summary>
    /// Começa a deslizar para fora da animação    
    /// </summary>
    public void SlideOut()
    {
        anim.SetTrigger(slideOutHash);
    }

    public void SlideIn()
    {
        anim.SetTrigger(slideInHash);
    }
}
