using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameplaySettings settings;

    public SimpleAnimator animator;

    public void MoveHorizontal(bool right)
    {
        transform.localPosition = new Vector2(
            right ?
                transform.localPosition.x + settings.enemySpeed * PersistanceData.GetLevelMultiplier() :
                transform.localPosition.x - settings.enemySpeed * PersistanceData.GetLevelMultiplier(),
            transform.localPosition.y);

        UpdateSpriteFrame();
    }

    public void MoveDown()
    {
        transform.localPosition = new Vector2(
            transform.localPosition.x,
            transform.localPosition.y - settings.enemySpeed * PersistanceData.GetLevelMultiplier());

        UpdateSpriteFrame();
    }

    private void UpdateSpriteFrame()
    {
        animator.NextFrame();
    }

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