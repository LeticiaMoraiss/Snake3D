using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : PooledObject
{
    [SerializeField]
    private float Minimum = 2f;

    [SerializeField]
    private float Maximum = 11f;

    void Awake()
    {
        iTween.Init(gameObject);     // inicializar o mecanismo de interpolação para esse objeto para evitar soluções        

        StartingAnimation();         // reproduzir a animação inicial        

        // .. Instancia o evento game over para que a bomba não funcione quando o jogador perde
        GameManager.Instance.GameOverEvent.AddListener(OnGameOver);
    }

    public override void OnPooledObjectActivated()
    {
        base.OnPooledObjectActivated();

        // .. Instancia o evento game over para que a bomba não funcione quando o jogador perde
        GameManager.Instance.GameOverEvent.AddListener(OnGameOver);

        StartingAnimation();         // reproduz a animação inicial        

        StartCoroutine(DestroyAfter(Random.Range(Minimum, Maximum)));
    }

    /// <summary>
    /// Destroi a bomba após uma duração aleatória    
    /// </summary>
    /// <param name="dur"></param>
    /// <returns></returns>
    IEnumerator DestroyAfter(float dur)
    {
        yield return new WaitForSeconds(dur);

        ObjectsPoolManager.Instance.DestroyGameObjectWithPooledChildren(gameObject);

        // .. Ao destruir a bomba, invoca o evento para informar o DIfficulyProgression.cs a gerar uma nova bomba         
        GameManager.Instance.BombDepletedEvent.Invoke();
    }

    void OnTriggerEnter(Collider other)
    {
        // .. Colide com a snake(a camada de colisão da fruta só colide com a cabeça da snake)        
        if (GameManager.Instance.gameState == GameState.Playing)
        {
            GameManager.Instance.GameOverEvent.Invoke();
;
            ObjectsPoolManager.Instance.DestroyGameObjectWithPooledChildren(gameObject);
        }
    }

    private void OnGameOver()
    {
        ObjectsPoolManager.Instance.DestroyGameObjectWithPooledChildren(gameObject);
    }

    private void StartingAnimation()
    {
        iTween.ScaleAdd(transform.GetChild(0).gameObject, iTween.Hash(
                "amount", new Vector3(.1f, .1f, .1f),
                "time", .15f,
                "looptype", iTween.LoopType.pingPong,
                "easeType", iTween.EaseType.linear));
    }

    public override void OnPooledObjectDeactivated()
    {
        base.OnPooledObjectDeactivated();

        GameManager.Instance.GameOverEvent.RemoveListener(OnGameOver);
    }
}
