using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float limitX;

    private float moving;

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moving = ctx.ReadValue<float>() * speed * Time.deltaTime;
    }

    public void OnFire(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && BulletsPool.Instance.currentPlayerBulletIndex == -1)
        {
            BulletsPool.Instance.InitBullet(Enums.BulletType.PlayerBullet, new Vector2(transform.position.x, transform.position.y + 0.4f));
        }
    }

    private void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        transform.localPosition = new Vector2(
            Mathf.Clamp(transform.localPosition.x + moving, -limitX, limitX),
            transform.localPosition.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Bullet))
        {
            BulletsPool.Instance.ReturnBullet(collision.gameObject);

            //bajar vida
        }
    }
}