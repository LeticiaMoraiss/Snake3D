using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : PooledObject {

    [HideInInspector]
    public int scoreAddition = 10;

    void Awake()
    {
        iTween.Init(gameObject);     // Inicializa o mecanismo de interpolação para esse objeto para evitar lag        

        StartingAnimation();         // Reproduzir a animação inicial
    }

    public override void OnPooledObjectActivated()
    {
        base.OnPooledObjectActivated();

        // .. Quando obtiver um objeto de fruta, reproduza a animação uma vez
        StartingAnimation();
    }

    void OnTriggerEnter(Collider other)
    {
        // .. Colide com a snake (a camada de colisão da fruta só colide com a cabeça do jogador)        
        if (GameManager.Instance.gameState == GameState.Playing)
        {
            ObjectsPoolManager.Instance.DestroyPooledGameObject(gameObject);

            GameManager.Instance.FruitAteEvent.Invoke(scoreAddition, transform.position);
        }
    }

    private void StartingAnimation()
    {
        iTween.ShakePosition(gameObject, iTween.Hash(
                "x", .035f,
                "y", .035f,
                "time", .2f,
                "delay", .1f,
                "easeType", iTween.EaseType.easeOutCirc));
    }
}
