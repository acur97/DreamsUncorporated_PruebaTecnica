using UnityEngine;

public class Pool<T> where T : Object
{
    public T[] objects;

    public Pool(int size, T prefab, Transform parent = null)
    {
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

    //public T GetComponet(int index)
    //{
    //    return (objects[index] as GameObject).GetComponent<T>();
    //}

    public void Enable(int index)
    {
        (objects[index] as GameObject).SetActive(true);
    }

    public void Disable(int index)
    {
        (objects[index] as GameObject).SetActive(false);
    }
}