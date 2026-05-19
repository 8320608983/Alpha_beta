1️⃣ GameEvents.cs

📁 Scripts/Managers/

using System;

public static class GameEvents
{
    public static Action OnGameStart;
    public static Action OnGameEnd;
    public static Action OnGameReset;

    public static Action<int> OnCoinCollected;
    public static Action<float> OnDistanceChanged;
}
2️⃣ GameManager.cs

📁 Scripts/Managers/

using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameStart();
    }

    public void GameStart()
    {
        GameEvents.OnGameStart?.Invoke();
    }

    public void GameOver()
    {
        Debug.Log("GameOver");
        GameEvents.OnGameEnd?.Invoke();
    }

    public void ResetGame()
    {
        GameEvents.OnGameEnd?.Invoke();
        GameEvents.OnGameReset?.Invoke();
        GameEvents.OnGameStart?.Invoke();
    }
}
3️⃣ ScoreManager.cs

📁 Scripts/Managers/

using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public Transform player;

    public int coins { get; private set; }
    public float distance { get; private set; }

    bool trackScore;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        GameEvents.OnGameStart += ResetScore;
        GameEvents.OnGameEnd += StopScore;
        GameEvents.OnCoinCollected += AddCoin;
        GameEvents.OnGameReset += ResetScore;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStart -= ResetScore;
        GameEvents.OnGameEnd -= StopScore;
        GameEvents.OnCoinCollected -= AddCoin;
        GameEvents.OnGameReset -= ResetScore;
    }

    private void Update()
    {
        if (!trackScore) return;

        distance = player.position.z;
        GameEvents.OnDistanceChanged?.Invoke(distance);
    }

    void ResetScore()
    {
        coins = 0;
        distance = 0;
        trackScore = true;
    }

    void StopScore()
    {
        trackScore = false;
    }

    void AddCoin(int amount)
    {
        coins += amount;
    }
}
4️⃣ E_PlayerController.cs

📁 Scripts/Player/

using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class E_PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float forwardSpeed = 8f;
    public float speedIncreaseRate = 0.2f;

    [Header("Lane")]
    public int laneCount = 3;
    public float laneDistance = 2f;
    public float laneChangeSpeed = 12f;

    [Header("Jump")]
    public float jumpForce = 10f;
    public float gravity = -20f;

    CharacterController cc;

    Vector3 velocity;
    Vector3 startPos;

    int currentLane;

    bool canMove;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    private void Start()
    {
        startPos = transform.position;
        currentLane = laneCount / 2;
    }

    private void OnEnable()
    {
        GameEvents.OnGameStart += EnableMovement;
        GameEvents.OnGameEnd += DisableMovement;
        GameEvents.OnGameReset += ResetPlayer;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStart -= EnableMovement;
        GameEvents.OnGameEnd -= DisableMovement;
        GameEvents.OnGameReset -= ResetPlayer;
    }

    void EnableMovement()
    {
        canMove = true;
    }

    void DisableMovement()
    {
        canMove = false;
    }

    void ResetPlayer()
    {
        cc.enabled = false;

        transform.position = startPos;

        velocity = Vector3.zero;

        forwardSpeed = 8f;

        currentLane = laneCount / 2;

        cc.enabled = true;
    }

    private void Update()
    {
        if (!canMove) return;

        forwardSpeed += speedIncreaseRate * Time.deltaTime;

        Vector3 move = Vector3.forward * forwardSpeed;

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            ChangeLane(-1);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            ChangeLane(1);
        }

        float targetX = GetLaneX(currentLane);

        float diffX = targetX - transform.position.x;

        move.x = diffX * laneChangeSpeed;

        if (cc.isGrounded)
        {
            velocity.y = -1f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity.y = jumpForce;
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        cc.Move((move + velocity) * Time.deltaTime);
    }

    void ChangeLane(int lane)
    {
        currentLane += lane;
        currentLane = Mathf.Clamp(currentLane, 0, laneCount - 1);
    }

    float GetLaneX(int laneIndex)
    {
        float middleLane = (laneCount - 1) / 2f;
        return (laneIndex - middleLane) * laneDistance;
    }
}
5️⃣ E_PlayerDeath.cs

📁 Scripts/Player/

