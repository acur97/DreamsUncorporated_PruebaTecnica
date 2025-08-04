using UnityEngine;

public class VfxPool : MonoBehaviour
{
    public static VfxPool instance;

    [SerializeField] private SimpleAnimator playerDeath;
    [SerializeField] private SimpleAnimator enemyDeath;
    [SerializeField] private SimpleAnimator bigEnemyDeath;

    [Space]
    [SerializeField] private SimpleAnimator ObstacleEnemyCollision;
    [SerializeField] private SimpleAnimator ObstaclePlayerCollision;
    [SerializeField] private SimpleAnimator BulletCollision;

    private void Awake()
    {
        instance = this;
    }

    public void InitVfx(Enums.VfxType type, Vector2 position)
    {
        switch (type)
        {
            case Enums.VfxType.PlayerDeath:
                break;

            case Enums.VfxType.EnemyDeath:
                break;

            case Enums.VfxType.BigEnemyDeath:
                break;

            case Enums.VfxType.ObstacleEnemyCollision:
                break;

            case Enums.VfxType.ObstaclePlayerCollision:
                break;

            case Enums.VfxType.BulletCollision:
                break;
        }
    }
}