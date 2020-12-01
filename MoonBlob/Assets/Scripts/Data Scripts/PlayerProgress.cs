using System;

[Serializable]
public class PlayerData
{
    public int easy;
    public int normal;
    public int hard;
    public int impossible;

    public PlayerData(int easy, int normal, int hard, int impossible)
    {
        // If you have earned a trophy at a higher difficulty that is greater than
        // a trophy earned at a lower difficulty, set the lower difficulty to the 
        // higher difficulty's trophy value
        if (impossible >= hard)
        {
            hard = impossible;
        }

        if (hard >= normal)
        {
            normal = hard;
        }

        if (normal >= easy)
        {
            easy = normal;
        }

        this.easy = easy;
        this.normal = normal;
        this.hard = hard;
        this.impossible = impossible;
    }
}
