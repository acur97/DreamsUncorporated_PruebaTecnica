using UnityEngine;

public class BulletsPool : MonoBehaviour
{
    public static BulletsPool Instance;

    [SerializeField] private GameplaySettings settings;
    [SerializeField] private SimpleAnimator prefab;
    [SerializeField] private int poolSize;

    private Pool<SimpleAnimator> poolObjects;

    [HideInInspector] public int currentPlayerBulletIndex = -1;

    private void Awake()
    {
        Instance = this;

        poolObjects = new Pool<SimpleAnimator>(poolSize, prefab, transform);
    }

    /// <summary>
    /// Init/Enable the bullet
    /// </summary>
    /// <param name="type">player or enemy</param>
    /// <param name="position">point to move the bullet</param>
    public void InitBullet(Enums.BulletType type, Vector2 position)
    {
        poolObjects.Get(out SimpleAnimator _object, out int index);

        _object.transform.localPosition = position;

        if (type == Enums.BulletType.PlayerBullet)
        {
            currentPlayerBulletIndex = index;
            _object.spriteSheet = settings.playerBulletSheet;
            _object.transform.localEulerAngles = new Vector3(0, 0, 180);
        }
        else
        {
            _object.spriteSheet = settings.enemiesBulletSheets[Random.Range(0, settings.enemiesBulletSheets.Length)];
            _object.transform.localEulerAngles = Vector3.zero;
        }

        _object.gameObject.SetActive(true);
    }

    /// <summary>
    /// Move all the active bullets and disable outside the screen bullets.
    /// </summary>
    private void Update()
    {
        for (int i = 0; i < poolSize; i++)
        {
            if (!poolObjects.objects[i].gameObject.activeSelf)
            {
                continue;
            }

            if (i == currentPlayerBulletIndex)
            {
                poolObjects.objects[i].transform.position -= settings.bulletsSpeed * Time.deltaTime * poolObjects.objects[i].transform.up;
            }
            else
            {
                poolObjects.objects[i].transform.position -= settings.bulletsSpeed * 0.5f * Time.deltaTime * poolObjects.objects[i].transform.up;
            }

            if (poolObjects.objects[i].transform.position.y >= settings.bulletScreenLimit ||
                poolObjects.objects[i].transform.position.y <= -settings.bulletScreenLimit)
            {
                ReturnBullet(poolObjects.objects[i].gameObject, true);
            }
        }
    }

    /// <summary>
    /// Disable the bullet for later use
    /// </summary>
    /// <param name="bullet">the bullet to disable</param>
    /// <param name="vfx">spawn automatic vfx</param>
    /// <param name="dontDestroy">use this and vfx = true to not destroy the vfx</param>
    public void ReturnBullet(GameObject bullet, bool vfx = false, bool dontDestroy = false)
    {
        bullet.SetActive(false);

        if (bullet.transform.GetSiblingIndex() == currentPlayerBulletIndex)
        {
            currentPlayerBulletIndex = -1;

            if (vfx)
            {
                VfxPool.instance.InitVfx(
                    Enums.VfxType.ObstaclePlayerCollision,
                    GetContactPosition(bullet.transform.GetSiblingIndex()),
                    dontDestroy);
            }
        }
        else
        {
            if (vfx)
            {
                VfxPool.instance.InitVfx(
                    Enums.VfxType.ObstacleEnemyCollision,
                    GetContactPosition(bullet.transform.GetSiblingIndex()),
                    dontDestroy);
            }
        }
    }

    /// <summary>
    /// Get the position to spawn the vfx in the border of the sprite
    /// </summary>
    /// <param name="index">use GetSiblingIndex, this pool uses the spawn order</param>
    /// <returns></returns>
    private Vector2 GetContactPosition(int index)
    {
        if (poolObjects.objects[index].transform.localEulerAngles == Vector3.zero)
        {
            return new Vector2(
                poolObjects.objects[index].transform.position.x,
                poolObjects.objects[index].transform.position.y - 1);
        }
        else
        {
            return new Vector2(
                poolObjects.objects[index].transform.position.x,
                poolObjects.objects[index].transform.position.y + 1);
        }
    }
}