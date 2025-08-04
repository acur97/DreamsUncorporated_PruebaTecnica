using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameplaySettings settings;

    public SimpleAnimator animator;

    /// <summary>
    /// Move the enemy horizontally
    /// </summary>
    /// <param name="right">true = move right, false = move left</param>
    public void MoveHorizontal(bool right)
    {
        transform.localPosition = new Vector2(
            right ?
                transform.localPosition.x + settings.enemySpeed * PersistanceData.GetLevelMultiplier() :
                transform.localPosition.x - settings.enemySpeed * PersistanceData.GetLevelMultiplier(),
            transform.localPosition.y);

        UpdateSpriteFrame();
    }

    /// <summary>
    /// Move the enemy down
    /// </summary>
    public void MoveDown()
    {
        transform.localPosition = new Vector2(
            transform.localPosition.x,
            transform.localPosition.y - settings.enemySpeed * PersistanceData.GetLevelMultiplier());

        UpdateSpriteFrame();
    }

    /// <summary>
    /// Change to the next sprite frame
    /// </summary>
    private void UpdateSpriteFrame()
    {
        animator.NextFrame();
    }

    /// <summary>
    /// Check if a bullet hit this enemy
    /// </summary>
    /// <param name="collision">the bullet that hit</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Bullet))
        {
            BulletsPool.Instance.ReturnBullet(collision.gameObject);
            VfxPool.instance.InitVfx(Enums.VfxType.EnemyDeath, transform.position);
            EnemysManager.Instance.ReturnEnemy(gameObject);
            StepsTimer.DestroyEnemy?.Invoke();
            GameManager.instance.UpScores(settings.enemyScore);
        }
    }
}