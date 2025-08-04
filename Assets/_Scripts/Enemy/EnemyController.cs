using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public SimpleAnimator animator;

    public void MoveHorizontal(bool right)
    {
        transform.localPosition = new Vector2(
            right ? transform.localPosition.x + 0.2f : transform.localPosition.x - 0.2f,
            transform.localPosition.y);

        UpdateSpriteFrame();
    }

    public void MoveDown()
    {
        transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y - 0.2f);
    
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
            GameManager.instance.UpScores(30);
        }
        else if (collision.CompareTag(Tags.Obstacle))
        {
            Debug.Log("Obstacle");
        }
    }
}