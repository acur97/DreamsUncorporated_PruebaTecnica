using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameplaySettings settings;
    [SerializeField] private new SpriteRenderer renderer;

    private float moving;

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moving = ctx.ReadValue<float>() * settings.playerSpeed * Time.deltaTime;
    }

    public void OnFire(InputAction.CallbackContext ctx)
    {
        if (StepsTimer.PauseTimer ||
            !GameManager.GameStarted)
            return;

        if (ctx.performed && BulletsPool.Instance.currentPlayerBulletIndex == -1)
        {
            BulletsPool.Instance.InitBullet(Enums.BulletType.PlayerBullet, new Vector2(transform.position.x, transform.position.y + 0.45f));
            AudioManager.instance.Play(Enums.AudioType.Bullet);
        }
    }

    private void Update()
    {
        if (StepsTimer.PauseTimer ||
            !GameManager.GameStarted)
            return;

        UpdatePosition();
    }

    private void UpdatePosition()
    {
        transform.localPosition = new Vector2(
            Mathf.Clamp(transform.localPosition.x + moving, -settings.horizontalPlayerLimit, settings.horizontalPlayerLimit),
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
        if (!GameManager.GameStarted)
            return;

        renderer.enabled = true;
        StepsTimer.OnResume -= ResumeGame;
    }
}