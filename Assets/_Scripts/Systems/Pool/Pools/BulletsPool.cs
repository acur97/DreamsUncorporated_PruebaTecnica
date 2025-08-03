using UnityEngine;

public class BulletsPool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int poolSize;

    private Pool<GameObject> pool;

    private void Awake()
    {
        pool = new Pool<GameObject>(poolSize, prefab);
    }
}