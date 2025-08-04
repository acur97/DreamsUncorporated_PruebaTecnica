using UnityEngine;

public class BulletOnCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Bullet))
        {
            BulletsPool.Instance.ReturnBullet(collision.gameObject);
            BulletsPool.Instance.ReturnBullet(gameObject);

            //double vfx dead
        }
        else if (collision.CompareTag(Tags.Obstacle))
        {
            Debug.Log("Obstacle");
        }
    }
}