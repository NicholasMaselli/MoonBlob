using UnityEngine;
using UnityEngine.UI;

public class EnemyIcon : MonoBehaviour
{
    [HideInInspector] public Enemy enemy;
    public Image enemyImage;
    public Image moonImage;
    [HideInInspector] public Color moonColor;

    public void Set(Enemy enemy, Color moonColor)
    {
        SetEnemy(GameManager.instance.dataDB.enemySprites[enemy.entityData.entityName]);
        SetMoonColor(moonColor);
    }

    public void SetEnemy(Sprite sprite)
    {
        enemyImage.sprite = sprite;
    }

    public void SetMoonColor(Color color)
    {
        moonImage.color = color;
        moonColor = color;
    }
}
