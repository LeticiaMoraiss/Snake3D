using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHead : MonoBehaviour {

	void Start ()
    {
        // .. Usado para atualizar a pontuação quando as frutas comiam        
        GameManager.Instance.FruitAteEvent.AddListener(OnFruitAte);
    }

    /// <summary>
    /// invocado quando a snake come uma fruta
    /// </summary>
    private void OnFruitAte(int scoreAddition, Vector3 position)
    {
        // .. Sacode a cabeça da snake quando ela come uma fruta
        iTween.ShakeScale(transform.GetChild(0).gameObject, iTween.Hash(
                "amount", new Vector3(1f, 1f, 1f),
                "time", .25f,
                "delay", 0f,
                "easeType", iTween.EaseType.linear));
    }
}
