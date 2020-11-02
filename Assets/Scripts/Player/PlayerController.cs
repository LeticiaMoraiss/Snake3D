// Este script é responsável por todos os movimentos da Snake
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public PlayerDirection direction;       // Direção do jogador (cima, baixo, esquerda ou direita)

    [HideInInspector]
    public float stepLength = .2f;          // Move por esse valor a cada intervalo 

    [HideInInspector]
    public float movementFrequency = .1f;   // Muda a frequencia de quadro 

    [SerializeField]
    private GameObject nodePrefab;          // O nó da snake que será criado ao comer uma fruta

    private float defaultMovementFreq = .1f;// Redefina a frequência de movimento para esse valor ao reiniciar o jogo
    private List<Vector3> deltaPosition;    // Armazena o próximo deslocamento de posição com base na direção do jogador
    private List<Rigidbody> nodes;          // Acompanha os nós de criados para controlar o movimento 
    private Vector3 fruitNodePosition;      // armazena a posição do nó que é gerada comendo uma fruta

    private Rigidbody headRB;             
    private Transform tr;

    private float counter = 0f;
    private bool move = false;
    private bool createNodeAtTail = false;     // essa tag é acionada ao comer uma fruta para criar um novo nó na cauda da cobra

    void Awake()
    {
        tr = transform;        // Componente de transformação da snake (ficar maior) 

        InitSnakeNodes();      // Obtem referências para nós de cobra

        InitPlayer();          // modifica as partes da cobra com base na direção atribuída

                deltaPosition = new List<Vector3>()
        {
            new Vector3(-stepLength, 0f),  // -dx .. Esquerda
            new Vector3(0f, stepLength),   // dy  .. Acima
            new Vector3(stepLength, 0f),   // dx  .. Direita
            new Vector3(0f, -stepLength)   // -dy .. Baixo
        };

        // .. instancia o envento para comer as frutas 
        GameManager.Instance.FruitAteEvent.AddListener(OnFruitAte);

        // .. instancia o envento para reset do jogo 
        GameManager.Instance.ResetEvent.AddListener(OnReset);
    }

    void Update()
    {
        if (GameManager.Instance.gameState == GameState.Playing)
        {
            counter += Time.deltaTime;

            if (counter >= movementFrequency)
            {
                counter = 0f;
                move = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (move && GameManager.Instance.gameState == GameState.Playing)
        {
            move = false;

            Move();
        }
    }

    /// <summary>
    /// Move a cobra em uma direção
    /// </summary>
    private void Move()
    {
        // .. Obtem o deslocamento com base na direção do jogador
        Vector3 dPosition = deltaPosition[(int)direction];

        Vector3 parentPos = headRB.position;
        Vector3 prevPos;
        // .. Move o nó principal primeiro e depois o resto da snake
        headRB.position = headRB.position + dPosition;

        // .. Move todos os nós de snake
        for (int i = 1; i < nodes.Count; i++)
        {
            // .. Atribue a posição de cada nó à posição do nó a seguir!
                        prevPos = nodes[i].position;

            nodes[i].position = parentPos;

            parentPos = prevPos;
        }

        // .. Depois de mover todos os nós da snake, verifica se devemos criar um nó extra porque comemos uma fruta!
        if (createNodeAtTail)
        {
            createNodeAtTail = false;

            GameObject newNode = ObjectsPoolManager.Instance.GetPooledObject(nodePrefab, fruitNodePosition, Quaternion.identity);
            newNode.transform.SetParent(transform, true);
            nodes.Add(newNode.GetComponent<Rigidbody>());
        }
    }

    /// <summary>
    /// Move a snake imediatamente se a entrada for detectada
    /// </summary>
    private void ForceMove()
    {
        counter = 0f;

        move = false;

        Move();
    }

    /// <summary>
    /// Adiciona a cabeça, o nó e a cauda da snake a uma fila
    /// </summary>
    private void InitSnakeNodes()
    {
        nodes = new List<Rigidbody>();
        
        nodes.Add(tr.GetChild(0).GetComponent<Rigidbody>());    // cabeça
        nodes.Add(tr.GetChild(1).GetComponent<Rigidbody>());    // Nó do corpo
        nodes.Add(tr.GetChild(2).GetComponent<Rigidbody>());    // Rabo

        headRB = nodes[0];
    }

    private void SetDirectionRandom()
    {
        int ranDirection = Random.Range(0, (int)PlayerDirection.Count);
        direction = (PlayerDirection)ranDirection;
    }

    public void SetInputDirection(PlayerDirection dir)
    {
        // .. Impede o movimento na direção oposta
        if (dir == PlayerDirection.Up && direction == PlayerDirection.Down ||
            dir == PlayerDirection.Down && direction == PlayerDirection.Up ||
            dir == PlayerDirection.Right && direction == PlayerDirection.Left ||
            dir == PlayerDirection.Left && direction == PlayerDirection.Right)
        {
            return;
        }

        direction = dir;

        ForceMove();     // Move a snake imediatamente, sem esperar pelo próximo movimento
    }

    // Chamado quando a snake come uma fruta
    private void OnFruitAte(int scoreAddition, Vector3 fruitPosition)
    {
        // .. Especifica a nova posição da fruta como a última posição do nó
                fruitNodePosition = nodes[nodes.Count - 1].position;

        createNodeAtTail = true;
    }

    /// <summary>
    /// Reorganiza as partes da snake com base em uma direção aleatória
    /// </summary>
    private void InitPlayer()
    {
        SetDirectionRandom();  // Define uma direção inicial aleatória

        switch (direction)
        {
            case PlayerDirection.Right:   // desloca o nó do meio e a cauda da snake para a esquerda da cabeça
                nodes[1].position = nodes[0].position - new Vector3(Metrics.SNACK_NODE, 0f, 0f);
                nodes[2].position = nodes[0].position - new Vector3(Metrics.SNACK_NODE * 2, 0f, 0f);
                break;

            case PlayerDirection.Left:    // desloca o nó do meio e a cauda da snake para a direita da cabeça 
                nodes[1].position = nodes[0].position + new Vector3(Metrics.SNACK_NODE, 0f, 0f);
                nodes[2].position = nodes[0].position + new Vector3(Metrics.SNACK_NODE*2, 0f, 0f);
                break;

            case PlayerDirection.Up:     // Mudar para baixo
                nodes[1].position = nodes[0].position - new Vector3(0f, Metrics.SNACK_NODE, 0f);
                nodes[2].position = nodes[0].position - new Vector3(0f, Metrics.SNACK_NODE*2f, 0f);
                break;

            case PlayerDirection.Down:     // Mudar para cima
                nodes[1].position = nodes[0].position + new Vector3(0f, Metrics.SNACK_NODE, 0f);
                nodes[2].position = nodes[0].position + new Vector3(0f, Metrics.SNACK_NODE * 2f, 0f);
                break;
        }
    }

    /// <summary>
    /// Redefine o player para o estado inicial(3 nós e no meio da tela)
    /// </summary>
    private void OnReset()
    {
        createNodeAtTail = false;

        // Redefine frequência de movimento
        movementFrequency = defaultMovementFreq;

        // .. Limpa todos os nós extras e deixa apenas três nós(cabeça, nó do corpo e cauda)
        while (nodes.Count > 3)
        {
            Rigidbody node = nodes[3];

            nodes.Remove(node);       // Remove da lista

            // .. Destroi o nó de snake extra
            ObjectsPoolManager.Instance.DestroyGameObjectWithPooledChildren(node.gameObject);
        }

        // ..  Reposiciona a cabeça da cobra no centro da tela
                nodes[0].transform.position = new Vector3(0f, 0f, 5.72f);

        // .. Reorganiza os nós de snake com base na nova direção aleatória atribuída
        InitPlayer();
    }

    /// <summary>
    /// Chamado pelo DiffitivityPregression.cs para aumentar a dificuldade
    /// </summary>
    /// <param name="amount"></param>
    public void DecreaseMovementFrequency(float amount, float minMovementFreq)
    {
        float newFreq = movementFrequency - amount;

        movementFrequency = Mathf.Max(newFreq, minMovementFreq);
    }
}
