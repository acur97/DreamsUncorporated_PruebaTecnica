using UnityEngine;

public class Pool<T> where T : Object
{
    public int size;
    public T[] objects;

    /// <summary>
    /// Constructor of the pool, its not completly generic
    /// </summary>
    /// <param name="size">size of the pool</param>
    /// <param name="prefab">the object to instantiate</param>
    /// <param name="parent">where to instantiate, can be null</param>
    public Pool(int size, T prefab, Transform parent = null)
    {
        this.size = size;
        objects = new T[size];

        for (int i = 0; i < size; i++)
        {
            objects[i] = Object.Instantiate(prefab, parent);
        }
    }

    /// <summary>
    /// Returns an object by his index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public T Get(int index)
    {
        return objects[index];
    }

    /// <summary>
    /// Returns an available object from the pool
    /// </summary>
    /// <returns></returns>
    public T Get()
    {
        for (int i = 0; i < size; i++)
        {
            if (!(objects[i] as Component).gameObject.activeSelf)
            {
                return objects[i];
            }
        }

        return null;
    }

    /// <summary>
    /// Returns an available object from the pool and its index
    /// </summary>
    /// <param name="_object">the object</param>
    /// <param name="index">his index</param>
    public void Get(out T _object, out int index)
    {
        for (int i = 0; i < size; i++)
        {
            if (!(objects[i] as Component).gameObject.activeSelf)
            {
                _object = objects[i];
                index = i;
                return;
            }
        }

        _object = null;
        index = -1;
    }
}