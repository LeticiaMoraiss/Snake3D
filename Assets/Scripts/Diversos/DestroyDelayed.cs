using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDelayed : MonoBehaviour {

    private float duration;
	void Start ()
    {
        // .. Destroe este objeto depois que a duração ativa for passada do texto da pontuação        
        duration = GetComponent<ScoreText>().aliveDuration;

        StartCoroutine(DestroyAfter(duration));	
	}
	
	IEnumerator DestroyAfter(float dur)
    {
        yield return new WaitForSeconds(dur);

        Destroy(gameObject);
    }
}
