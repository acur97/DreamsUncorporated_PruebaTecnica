using UnityEngine;

public class EnemysManager : MonoBehaviour
{
    [SerializeField] private int rows, columns;
    [SerializeField] private float spaceX, spaceY;
    [SerializeField] private float topY;
    [SerializeField] private float limitX;
    [SerializeField] private EnemyController prefab;
    [SerializeField] private Transform parent;

    private EnemyController[,] enemies;
    private Pool<EnemyController> pool;
    private float offsetX, offsetY;

    private int index = -1;

    private bool direction = true;
    private bool needMoveVertical = false;
    private bool skipMoveVertical = false;
    private bool movingVertical = false;

    private void Awake()
    {
        pool = new Pool<EnemyController>(rows * columns, prefab, parent);
        enemies = new EnemyController[rows, columns];

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
    }

    private void RealIndex()
    {
        UpIndex();

        if (!pool.objects[index].gameObject.activeSelf)
        {
            UpIndex();
        }
    }

    private void UpIndex()
    {
        index++;

        if (index >= pool.objects.Length)
        {
            index = 0;

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
            skipMoveVertical = true;
            direction = !direction;
        }
    }

    private void OnStep()
    {
        if (movingVertical)
            return;

        pool.objects[index].MoveHorizontal(direction);

        if (index == pool.objects.Length - 1 && !skipMoveVertical)
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

        if (index == pool.objects.Length - 1 && skipMoveVertical)
        {
            skipMoveVertical = false;
            return;
        }
    }
}