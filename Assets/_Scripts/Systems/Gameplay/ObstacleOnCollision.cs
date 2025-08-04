using UnityEngine;

public class ObstacleOnCollision : MonoBehaviour
{
    /// <summary>
    /// Checl if a bullet hit this part of the obstacle, the whole obstacle are many of this mini colliders
    /// </summary>
    /// <param name="collision">the bullet that hit</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Bullet))
        {
            BulletsPool.Instance.ReturnBullet(collision.gameObject, true, true);

            gameObject.SetActive(false);
        }
    }
}