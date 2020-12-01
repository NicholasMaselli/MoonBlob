using System;
using System.Collections.Generic;
using UnityEngine;

public static class WaveSystem
{
    public static Dictionary<int, string> enemyLevel = new Dictionary<int, string>()
    {
        { 1, "Peep" },
        { 2, "Blurp" },
        { 3, "Glob" },
        { 4, "Yolg" },
        { 5, "Oobby" },
        { 6, "Rooboo" },
        { 7, "Broogr" },
        { 8, "Bluubr" },
    };

    public static Wave CalculateWave(int waveNumber)
    {
        Wave wave = new Wave(waveNumber);

        int waveValue = waveNumber;
        while (waveValue > 0)
        {
            // First find the highest power of 2 that is less than or equal to the current wave number + 1
            int largestPowerOfTwo = FindLargestPowerOfTwoLessThanOrEqualToNumber(waveValue + 1);
            waveValue -= ((int)Math.Pow(2, largestPowerOfTwo) - 1);
            wave.Add(enemyLevel[largestPowerOfTwo], 1);
        }
        return wave;
    }

    public static void SpawnWave(Wave wave)
    {
        GameManager.instance.currentWave = wave;
        SpawnMoon(wave);

        // Spawn all enemies on the wave, can only spawn on moons that have been enabled
        int largestPowerOfTwo = FindLargestPowerOfTwoLessThanOrEqualToNumber(wave.waveNumber + 1);
        int moonCount = largestPowerOfTwo + 1;

        foreach (KeyValuePair<string, int> enemyPair in wave.waveEnemies)
        {
            GameObject enemyPrefab = GameManager.instance.dataDB.enemyPrefabs[enemyPair.Key];
            for (int i = 0; i < enemyPair.Value; i++)
            {
                Vector3 spawnPosition = GameManager.instance.GetRandomSpawn(moonCount);
                GameObject enemyGO = GameObject.Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, GameManager.instance.entityMap);
                Enemy enemy = enemyGO.GetComponent<Enemy>();
                enemy.Initialize();

                wave.AddActiveEnemy(enemy);
            }
        }
    }

    public static void SpawnMoon(Wave wave)
    {
        bool isPowerOfTwo = Mathf.IsPowerOfTwo(wave.waveNumber + 1);
        if (isPowerOfTwo)
        {
            int moonNumber = (int)Math.Log(wave.waveNumber + 1, 2);
            GameManager.instance.EnableMoon(moonNumber);
        }
    }

    public static int FindLargestPowerOfTwoLessThanOrEqualToNumber(int number)
    {
        int index = 0;
        while (number >= 2)
        {
            number /= 2;
            index += 1;
        }
        return index;
    }
}
