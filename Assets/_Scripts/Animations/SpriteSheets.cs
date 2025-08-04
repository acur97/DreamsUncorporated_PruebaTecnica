using UnityEngine;

[CreateAssetMenu(fileName = "Sheet ", menuName = "Scriptable Objects/Sprite Sheet")]
public class SpriteSheets : ScriptableObject
{
    public float intervals;
    public float lifeTime;
    public Sprite[] sprites;
}