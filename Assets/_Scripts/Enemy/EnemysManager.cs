using UnityEngine;

public class EnemysManager : MonoBehaviour
{
    public static EnemysManager Instance;

    [SerializeField] private int rows, columns;
    [SerializeField] private float spaceX, spaceY;
    [SerializeField] private float topY;
    [SerializeField] private float limitX;
    [SerializeField] private float minTime, maxTime;

    [Space]
    [SerializeField] private EnemyController prefab;
    [SerializeField] private Transform parent;

    private EnemyController[,] enemies;
    private int[] lastAvailableRow;
    private Pool<EnemyController> pool;
    private float offsetX, offsetY;

    private int index = -1;
    private int firstAvailableIndex = 0;

    private bool direction = true;
    private bool needMoveVertical = false;
    private int skipMoveVertical = 0;
    private bool movingVertical = false;

    private float shootRandomInterval;
    private float shootTimer = 0f;

    private void Awake()
    {
        Instance = this;

        pool = new Pool<EnemyController>(rows * columns, prefab, parent);
        enemies = new EnemyController[rows, columns];
        lastAvailableRow = new int[columns];

        offsetY = topY - (rows - 1) * spaceY;
        offsetX = -((columns - 1) * spaceX) / 2f;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                enemies[i, j] = pool.Get(i * columns + j);
                enemies[i, j].transform.localPosition = new Vector2(offsetX + j * spaceX, offsetY + i * spaceY);
                enemies[i, j].gameObject.SetActive(true);
            }
        }

        StepsTimer.OnStep += RealIndex;
        StepsTimer.OnStep += MoveEnemiesDown;
        StepsTimer.OnStep += OnStep;

        shootRandomInterval = maxTime;
    }

    private void RealIndex()
    {
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

        if (index == pool.objects.Length - 1)
        {
            movingVertical = false;
            skipMoveVertical = 2;
            direction = !direction;
        }
    }

    private void OnStep()
    {
        if (movingVertical)
            return;

        pool.objects[index].MoveHorizontal(direction);

        if (index == firstAvailableIndex && skipMoveVertical == 0)
        {
            for (int i = 0; i < pool.objects.Length; i++)
            {
                if (!pool.objects[i].gameObject.activeSelf)
                    continue;

                if (pool.objects[i].transform.localPosition.x > limitX ||
                    pool.objects[i].transform.localPosition.x < -limitX)
                {
                    pool.objects[index].MoveHorizontal(direction);
                    needMoveVertical = true;
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

        for (int j = 0; j < columns; j++)
        {
            bool found = false;
            for (int i = 0; i < rows; i++)
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
    }

    private void Update()
    {
        shootTimer += Time.deltaTime;

        if (shootTimer >= shootRandomInterval)
        {
            shootTimer = 0f;
            shootRandomInterval = Random.Range(minTime, maxTime);

            //SearchBottomEnemy(Random.Range(0, columns));
        }
    }

    private void SearchBottomEnemy(int column)
    {
        if (lastAvailableRow[column] == -1)
        {
            column++;

            if (column > columns)
            {
                column = 0;
            }

            SearchBottomEnemy(column);
            return;
        }

        BulletsPool.Instance.InitBullet(Enums.BulletType.EnemyBullet, new Vector2(
            enemies[lastAvailableRow[column], column].transform.position.x,
            enemies[lastAvailableRow[column], column].transform.position.y - 0.3f));
    }
}