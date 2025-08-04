using UnityEngine;

public class VfxPool : MonoBehaviour
{
    public static VfxPool instance;

    [SerializeField] private SimpleAnimator prefab;
    [SerializeField] private int poolSize;

    [Space]
    [SerializeField] private SpriteSheets playerDeath;
    [SerializeField] private SpriteSheets enemyDeath;
    [SerializeField] private SpriteSheets bigEnemyDeath;
    [SerializeField] private SpriteSheets obstacleEnemyCollision;
    [SerializeField] private SpriteSheets obstaclePlayerCollision;

    private Pool<SimpleAnimator> pool;
    private SimpleAnimator vfx;

    private SimpleAnimator _playerDeath;
    private Pool<SimpleAnimator> _enemyDeath;
    private SimpleAnimator _bigEnemyDeath;

    private Pool<SimpleAnimator> _ObstacleEnemyCollision;
    private Pool<SimpleAnimator> _ObstaclePlayerCollision;

    private void Awake()
    {
        instance = this;

        pool = new Pool<SimpleAnimator>(poolSize, prefab, transform);
    }

    public void InitVfx(Enums.VfxType type, Vector2 position, bool dontDestroy = false)
    {
        vfx = pool.Get();
        vfx.transform.localPosition = position;

        switch (type)
        {
            case Enums.VfxType.PlayerDeath:
                vfx.spriteSheet = playerDeath;
                break;

            case Enums.VfxType.EnemyDeath:
                vfx.spriteSheet = enemyDeath;
                break;

            case Enums.VfxType.BigEnemyDeath:
                vfx.spriteSheet = bigEnemyDeath;
                break;

            case Enums.VfxType.ObstacleEnemyCollision:
                vfx.spriteSheet = obstacleEnemyCollision;
                if (dontDestroy)
                {
                    vfx.noLifeTime = true;
                    vfx.renderer.color = Color.black;
                }
                break;

            case Enums.VfxType.ObstaclePlayerCollision:
                vfx.spriteSheet = obstaclePlayerCollision;
                if (dontDestroy)
                {
                    vfx.noLifeTime = true;
                    vfx.renderer.color = Color.black;
                }
                break;
        }

        vfx.gameObject.SetActive(true);
    }
}