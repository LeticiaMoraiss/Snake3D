using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFX : MonoBehaviour {

    [SerializeField]
    private GameObject hitEffect;        // É instanciado ao bater em uma parede ou obstáculo     

    private Transform headPosition;      // cria o efeito na posição da cabeça da cobra    

    void Start()
    {
        headPosition = transform.GetChild(0);

        GameManager.Instance.GameOverEvent.AddListener(OnGameOver);
    }

    private void OnGameOver()
    {
        // .. Cria o efeito
        Instantiate(hitEffect, headPosition.position, Quaternion.identity);
    }
}
