using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player Data")]
    public PlayerController localPlayer;

    [Header("Game Data")]
    public DataDB dataDB;

    [Header("Moons")]
    [SerializeField] private List<GravityAttractor> moonList = new List<GravityAttractor>();
    public Dictionary<int, GravityAttractor> moons = new Dictionary<int, GravityAttractor>();

    [Header("Spawn Points")]
    public Transform entityMap;
    public List<Transform> spawnPoints = new List<Transform>();

    [Header("Random")]
    public System.Random random;

    [Header("Active Game Data")]
    public Difficulty difficulty;
    public Wave currentWave;
    public float waveDelayTime = 3.0f;
    public float waveCountdownTime = 0.0f;    
    
    private int currentEntityId = 1000;
    private bool gameEnded = false;

    [Header("Active Game Data")]
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI countdownText;
    private int baseUIcountdownValue = 3;
    private int UIcountdownValue = 3;

    //-----------------------------------------------------------------------------------//
    //Initialization and Update Functions
    //-----------------------------------------------------------------------------------//
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("GameManager can only exist once in the scene");
            return;
        }
        instance = this;

        int seed = 1337;
        random = new System.Random(seed);

        int index = 0;
        foreach (GravityAttractor moon in moonList)
        {
            moons.Add(index, moon);
            index += 1;
        }
        dataDB.Initialize();
    }

    private void Update()
    {
        if (!gameEnded && (currentWave == null || !currentWave.EnemiesRemaining()))
        {
            waveCountdownTime -= Time.deltaTime;
            UIWaveCountDown();
            if (waveCountdownTime <= 0)
            {
                waveCountdownTime = waveDelayTime;
                UIcountdownValue = baseUIcountdownValue;

                int waveNumber = 1;
                if (currentWave != null)
                {
                    waveNumber = currentWave.waveNumber + 1;
                }

                gameEnded = CheckEndGame(waveNumber);
                if (gameEnded) return;

                Wave nextWave = WaveSystem.CalculateWave(waveNumber);
                WaveSystem.SpawnWave(nextWave);
                currentWave = nextWave;

                UpdateUI();
            }
        }
    }

    public int GetNextEntityId()
    {
        int id = currentEntityId;
        currentEntityId++;
        return id;
    }
    //-----------------------------------------------------------------------------------//

    //-----------------------------------------------------------------------------------//
    //Spawn And Moon Functions
    //-----------------------------------------------------------------------------------//
    public void EnableMoon(int moonNumber)
    {
        moons.TryGetValue(moonNumber, out GravityAttractor moon);
        if (moon != null)
        {
            moon.gameObject.SetActive(true);
        }
    }

    public Vector3 GetRandomSpawn(int moonCount)
    {
        int spawnsPerMoon = 6;
        int spawn = random.Next(spawnsPerMoon * moonCount);
        return spawnPoints[spawn].transform.position;
    }
    //-----------------------------------------------------------------------------------//

    //-----------------------------------------------------------------------------------//
    //End Game Functions
    //-----------------------------------------------------------------------------------//
    public bool CheckEndGame(int waveNumber)
    {
        if (waveNumber >= 32 && difficulty == Difficulty.Easy)
        {
            EndGame();
            return true;
        }
        else if (waveNumber >= 64 && difficulty == Difficulty.Normal)
        {
            EndGame();
            return true;
        }
        else if (waveNumber >= 128 && difficulty == Difficulty.Hard)
        {
            EndGame();
            return true;
        }
        else if (waveNumber >= 256 && difficulty == Difficulty.Impossible)
        {
            EndGame();
            return true;
        }
        return false;
    }

    public void EndGame()
    {
        Debug.Log("Game Ended!");
        Debug.Log(difficulty.ToString());
    }
    //-----------------------------------------------------------------------------------//

    //-----------------------------------------------------------------------------------//
    //UI Functions
    //-----------------------------------------------------------------------------------//
    public void UpdateUI()
    {
        waveText.text = String.Format("Wave {0}", currentWave.waveNumber);
    }

    public void UIWaveCountDown()
    {
        if (waveCountdownTime <= UIcountdownValue)
        {            
            countdownText.text = UIcountdownValue.ToString();
            UIcountdownValue -= 1;

            Color currentColor = new Color(countdownText.color.r, countdownText.color.g, countdownText.color.b, 0.0f);
            Color newColor = new Color(countdownText.color.r, countdownText.color.g, countdownText.color.b, 1.0f);

            Sequence sequence = DOTween.Sequence();
            sequence.Insert(0.0f, countdownText.DOColor(newColor, 0.5f).SetEase(Ease.Linear));
            sequence.Insert(0.5f, countdownText.DOColor(currentColor, 1.0f).SetEase(Ease.Linear));
            sequence.Play();
        }
    }
    //-----------------------------------------------------------------------------------//
}
