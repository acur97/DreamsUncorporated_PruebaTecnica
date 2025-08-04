using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private GameplaySettings settings;
    [SerializeField] private AudioSource source;

    private int beatIndex = -1;

    private void Awake()
    {
        instance = this;
    }

    public void Play(Enums.AudioType type)
    {
        switch (type)
        {
            case Enums.AudioType.Beat:
                beatIndex++;
                if (beatIndex >= settings.beatClips.Length - 1)
                    beatIndex = 0;
                source.PlayOneShot(settings.beatClips[beatIndex]);
                break;

            case Enums.AudioType.Bullet:
                source.PlayOneShot(settings.bulletClip);
                break;

            case Enums.AudioType.EnemyDeath:
                source.PlayOneShot(settings.enemyDieClip);
                break;

            case Enums.AudioType.PlayerDeath:
                source.PlayOneShot(settings.playerDieClip);
                break;
        }
    }
}