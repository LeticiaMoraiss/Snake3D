//Este scrip refere-se as paredes da cena e suas colisões
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

	void OnTriggerEnter(Collider other)
    {
        // .. Colide com o jogador(a camada de colisão da parede só colide com a cabeça do jogador)
        if (GameManager.Instance.gameState == GameState.Playing)
        {
            GameManager.Instance.GameOverEvent.Invoke();
        }
    }
}
