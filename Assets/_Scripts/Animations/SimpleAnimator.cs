using UnityEngine;

public class SimpleAnimator : MonoBehaviour
{
    public new SpriteRenderer renderer;
    public SpriteSheets spriteSheet;
    public bool noLifeTime = false;

    private float timer = 0;
    private int frame = 0;
    private float life = 0;

    /// <summary>
    /// Starts the first frame of the animation
    /// </summary>
    private void OnEnable()
    {
        life = 0;
        timer = 0;
        frame = 0;
        renderer.sprite = spriteSheet.sprites[frame];
    }

    /// <summary>
    /// Changes the sprites depending on the intervals
    /// </summary>
    private void Update()
    {
        if (spriteSheet.lifeTime != 0 && !noLifeTime)
        {
            life += Time.deltaTime;

            if (life >= spriteSheet.lifeTime)
            {
                gameObject.SetActive(false);
                return;
            }
        }

        if (spriteSheet.intervals != 0)
        {
            timer += Time.deltaTime;
            if (timer >= spriteSheet.intervals)
            {
                timer = 0;
                NextFrame();
            }
        }
    }

    /// <summary>
    /// Manually updates to the next frame
    /// </summary>
    public void NextFrame()
    {
        frame = (frame + 1) % spriteSheet.sprites.Length;
        renderer.sprite = spriteSheet.sprites[frame];
    }
}