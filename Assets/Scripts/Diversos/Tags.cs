// .. Armazena todas as tags do jogo em uma classe pública para estar disponível em qualquer lugar.
public class Tags
{
    public const string player = "Player";
    public const string node = "Node";
    public const string food = "Food";
    public const string wall = "Wall";
    public const string fruit = "Fruit";
    public const string bomb = "Bomb";
    public const string pickupsParent = "PickupsParent";
}

// .. Armazena todas as dimensões constantes que usamos no jogo
public class Metrics
{
    public const float SNACK_NODE = .2f;
    public const float FRUIT = .2f;
    public const float OBSTACLE = .2f;
}

// .. Armazena valores das bordas do jogo
public class WorldBorders
{
    public const float LeftBorder = -4.4f;
    public const float RightBorder = 4.4f;
    public const float TopBorder = 2.4f;
    public const float BottomBorder = -2.4f;
}

// .. Armazena todas as direções que o jogador pode ter
public enum PlayerDirection
{
    Left = 0,
    Up = 1,
    Right = 2,
    Down = 3,
    Count = 4,
}

// .. Armazena todos os tipos diferentes de captadores (objetos que aparecem na cena para a snake coletar)
public enum PickupType
{
    Fruit,
    Bomb,
}