using System;
using System.Collections.Generic;

[Serializable]
public class Wave
{
    public int waveNumber;
    public Dictionary<string, int> waveEnemies = new Dictionary<string, int>();

    // Current Wave
    public Dictionary<int, Enemy> remainingEnemies = new Dictionary<int, Enemy>();

    //-----------------------------------------------------------------------------------//
    //Initialization Functions
    //-----------------------------------------------------------------------------------//
    public Wave() { }

    public Wave(int waveNumber)
    {
        this.waveNumber = waveNumber;
    }

    public Wave(int waveNumber, Dictionary<string, int> waveEnemies)
    {
        this.waveNumber = waveNumber;
        this.waveEnemies = waveEnemies;
    }

    public void Add(string enemy, int amount)
    {
        bool exists = waveEnemies.TryGetValue(enemy, out int value);
        if (exists)
        {
            waveEnemies[enemy] = value + amount;
        }
        else
        {
            waveEnemies.Add(enemy, amount);
        }
    }
    //-----------------------------------------------------------------------------------//

    //-----------------------------------------------------------------------------------//
    //Active Wave Functions
    //-----------------------------------------------------------------------------------//
    public bool EnemiesRemaining()
    {
        return remainingEnemies.Count != 0;
    }

    public void AddActiveEnemy(Enemy enemy)
    {
        remainingEnemies.Add(enemy.entityData.entityId, enemy);
    }

    public void RemoveActiveEnemy(int entityId)
    {
        remainingEnemies.Remove(entityId);
    }
    //-----------------------------------------------------------------------------------//
}
