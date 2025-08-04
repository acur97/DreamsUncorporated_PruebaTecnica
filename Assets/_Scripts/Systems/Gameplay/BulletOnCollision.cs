using UnityEngine;

public class BulletOnCollision : MonoBehaviour
{
    /// <summary>
    /// Check if another bullet hits this one
    /// </summary>
    /// <param name="collision">the other bullet that hit</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Bullet))
        {
            BulletsPool.Instance.ReturnBullet(collision.gameObject, true);
            BulletsPool.Instance.ReturnBullet(gameObject, true);
        }
    }
}