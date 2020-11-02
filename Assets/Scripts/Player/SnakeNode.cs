using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeNode : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        // .. Colide com a snake (a camada de colisão da parede só colide com a cabeça do snake)
        if (GameManager.Instance.gameState == GameState.Playing)
        {
            GameManager.Instance.GameOverEvent.Invoke();
        }
    }
}
