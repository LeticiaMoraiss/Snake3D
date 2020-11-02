using UnityEngine.UI;
using UnityEngine;

public class PauseButton : MonoBehaviour {

    [SerializeField]
    private Sprite pauseIcon;
    [SerializeField]
    private Sprite playIcon;

    private Image imageComponent;       // Referência o componente de imagem    
    private bool pauseActive = true;    // Icone ativo atual    

    void Start()
    {
        imageComponent = GetComponent<Image>();
    }

    public void OnPause()
    {
        if (pauseActive)
        {
            // .. Carrega o ícone de reprodução (este icone é o que fica no topo do jogo)            
            imageComponent.overrideSprite = playIcon;

            pauseActive = false;
        }
        else
        {
            // .. Carregua o ícone de pausa (este icone é o que fica no topo do jogo)
            imageComponent.overrideSprite = pauseIcon;

            pauseActive = true;
        }
    }
}
