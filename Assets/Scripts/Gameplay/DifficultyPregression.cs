//Este script faz com que o grau de dificudade seja aumentado conforme o pecorrer do jogo
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyPregression : MonoBehaviour
{
    private float minMovementFreq = .02f; // Esta é a frequência mínima de movimento da snake, pois não queremos que o jogo seja impossível de jogar

    private float difficultyFactor = .01f;// Diminue a frequência de movimento por esse valor

    private int foodCountToIncreaseSpeed = 4;  // Cada vez a snake come 4 frutas, a velocidade aumenta
    private int foodCountToIncreaseBombCount = 7;
    private int fruitAteCount = 0;        // Acompanha o número de frutas comidas

    private int spawnBombAfter = 2;       // Começa a gerar bombas depois de 2 frutas comidas
    private float bombSpawnCounter = 0f;  // Usado para gerar bombas em intervalos específicos

    private float minBombSpawnTime = 2f;
    private float maxBombSpawnTime = 11f;
    private float nextBombSpawnTime = 0f;
    private bool canSpawnBombs = true;    // Usado para impedir que uma bomba apareça enquanto outra bomba ainda existe

    private int bombCount = 1;            // Quantidade de bombas para ir aparecendo no cenário 
    private int depletedCounter = 0;

    [SerializeField]
    private PlayerController playerController;
    void Start()
    {
        // .. Instancia o envento de frutas (audio)
        GameManager.Instance.FruitAteEvent.AddListener(OnFruitAte);

        // .. Instancia o envento de Reset (audio)
        GameManager.Instance.ResetEvent.AddListener(OnReset);

        // .. Instancia o evento da bomba quando aparece no cenário
        GameManager.Instance.BombDepletedEvent.AddListener(OnBombDepleted);
    }

    /// <summary>
    /// Aumenta a dificuldade com base no número de frutas consumidas
    /// </summary>
    /// <param name="scoreAdded">Quantidade de pontos adicionados ao comer a fruta</param>  
    /// <param name="position">posição da fruta a ser comida</param>
    private void OnFruitAte(int scoreAdded, Vector3 position)
    {
        fruitAteCount++;

        if (fruitAteCount % foodCountToIncreaseSpeed == 0)
        {
            // .. Aumenta a velocidade do jogador
            playerController.DecreaseMovementFrequency(difficultyFactor, minMovementFreq);
        }

        if (fruitAteCount % foodCountToIncreaseBombCount == 0)
        {
            // .. Aumenta o número de bombas geradas de uma só vez
            bombCount++;
        }
    }

    private void Update()
    {
        // .. Jogar bombas apenas se comemos pelo menos 2 frutas
        if (fruitAteCount >= 2 && canSpawnBombs)
        {
            // .. Calcula o tempo da próxima bomba            
            if (bombSpawnCounter == 0)
            {
                nextBombSpawnTime = Time.time + Random.Range(minBombSpawnTime, maxBombSpawnTime);
            }

            bombSpawnCounter += Time.deltaTime;

            // .. joga bombas no cenário 
            if (bombSpawnCounter + Time.time >= nextBombSpawnTime)
            {
                // .. Invoca o evento de criação da bomba para que PickupSpawner.cs o crie!                 
                depletedCounter = Random.Range(1, bombCount + 1);
                GameManager.Instance.CreateBombEvent.Invoke(depletedCounter);

                // .. Reinicia o contator 
                bombSpawnCounter = 0f;

                // .. Para de jogar bombas no cenário
                canSpawnBombs = false;
            }
        }
    }

    /// <summary>
    /// Enable spawning bombs when the last bomb is destroyed
    /// </summary>
    private void OnBombDepleted()
    {
        depletedCounter--;

        if (depletedCounter == 0)
            canSpawnBombs = true;
    }

    /// <summary>
    /// Reset eaten fruit count when returning to main menu
    /// </summary>
    private void OnReset()
    {
        canSpawnBombs = true;
        fruitAteCount = 0;
        bombSpawnCounter = 0f;
        bombCount = 1;
    }
}
