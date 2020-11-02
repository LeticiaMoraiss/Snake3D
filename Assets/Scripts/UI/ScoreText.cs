using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreText : MonoBehaviour {

    public float aliveDuration = .6f;           // Este texto de pontuação permanecerá ativo por esse período

    private SpriteRenderer textSpriteRenderer;

    // Usado para inicialização    
    void Awake()
    {
        iTween.Init(gameObject);

        textSpriteRenderer = GetComponent<SpriteRenderer>();
	}

    void Start()
    {
        ScoreTextAnimation();
    }

    /// <summary>
    /// Anima texto de pontuação para aparecer se estivesse flutuando    
    /// </summary>
    private void ScoreTextAnimation()
    {
        // .. Posição de interpolação
        iTween.MoveAdd(gameObject, iTween.Hash(
                "y", .4f,
                "time", aliveDuration,
                "delay", 0f,
                "easeType", iTween.EaseType.easeOutQuad));

        // .. Interpolação de cores        
        iTween.ValueTo(gameObject, iTween.Hash(
                "from", textSpriteRenderer.color,
                "to", Color.clear,
                "time", aliveDuration * .5f,
                "delay", aliveDuration * .5f,
                "onupdate", "OnColorUpdated",
                "easeType", iTween.EaseType.linear));

        // .. Escala de interpolação
        iTween.ScaleTo(gameObject, iTween.Hash(
                "scale", new Vector3(1f, 1f, 1f),
                "time", aliveDuration,
                "delay", 0f,
                "easeType", iTween.EaseType.easeOutQuad));        
    }

    private void OnColorUpdated(Color color)
    {
        textSpriteRenderer.color = color;
    }
}
