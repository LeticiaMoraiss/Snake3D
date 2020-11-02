using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePanel : MonoBehaviour
{
    private Animator anim;       // Componente animador de cache

    private int slideOutHash;    // Parâmetros da caixa de animação
    private int slideInHash;     // Parâmetros da caixa de animação

    [SerializeField]
    private HUD hudScript;       // Referência ao script HUD para deslizar a cabeça    

    [SerializeField]
    private GameObject player;   // Ativa o player quando o botão play for pressionado    

    void Awake()
    {
        anim = GetComponent<Animator>();

        slideOutHash = Animator.StringToHash("SlideOut");
        slideInHash = Animator.StringToHash("SlideIn");
    }

    void Start()
    {
        GameManager.Instance.GameOverEvent.AddListener(OnGameOver);
    }

    /// <summary>
    /// Quando a imagem da snake for clicada, deslize para fora e inicie o jogo    
    /// </summary>
    public void OnPlay()
    {
        // .. Verifica esta jogando para evitar spam no botão        
        if (GameManager.Instance.gameState == GameState.Playing)
            return;

        // .. Altera o estado do jogo para jogar        
        GameManager.Instance.gameState = GameState.Playing;

        // .. Reproduz música do menu(é o mesmo menu e música no jogo)      
        SoundManager.Instance.PlayMenuMusic();

        // .. Deslizar para fora
        anim.SetTrigger(slideOutHash);

        // .. Deslize no HUD
        StartCoroutine(SlideInHUD(.5f));

        // .. Ativar jogador
        StartCoroutine(ActivatePlayer(.85f));
    }

    IEnumerator SlideInHUD(float delay, bool slideIn = true)
    {
        yield return new WaitForSeconds(delay);

        if (slideIn)
            hudScript.SlideIn();
        else
            hudScript.SlideOut();
    }

    IEnumerator ActivatePlayer(float delay)
    {
        yield return new WaitForSeconds(delay);

        GameManager.Instance.GameCommencingEvent.Invoke();   // O jogo começa aqui 

        player.SetActive(true);
    }

    /// <summary>
    /// Redefine jogo quando o jogador perde
    /// </summary>
    private void OnGameOver()
    {
        StartCoroutine(ResetGameAfter(1f));
    }

    IEnumerator ResetGameAfter(float dur)
    {
        yield return new WaitForSeconds(dur);

        // .. Chama o evento de redefinição
        GameManager.Instance.ResetEvent.Invoke();

        OnReturn();
    }

    /// <summary>
    /// Slide Na animação do menu principal e oculte o HUD
    /// </summary>
    private void OnReturn()
    {
        // .. Slide No menu principal UI
        anim.SetTrigger(slideInHash);

        // .. Deslize o HUD
        StartCoroutine(SlideInHUD(0f, false));

        // .. Desativa o player
        player.SetActive(false);        
    }
}
