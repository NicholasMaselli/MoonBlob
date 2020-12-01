using System;
using UnityEngine;

[Serializable]
public class DifficultyColors
{
    public Difficulty difficulty;
    public Color color;
    public DifficultyColors() { }
    public DifficultyColors(Difficulty difficulty, Color color)
    {
        this.difficulty = difficulty;
        this.color = color;
    }
}
