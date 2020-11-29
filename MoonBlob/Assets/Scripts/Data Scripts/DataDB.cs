using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataDB", menuName = "DataDB")]
public class DataDB : ScriptableObject
{
    [Header("Enemies")]
    [SerializeField] private List<EntitySO> enemySOList = new List<EntitySO>();
    [SerializeField] private List<GameObject> enemyPrefabList = new List<GameObject>();
    public Dictionary<string, EntitySO> enemySOs = new Dictionary<string, EntitySO>();
    public Dictionary<string, GameObject> enemyPrefabs = new Dictionary<string, GameObject>();

    public void Initialize()
    {
        enemySOs = new Dictionary<string, EntitySO>();
        enemyPrefabs = new Dictionary<string, GameObject>();
        for (int i = 0; i < enemySOList.Count; i++)
        {
            enemySOs.Add(enemySOList[i].entityName, enemySOList[i]);
            enemyPrefabs.Add(enemySOList[i].entityName, enemyPrefabList[i]);
        }        
    }
}
