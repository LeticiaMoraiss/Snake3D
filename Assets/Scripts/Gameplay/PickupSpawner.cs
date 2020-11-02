// PickupSpawner.cs: gera captadores como frutas e qualquer outro tipo de captador adicionado
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField]
    private List<Pickups> pickups;      // Atribua todos os captadores no inspetor  

    private Transform pickupsParent;    // Pai de todos os captadores, usados ​​para organização de cenas e excluídos na partida
    private int bombAmount = 1;         // contagem de bombas jogadas
    void Start()
    {
        // .. Instancia o evento fruit para gerar uma nova coleta de frutas        
        GameManager.Instance.FruitAteEvent.AddListener(OnFruitAte);

        // .. Instancia o evento fruit para gerar uma nova coleta de frutas  
        GameManager.Instance.CreateBombEvent.AddListener(OnCreateBomb);

        // ..  Instancia o evento de início do jogo para gerar uma nova coleta de frutas no início       
        GameManager.Instance.GameCommencingEvent.AddListener(OnGameCommence);

        // .. Instancia o evento game over para limpar todos os captadores
        GameManager.Instance.GameOverEvent.AddListener(OnGameOver);

        pickupsParent = GameObject.FindGameObjectWithTag(Tags.pickupsParent).transform;
    }

    private void OnFruitAte(int scoreAdded, Vector3 position)
    {
        StartCoroutine(CreateFruitRandom());
    }

    private void OnCreateBomb(int amount = 1)
    {
        bombAmount = amount;

        StartCoroutine(CreateBombRandom());
    }

    private void OnGameCommence()
    {
        // .. Certifique-se de que todos os captadores sejam limpos ao iniciar o jogo
        OnGameOver();

        StartCoroutine(CreateFruitRandom(false));
    }

    /// <summary>
    /// Gera um novo objeto de fruta em uma posição aleatória e válida
    /// </summary>
    private IEnumerator CreateFruitRandom(bool playEatFruit = true)
    {
        while(true)
        {
            int ranX = Random.Range((int)WorldBorders.LeftBorder, (int)WorldBorders.RightBorder);
            int ranY = Random.Range((int)WorldBorders.BottomBorder, (int)WorldBorders.TopBorder);
            Vector3 newPosition = new Vector3(ranX, ranY, 5.72f);

            Collider[] hit = Physics.OverlapSphere(newPosition, (int)Metrics.FRUIT);

            // .. Se colidir com alguma coisa, tenta encontrar outra posição válida depois de pular esse quadro para evitar lag's            
            if (hit.Length > 0)
            {
                yield return new WaitForEndOfFrame();
            }
            else
            {
                // .. Encontra um local válido, e cria a fruta nele                
                Quaternion rot = Quaternion.Euler(-90f, 0f, 0f);
                GameObject fruit = ObjectsPoolManager.Instance.GetPooledObject(pickups[0].prefab, newPosition, rot);
                fruit.transform.SetParent(pickupsParent);

                // .. joga a fruta spawn sfx
                if (playEatFruit)
                    SoundManager.Instance.PlaySoundEffect(SoundEffectName.EAT_FRUIT, 1f);
                else
                    SoundManager.Instance.PlaySoundEffect(SoundEffectName.COLLECT_FRUIT, 1f);
                yield break;
            }
        }
    }

    /// <summary>
    /// Gera um novo objeto de bomba em uma posição aleatória e válida
    /// </summary>
    private IEnumerator CreateBombRandom()
    {
        while (true)
        {
            int ranX = Random.Range((int)WorldBorders.LeftBorder, (int)WorldBorders.RightBorder);
            int ranY = Random.Range((int)WorldBorders.BottomBorder, (int)WorldBorders.TopBorder);
            Vector3 newPosition = new Vector3(ranX, ranY, 5.72f);

            Collider[] hit = Physics.OverlapSphere(newPosition, (int)Metrics.FRUIT);

            // .. Se colidir com alguma coisa, tenta encontrar outra posição válida depois de pular esse quadro para evitar lag's            
            if (hit.Length > 0)
            {
                yield return new WaitForEndOfFrame();
            }
            else
            {
                // .. Encontra um local válido, e cria a bomba nele 
                GameObject bomb = ObjectsPoolManager.Instance.GetPooledObject(pickups[1].prefab, newPosition, Quaternion.identity);
                bomb.transform.SetParent(pickupsParent);

                // .. Joga bomba spawn sfx
                SoundManager.Instance.PlaySoundEffect(SoundEffectName.SPAWN_BOMB, 1f);

                bombAmount--;
                if (bombAmount != 0)        // .. Cria outra bomba se a quantidade especificada for maior que 1                  
                                    {               
                    StartCoroutine(CreateBombRandom());
                }
                yield break;
            }
        }
    }

    /// <summary>
    /// Destroe todos os captadores no gameover
    /// </summary>
    private void OnGameOver()
    {
        foreach(Transform pickup in pickupsParent)
        {
            ObjectsPoolManager.Instance.DestroyGameObjectWithPooledChildren(pickup.gameObject);
        }
    }
}

[System.Serializable]
class Pickups
{
    public PickupType type = PickupType.Fruit;
    public GameObject prefab;
}