using UnityEngine;

public class E_PlayerDeath : MonoBehaviour
{
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Obstacle"))
        {
            GameManager.Instance.GameOver();
        }
    }
}
6️⃣ TileSpawner.cs

📁 Scripts/Spawner/

using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    public Transform player;
    public Transform tileHolder;

    public GameObject[] tilePrefabs;

    public float tileLength = 30f;
    public int tilesOnScreen = 6;

    float spawnZ;

    bool canSpawn;

    List<GameObject> activeTiles = new();

    private void OnEnable()
    {
        GameEvents.OnGameStart += StartSpawning;
        GameEvents.OnGameEnd += StopSpawning;
        GameEvents.OnGameReset += ResetTiles;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStart -= StartSpawning;
        GameEvents.OnGameEnd -= StopSpawning;
        GameEvents.OnGameReset -= ResetTiles;
    }

    void StartSpawning()
    {
        canSpawn = true;

        for (int i = 0; i < tilesOnScreen; i++)
        {
            SpawnTile();
        }
    }

    void StopSpawning()
    {
        canSpawn = false;
    }

    void Update()
    {
        if (!canSpawn) return;

        if (player.position.z - 20f > spawnZ - (tilesOnScreen * tileLength))
        {
            SpawnTile();
            DeleteTile();
        }
    }

    void SpawnTile()
    {
        GameObject tile = Instantiate(tilePrefabs[Random.Range(0, tilePrefabs.Length)]);

        tile.transform.position = Vector3.forward * spawnZ;

        if (tileHolder != null)
        {
            tile.transform.SetParent(tileHolder);
        }

        spawnZ += tileLength;

        activeTiles.Add(tile);
    }

    void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }

    void ResetTiles()
    {
        foreach (GameObject tile in activeTiles)
        {
            Destroy(tile);
        }

        activeTiles.Clear();

        spawnZ = 0;
    }
}
7️⃣ ObstacleSpawner.cs

📁 Scripts/Spawner/

using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public Transform player;
    public Transform obstacleHolder;

    public GameObject obstaclePrefab;

    public float spawnDistanceAhead = 50f;
    public float distanceBetweenSpawn = 15f;

    public int laneCount = 3;
    public float laneDistance = 2f;

    float lastSpawnZ;

    bool canSpawn;

    List<GameObject> spawnedObstacles = new();

    private void OnEnable()
    {
        GameEvents.OnGameStart += StartSpawning;
        GameEvents.OnGameEnd += StopSpawning;
        GameEvents.OnGameReset += ResetObstacles;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStart -= StartSpawning;
        GameEvents.OnGameEnd -= StopSpawning;
        GameEvents.OnGameReset -= ResetObstacles;
    }

    void StartSpawning()
    {
        canSpawn = true;
        lastSpawnZ = player.position.z + spawnDistanceAhead;
    }

    void StopSpawning()
    {
        canSpawn = false;
    }

    void Update()
    {
        if (!canSpawn) return;

        if (player.position.z + spawnDistanceAhead > lastSpawnZ)
        {
            SpawnObstacle();
        }
    }

    void SpawnObstacle()
    {
        int lane = Random.Range(0, laneCount);

        Vector3 spawnPos = new(
            GetLaneX(lane),
            1f,
            lastSpawnZ
        );

        GameObject obstacle = Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);

        if (obstacleHolder != null)
        {
            obstacle.transform.SetParent(obstacleHolder);
        }

        spawnedObstacles.Add(obstacle);

        lastSpawnZ += distanceBetweenSpawn;
    }

    float GetLaneX(int laneIndex)
    {
        float middleLane = (laneCount - 1) / 2f;
        return (laneIndex - middleLane) * laneDistance;
    }

    void ResetObstacles()
    {
        foreach (GameObject obstacle in spawnedObstacles)
        {
            Destroy(obstacle);
        }

        spawnedObstacles.Clear();
    }
}
8️⃣ CoinSpawner.cs

📁 Scripts/Spawner/

