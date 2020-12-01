using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataDB", menuName = "DataDB")]
public class DataDB : ScriptableObject
{
    [Header("Enemies")]
    [SerializeField] private List<EntitySO> enemySOList = new List<EntitySO>();
    [SerializeField] private List<GameObject> enemyPrefabList = new List<GameObject>();
    [SerializeField] private List<Sprite> enemySpriteList = new List<Sprite>();
    public Dictionary<string, EntitySO> enemySOs = new Dictionary<string, EntitySO>();
    public Dictionary<string, GameObject> enemyPrefabs = new Dictionary<string, GameObject>();
    public Dictionary<string, Sprite> enemySprites = new Dictionary<string, Sprite>();

    [Header("Difficulty")]
    public List<DifficultyColors> difficultyColorsList = new List<DifficultyColors>();
    public Dictionary<Difficulty, Color> difficultyColors = new Dictionary<Difficulty, Color>();

    [Header("Audio Clips")]
    public AudioClip blobWalking;
    public AudioClip blobJumping;
    public AudioClip blobHit;
    public AudioClip blobDie;
    public AudioClip blobShoot;
    public AudioClip waveCountdown;
    public AudioClip startWave;
    public AudioClip explosion;

    [Header("Trophy Sprites")]
    public List<Sprite> trophySprites = new List<Sprite>();

    public void Initialize()
    {
        enemySOs = new Dictionary<string, EntitySO>();
        enemyPrefabs = new Dictionary<string, GameObject>();
        enemySprites = new Dictionary<string, Sprite>();
        for (int i = 0; i < enemySOList.Count; i++)
        {
            enemySOs.Add(enemySOList[i].entityName, enemySOList[i]);
            enemyPrefabs.Add(enemySOList[i].entityName, enemyPrefabList[i]);
            enemySprites.Add(enemySOList[i].entityName, enemySpriteList[i]);
        }

        difficultyColors = new Dictionary<Difficulty, Color>();
        for (int i = 0; i < difficultyColorsList.Count; i++)
        {
            difficultyColors.Add(difficultyColorsList[i].difficulty, difficultyColorsList[i].color);
        }
    }
}
