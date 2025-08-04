using UnityEngine;

public class Pool<T> where T : Object
{
    public int size;
    public T[] objects;

    public Pool(int size, T prefab, Transform parent = null)
    {
        this.size = size;
        objects = new T[size];

        for (int i = 0; i < size; i++)
        {
            objects[i] = Object.Instantiate(prefab, parent);
        }
    }

    public T Get(int index)
    {
        return objects[index];
    }

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