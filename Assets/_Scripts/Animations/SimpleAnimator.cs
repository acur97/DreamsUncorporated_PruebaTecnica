using UnityEngine;

public class SimpleAnimator : MonoBehaviour
{
    [SerializeField] private new SpriteRenderer renderer;

    public SpriteSheets spriteSheet;

    private float timer = 0;
    private int frame = 0;
    private float life = 0;

    private void OnEnable()
    {
        renderer.sprite = spriteSheet.sprites[0];
    }

    private void Update()
    {
        if (spriteSheet.lifeTime != 0)
        {
            life += Time.deltaTime;

            if (life >= spriteSheet.lifeTime)
            {
                gameObject.SetActive(false);
                return;
            }
        }

        if (spriteSheet.intervals == 0)
            return;

        timer += Time.deltaTime;
        if (timer >= spriteSheet.intervals)
        {
            timer = 0;
            frame = (frame + 1) % spriteSheet.sprites.Length;
            renderer.sprite = spriteSheet.sprites[frame];
        }
    }
}