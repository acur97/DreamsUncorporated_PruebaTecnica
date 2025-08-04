using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float limitX;
    [SerializeField] private new SpriteRenderer renderer;

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
        if (StepsTimer.PauseTimer)
            return;

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
            renderer.enabled = false;
            VfxPool.instance.InitVfx(
                Enums.VfxType.PlayerDeath,
                transform.position);

            StepsTimer.OnResume += ResumeGame;
            GameManager.instance.RemoveLife();
        }
    }

    private void ResumeGame()
    {
        renderer.enabled = true;
        StepsTimer.OnResume -= ResumeGame;
    }
}