﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    [SerializeField] private List<Color> moonColorList = new List<Color>();
    public Dictionary<int, GravityAttractor> moons = new Dictionary<int, GravityAttractor>();
    public Dictionary<int, Color> moonColors = new Dictionary<int, Color>();

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
    public bool gameEnded = false;

    [Header("Active Game UI Data")]
    public Image waveBG;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI countdownText;
    private int baseUIcountdownValue = 3;
    private int UIcountdownValue = 3;

    public GameObject enemyIconPrefab;
    public Transform enemyIconParent;
    public Dictionary<int, EnemyIcon> enemyIcons = new Dictionary<int, EnemyIcon>();

    [Header("End Game UI")]
    public RectTransform endGameUI;
    public TextMeshProUGUI endGameText;
    public Image endWaveBG;
    public TextMeshProUGUI waveEndText;
    public bool showContinue = false;
    public RectTransform continueButton;
    public RectTransform mainMenuButton;
    public int continueCount = 0;

    [Header("Audio Variables")]
    public AudioSource audioSource;

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

        index = 0;
        foreach (Color moonColor in moonColorList)
        {
            moonColors.Add(index, moonColor);
            index += 1;
        }
        dataDB.Initialize();

        if (StateManager.instance != null)
        {
            difficulty = StateManager.instance.difficulty;
        }
        else
        {
            difficulty = Difficulty.Easy;
        }

        waveBG.color = dataDB.difficultyColors[difficulty];
        waveText.color = dataDB.difficultyColors[difficulty];
    }

    private void Update()
    {
        if (!gameEnded && (currentWave == null || !currentWave.EnemiesRemaining()))
        {
            if (CheckEndGame(currentWave.waveNumber)) return;

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

                ClearWaveUI();
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
    //Wave Functions
    //-----------------------------------------------------------------------------------//
    public void ClearWaveUI()
    {
        foreach (KeyValuePair<int, EnemyIcon> enemyIconPair in enemyIcons)
        {
            EnemyIcon enemyIcon = enemyIconPair.Value;
            Destroy(enemyIcon);
        }
        enemyIcons.Clear();
    }

    public void AddWaveUI(Enemy enemy, int moonId)
    {
        GameObject enemyIconGO = Instantiate(enemyIconPrefab, enemyIconParent);
        EnemyIcon enemyIcon = enemyIconGO.GetComponent<EnemyIcon>();
        enemyIcons.Add(enemy.entityData.entityId, enemyIcon);

        Color moonColor = Color.gray;
        if (moonId >= 0)
        {
            moonColor = moonColors[moonId];
        }
        enemyIcon.Set(enemy, moonColor);
    }

    public void RemoveWaveUI(Enemy enemy)
    {
        enemyIcons.TryGetValue(enemy.entityData.entityId, out EnemyIcon enemyIcon);
        if (enemyIcon != null)
        {
            enemyIcons.Remove(enemy.entityData.entityId);
            Destroy(enemyIcon.gameObject);
        }
    }

    public void ChangeMoonWaveUI(Enemy enemy, int moonId)
    {
        enemyIcons.TryGetValue(enemy.entityData.entityId, out EnemyIcon enemyIcon);
        if (enemyIcon != null)
        {
            Color moonColor = Color.gray;
            if (moonId >= 0)
            {
                moonColor = moonColors[moonId];
            }
            enemyIcon.SetMoonColor(moonColor);
        }
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

    public bool SameMoonAsPlayer(Entity entity)
    {
        if (entity.gravityBody == null) return false;
        
        if (entity.gravityBody.gravityAttractor == localPlayer.gravityBody.gravityAttractor)
        {
            return true;
        }
        return false;
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
            float volume = 0.6f;
            AudioClip clip = GameManager.instance.dataDB.waveCountdown;
            if (UIcountdownValue <= 0)
            {
                volume = 1.0f;
                clip = GameManager.instance.dataDB.startWave;
            }

            GameManager.instance.audioSource.PlayOneShot(clip, volume);
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

    //-----------------------------------------------------------------------------------//
    //End Game Functions
    //-----------------------------------------------------------------------------------//
    public bool CheckEndGame(int waveNumber)
    {
        if (waveNumber >= 31 && difficulty == Difficulty.Easy)
        {
            EndGame(true);
            return true;
        }
        else if (waveNumber >= 63 && difficulty == Difficulty.Normal)
        {
            EndGame(true);
            return true;
        }
        else if (waveNumber >= 127 && difficulty == Difficulty.Hard)
        {
            EndGame(true);
            return true;
        }
        else if (waveNumber >= 255 && difficulty == Difficulty.Impossible)
        {
            EndGame(true);
            return true;
        }
        return false;
    }

    public void EndGame(bool victory = false)
    {
        gameEnded = true;
        LoadEndGameScreen(victory);
    }


    public void LoadEndGameScreen(bool victory = false)
    {
        endGameUI.gameObject.SetActive(true);
        endGameText.text = victory ? "Victory!" : "Game Over";
        waveEndText.text = String.Format("{0} - Wave {1}", difficulty.ToString(), currentWave.waveNumber);
        waveEndText.color = dataDB.difficultyColors[difficulty];
        endWaveBG.color = dataDB.difficultyColors[difficulty];

        continueButton.gameObject.SetActive(false);
        if (showContinue && victory == false)
        {
            continueButton.gameObject.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    public void MainMenu()
    {
        SceneManager.LoadScene("Home");
    }

    public void Continue()
    {
        continueCount += 1;
        gameEnded = false;
        endGameUI.gameObject.SetActive(false);
        localPlayer.Start();
    }
    //-----------------------------------------------------------------------------------//

}
