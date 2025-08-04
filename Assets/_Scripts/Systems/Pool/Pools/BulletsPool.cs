using UnityEngine;

public class BulletsPool : MonoBehaviour
{
    public static BulletsPool Instance;

    [SerializeField] private SimpleAnimator prefab;
    [SerializeField] private int poolSize;
    [SerializeField] private float speed;
    [SerializeField] private float limitY;

    [Space]
    [SerializeField] private SpriteSheets playerBulletSpriteSheet;
    [SerializeField] private SpriteSheets[] enemieBulletSpriteSheets;

    private Pool<SimpleAnimator> poolObjects;

    [HideInInspector] public int currentPlayerBulletIndex = -1;

    private void Awake()
    {
        Instance = this;

        poolObjects = new Pool<SimpleAnimator>(poolSize, prefab, transform);
    }

    public void InitBullet(Enums.BulletType type, Vector2 position)
    {
        poolObjects.Get(out SimpleAnimator _object, out int index);

        _object.transform.localPosition = position;

        if (type == Enums.BulletType.PlayerBullet)
        {
            currentPlayerBulletIndex = index;
            _object.spriteSheet = playerBulletSpriteSheet;
            _object.transform.localEulerAngles = new Vector3(0, 0, 180);
        }
        else
        {
            _object.spriteSheet = enemieBulletSpriteSheets[Random.Range(0, enemieBulletSpriteSheets.Length)];
            _object.transform.localEulerAngles = Vector3.zero;
        }

        _object.gameObject.SetActive(true);
    }

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
                poolObjects.objects[i].transform.position -= speed * Time.deltaTime * poolObjects.objects[i].transform.up;
            }
            else
            {
                poolObjects.objects[i].transform.position -= speed * 0.5f * Time.deltaTime * poolObjects.objects[i].transform.up;
            }

            if (poolObjects.objects[i].transform.position.y >= limitY ||
                poolObjects.objects[i].transform.position.y <= -limitY)
            {
                ReturnBullet(poolObjects.objects[i].gameObject);
            }
        }
    }

    public void ReturnBullet(GameObject bullet /*, Agregar enum de tipo de vfx al morir */)
    {
        bullet.SetActive(false);

        if (bullet.transform.GetSiblingIndex() == currentPlayerBulletIndex)
        {
            currentPlayerBulletIndex = -1;
        }
    }
}