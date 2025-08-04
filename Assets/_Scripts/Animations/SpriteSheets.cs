using UnityEngine;

[CreateAssetMenu(fileName = "Sheet ", menuName = "Scriptable Objects/Sprite Sheet")]
/// <summary>
/// Scriptable object that stores sprites and data for an animation
/// </summary>
public class SpriteSheets : ScriptableObject
{
    public float intervals;
    public float lifeTime;
    public Sprite[] sprites;
}