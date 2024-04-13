using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster gm;
    public float scanRate;
    private float timeToScan;

    [SerializeField]
    private int maxLives = 3;
    private static int _remainingLives;

    public static int RemainingLives
    {
        get { return _remainingLives; }
    }

    void Awake()
    {
        if (gm == null){
            gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameMaster>();
        }
    }

    public Transform playerPrefab;
    public Transform spawnPoint;
    public int spawnDelay = 2;
    public Transform spawnPrefab;

    [SerializeField]
    private string levelMusic = "LevelMusic"; 

    public CameraShake cameraShake;

    [SerializeField]
    private GameObject gameOverUI;
    [SerializeField]
    private GameObject levelCompleteUI;

    private AudioManager audioManager;

    public IEnumerator _RespawnPlayer()
    {
        audioManager.PlaySound("Spawn");
        yield return new WaitForSeconds(spawnDelay);

        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation);
        //GameObject clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation) as GameObject;
        //Destroy(clone, 3f);
        
    }

    void Start()
    {
        _remainingLives = maxLives;

        audioManager = AudioManager.instance;
        if(audioManager == null)
        {
            Debug.LogError("No audio Manager");
        }
        //Play Music
        //audioManager.StopSounds();
        audioManager.PlaySound(levelMusic);
        timeToScan = scanRate;
    }
    private void FixedUpdate()
    {
        if (Time.time >= timeToScan)
        {
            AstarPath.active.Scan();
            timeToScan = Time.time + scanRate;
        }
            
    }

    public void EndGame()
    {
        Debug.Log("GAMEOVER");
        gameOverUI.SetActive(true);
        audioManager.StopSound(levelMusic);
    }

    public void LevelComplete()
    {
       levelCompleteUI.SetActive(true);
        audioManager.StopSound(levelMusic);
    }

    public static void KillPlayer(Player player)
    {
        Destroy(player.gameObject);
        _remainingLives -= 1;
        if(_remainingLives <= 0)
        {
            gm.EndGame();
        }
        else
        {
            gm.StartCoroutine(gm._RespawnPlayer());
        }
        
    }

    public static void KillEnemy(Enemy enemy)
    {
        gm._KillEnemy(enemy);
    }

    public void _KillEnemy(Enemy _enemy)
    {
        //Play sound 
        audioManager.PlaySound(_enemy.enemyDeathSound);
        Transform _clone = Instantiate(_enemy.deathParticles, _enemy.transform.position, Quaternion.identity) as Transform;

        //Camera shake
        cameraShake.Shake(_enemy.shakeAmount, _enemy.shakeLength); 
        Destroy(_enemy.gameObject);
        Destroy(_clone.gameObject, 3f);
    }
}
