using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameplaySettings settings;
    [SerializeField] private new SpriteRenderer renderer;

    private float moving;

    /// <summary>
    /// Read the input for the user and sets the moving value
    /// </summary>
    /// <param name="ctx"></param>
    public void OnMove(InputAction.CallbackContext ctx)
    {
        moving = ctx.ReadValue<float>() * settings.playerSpeed * Time.deltaTime;
    }

    /// <summary>
    /// Read the input for the user and sets the fire value,
    /// also does some checks to limit the number of bullets for the player in the screen at the same time
    /// </summary>
    /// <param name="ctx"></param>
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

    /// <summary>
    /// Uses the moving value to move the player
    /// </summary>
    private void Update()
    {
        if (StepsTimer.PauseTimer ||
            !GameManager.GameStarted)
            return;

        UpdatePosition();
    }

    /// <summary>
    /// Moves the player and limit the horizontal movement
    /// </summary>
    private void UpdatePosition()
    {
        transform.localPosition = new Vector2(
            Mathf.Clamp(transform.localPosition.x + moving, -settings.horizontalPlayerLimit, settings.horizontalPlayerLimit),
            transform.localPosition.y);
    }

    /// <summary>
    /// Check if a bullet hit the player
    /// </summary>
    /// <param name="collision">the bullet that hit</param>
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

    /// <summary>
    /// Resumes the game after a life was lost, it depends on the StepsTimer ticks
    /// </summary>
    private void ResumeGame()
    {
        if (!GameManager.GameStarted)
            return;

        renderer.enabled = true;
        StepsTimer.OnResume -= ResumeGame;
    }
}