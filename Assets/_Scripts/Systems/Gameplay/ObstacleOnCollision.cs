using UnityEngine;

public class ObstacleOnCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Bullet))
        {
            BulletsPool.Instance.ReturnBullet(collision.gameObject, true, true);

            gameObject.SetActive(false);
        }
    }
}