using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public Transform player;
    public Transform coinHolder;

    public GameObject coinPrefab;

    public float spawnDistanceAhead = 40f;
    public float distanceBetweenSpawn = 8f;

    public int laneCount = 3;
    public float laneDistance = 2f;

    float lastSpawnZ;

    bool canSpawn;

    List<GameObject> spawnedCoins = new();

    private void OnEnable()
    {
        GameEvents.OnGameStart += StartSpawning;
        GameEvents.OnGameEnd += StopSpawning;
        GameEvents.OnGameReset += ResetCoins;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStart -= StartSpawning;
        GameEvents.OnGameEnd -= StopSpawning;
        GameEvents.OnGameReset -= ResetCoins;
    }

    void StartSpawning()
    {
        canSpawn = true;
        lastSpawnZ = player.position.z + spawnDistanceAhead;
    }

    void StopSpawning()
    {
        canSpawn = false;
    }

    void Update()
    {
        if (!canSpawn) return;

        if (player.position.z + spawnDistanceAhead > lastSpawnZ)
        {
            SpawnCoin();
        }
    }

    void SpawnCoin()
    {
        int lane = Random.Range(0, laneCount);

        Vector3 spawnPos = new(
            GetLaneX(lane),
            1f,
            lastSpawnZ
        );

        GameObject coin = Instantiate(
            coinPrefab,
            spawnPos,
            Quaternion.Euler(90f, 0f, 0f)
        );

        if (coinHolder != null)
        {
            coin.transform.SetParent(coinHolder);
        }

        spawnedCoins.Add(coin);

        lastSpawnZ += distanceBetweenSpawn;
    }

    float GetLaneX(int laneIndex)
    {
        float middleLane = (laneCount - 1) / 2f;
        return (laneIndex - middleLane) * laneDistance;
    }

    void ResetCoins()
    {
        foreach (GameObject coin in spawnedCoins)
        {
            Destroy(coin);
        }

        spawnedCoins.Clear();
    }
}
9️⃣ Coin.cs

📁 Scripts/Gameplay/

using UnityEngine;

public class Coin : MonoBehaviour
{
    bool collected;

    private void OnEnable()
    {
        collected = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            collected = true;

            GameEvents.OnCoinCollected?.Invoke(1);

            gameObject.SetActive(false);
        }
    }
}
🔟 CameraFollow.cs

📁 Scripts/Camera/

using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public Vector3 offset = new(0, 5, -8);

    public float smoothSpeed = 10f;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPos = target.position + offset;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPos,
            smoothSpeed * Time.deltaTime
        );

        transform.LookAt(target.position + Vector3.forward * 5f);
    }
}
1️⃣1️⃣ UIManager.cs

📁 Scripts/UI/

using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI coinText;

    public GameObject gameOverPanel;

    private void OnEnable()
    {
        GameEvents.OnGameStart += OnGameStart;
        GameEvents.OnGameEnd += OnGameOver;

        GameEvents.OnDistanceChanged += UpdateDistance;
        GameEvents.OnCoinCollected += UpdateCoins;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStart -= OnGameStart;
        GameEvents.OnGameEnd -= OnGameOver;

        GameEvents.OnDistanceChanged -= UpdateDistance;
        GameEvents.OnCoinCollected -= UpdateCoins;
    }

    void OnGameStart()
    {
        gameOverPanel.SetActive(false);

        distanceText.text = "Distance : 0";
        coinText.text = "Coins : 0";
    }

    void OnGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    void UpdateDistance(float distance)
    {
        distanceText.text = "Distance : " + Mathf.FloorToInt(distance);
    }

    void UpdateCoins(int amount)
    {
        coinText.text = "Coins : " + ScoreManager.Instance.coins;
    }

    public void RestartButton()
    {
        GameManager.Instance.ResetGame();
    }
}
1️⃣2️⃣ CoinRotate.cs (Optional Polish)

📁 Scripts/Gameplay/

using UnityEngine;

public class CoinRotate : MonoBehaviour
{
    public float rotateSpeed = 200f;

    private void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }
}
✅ FINAL HIERARCHY
Main Camera
Directional Light

GameSystems
 ┣ GameManager
 ┣ ScoreManager
 ┣ TileSpawner
 ┣ ObstacleSpawner
 ┣ CoinSpawner

Environment
 ┣ Tiles
 ┣ Obstacles
 ┗ Coins

Player

UI
 ┣ Canvas
 ┗ UIManager
