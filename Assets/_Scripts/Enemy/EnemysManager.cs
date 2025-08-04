using UnityEngine;

public class EnemysManager : MonoBehaviour
{
    public static EnemysManager Instance;

    [SerializeField] private GameplaySettings settings;

    [Space]
    [SerializeField] private EnemyController prefab;
    [SerializeField] private Transform parent;

    private EnemyController[,] enemies;
    private int[] lastAvailableRow;
    private Pool<EnemyController> pool;
    private float offsetX, offsetY;

    private int index = -1;
    private int firstAvailableIndex = 0;
    private int lastAvailableIndex;
    private int aliveEnemies;

    private bool direction = true;
    private bool needMoveVertical = false;
    private int skipMoveVertical = 0;
    private bool movingVertical = false;

    private float shootRandomInterval;
    private float shootTimer = 0f;

    private void Awake()
    {
        Instance = this;

        pool = new Pool<EnemyController>(settings.rows * settings.columns, prefab, parent);
        enemies = new EnemyController[settings.rows, settings.columns];
        lastAvailableRow = new int[settings.columns];
        lastAvailableIndex = pool.objects.Length - 1;
        aliveEnemies = pool.objects.Length;

        offsetY = settings.topSpawn - (settings.rows - 1) * settings.spaceY;
        offsetX = -((settings.columns - 1) * settings.spaceX) / 2f;

        for (int i = 0; i < settings.rows; i++)
        {
            for (int j = 0; j < settings.columns; j++)
            {
                enemies[i, j] = pool.Get(i * settings.columns + j);
                enemies[i, j].transform.localPosition = new Vector2(offsetX + j * settings.spaceX, offsetY + i * settings.spaceY);

                if (i == settings.rows - 1)
                {
                    enemies[i, j].animator.spriteSheet = settings.enemiesSheets[0];
                }
                else if (i == settings.rows - 2 || i == settings.rows - 3)
                {
                    enemies[i, j].animator.spriteSheet = settings.enemiesSheets[1];
                }
                else
                {
                    enemies[i, j].animator.spriteSheet = settings.enemiesSheets[2];
                }

                //enemies[i, j].gameObject.SetActive(true);
            }
        }

        StepsTimer.OnStep += Init;
        StepsTimer.OnStep += RealIndex;
        StepsTimer.OnStep += MoveEnemiesDown;
        StepsTimer.OnStep += OnStep;

        shootRandomInterval = settings.maxShootTime;
    }

    private void Init()
    {
        bool found = false;
        for (int i = 0; i < pool.objects.Length; i++)
        {
            if (!pool.objects[i].gameObject.activeSelf)
            {
                pool.objects[i].gameObject.SetActive(true);
                return;
            }
        }

        if (!found)
            StepsTimer.OnStep -= Init;
    }

    private void RealIndex()
    {
        if (!GameManager.GameStarted)
            return;

        if (aliveEnemies <= 0)
        {
            GameManager.instance.WinRound();
            //Debug.Break();
            return;
        }

        UpIndex();

        if (!pool.objects[index].gameObject.activeSelf)
        {
            RealIndex();
        }
    }

    private void UpIndex()
    {
        index++;

        if (index >= pool.objects.Length)
        {
            index = firstAvailableIndex;

            if (needMoveVertical)
            {
                movingVertical = true;
                needMoveVertical = false;
            }
        }
    }

    private void MoveEnemiesDown()
    {
        if (!movingVertical)
            return;

        pool.objects[index].MoveDown();

        if (index == lastAvailableIndex)
        {
            movingVertical = false;
            skipMoveVertical = 3;
            direction = !direction;
        }
    }

    private void OnStep()
    {
        if (!GameManager.GameStarted)
            return;

        if (movingVertical)
            return;

        pool.objects[index].MoveHorizontal(direction);

        if (index == firstAvailableIndex && skipMoveVertical == 0)
        {
            for (int i = 0; i < pool.objects.Length; i++)
            {
                if (!pool.objects[i].gameObject.activeSelf)
                    continue;

                if (pool.objects[i].transform.localPosition.x > settings.horizontalScreenLimit ||
                    pool.objects[i].transform.localPosition.x < -settings.horizontalScreenLimit)
                {
                    pool.objects[index].MoveHorizontal(direction);
                    needMoveVertical = true;
                    break;
                }

                if (pool.objects[i].transform.localPosition.y < settings.bottomScreenLimit)
                {
                    GameManager.instance.RemoveLife();
                    break;
                }
            }
        }

        if (index == firstAvailableIndex && skipMoveVertical > 0)
        {
            skipMoveVertical--;
            return;
        }
    }

    public void ReturnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        aliveEnemies--;

        for (int j = 0; j < settings.columns; j++)
        {
            bool found = false;
            for (int i = 0; i < settings.rows; i++)
            {
                if (enemies[i, j].gameObject.activeSelf)
                {
                    lastAvailableRow[j] = i;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                lastAvailableRow[j] = -1;
            }
        }

        for (int i = 0; i < pool.objects.Length; i++)
        {
            if (pool.objects[i].gameObject.activeSelf)
            {
                firstAvailableIndex = i;
                break;
            }
        }

        for (int i = pool.objects.Length - 1; i >= 0; i--)
        {
            if (pool.objects[i].gameObject.activeSelf)
            {
                lastAvailableIndex = i;
                break;
            }
        }
    }

    private void Update()
    {
        if (StepsTimer.PauseTimer ||
            !GameManager.GameStarted)
            return;

        shootTimer += Time.deltaTime * PersistanceData.GetLevelMultiplier();

        if (shootTimer >= shootRandomInterval)
        {
            shootTimer = 0f;
            shootRandomInterval = Random.Range(settings.minShootTime, settings.maxShootTime);

            SearchBottomEnemy(Random.Range(0, settings.columns));
        }
    }

    private void SearchBottomEnemy(int column)
    {
        if (lastAvailableRow[column] == -1)
        {
            column++;

            if (column >= settings.columns)
            {
                column = 0;
            }

            SearchBottomEnemy(column);
            return;
        }

        BulletsPool.Instance.InitBullet(Enums.BulletType.EnemyBullet, new Vector2(
            enemies[lastAvailableRow[column], column].transform.position.x,
            enemies[lastAvailableRow[column], column].transform.position.y - 0.45f));
    }
}