using System;
using UnityEngine;

public class StepsTimer : MonoBehaviour
{
    public static bool PauseTimer = false;
    public static float Timer;

    public static Action OnStep;
    public static Action OnResume;
    public static Action DestroyEnemy;

    [SerializeField] private GameplaySettings settings;

    private float stepsMulti = 1;

    private void Awake()
    {
        DestroyEnemy += SetHoldSteps;
    }

    private void SetHoldSteps()
    {
        Timer -= 0.3f;
        stepsMulti += settings.stepsMultiplier;
    }

    private void Update()
    {
        Timer += Time.deltaTime * stepsMulti;

        if (Timer >= settings.stepDuration)
        {
            if (PauseTimer)
            {
                PauseTimer = false;
                OnResume?.Invoke();
            }

            Timer = 0;
            OnStep?.Invoke();
        }
    }

    private void OnDestroy()
    {
        OnStep = null;
        OnResume = null;
        DestroyEnemy = null;
    }
}