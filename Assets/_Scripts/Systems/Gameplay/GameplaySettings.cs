using UnityEngine;

[CreateAssetMenu(fileName = "Gameplay Settings", menuName = "Scriptable Objects/Gameplay Settings", order = 1)]
/// <summary>
/// All the variables that many scripts uses
/// </summary>
public class GameplaySettings : ScriptableObject
{
    [Header("Level")]
    public int rows = 5;
    public int columns = 11;
    public int lifes = 3;

    [Space]
    public float spaceX;
    public float spaceY;
    public float topSpawn = 7.5f;
    public float horizontalScreenLimit = 10.5f;
    public float bottomScreenLimit = -7.45f;

    [Header("Bullets")]
    public float bulletScreenLimit = 11.4f;
    public float bulletsSpeed = 30;

    [Header("Player")]
    public float playerSpeed = 10;
    public float horizontalPlayerLimit = 12;

    [Header("Enemies")]
    public float enemySpeed = 0.2f;
    public int enemyScore = 30;
    public float minShootTime = 1;
    public float maxShootTime = 1.5f;

    [Header("Game Steps")]
    public float stepDuration = 0.02f;
    public float stepsMultiplier = 0.35f;

    [Header("Sprite Sheets")]
    public SpriteSheets[] enemiesSheets;

    [Space]
    public SpriteSheets playerBulletSheet;
    public SpriteSheets[] enemiesBulletSheets;

    [Space]
    public Sprite[] screenColorSheets;

    [Header("Audio")]
    public int beatInterval = 20;
    public AudioClip[] beatClips;
    public AudioClip bulletClip;
    public AudioClip enemyDieClip;
    public AudioClip playerDieClip;
}