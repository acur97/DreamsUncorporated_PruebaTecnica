using UnityEngine.Scripting;

[Preserve]
public class Tags
{
    public const string Player = "Player";
    public const string Enemy = "Enemy";
    public const string Bullet = "Bullet";
    public const string Obstacle = "Obstacle";
}

[Preserve]
public class Enums
{
    public enum BulletType
    {
        PlayerBullet,
        EnemyBullet
    }

    public enum VfxType
    {
        PlayerDeath,
        EnemyDeath,
        BigEnemyDeath,
        ObstacleEnemyCollision,
        ObstaclePlayerCollision
    }
}