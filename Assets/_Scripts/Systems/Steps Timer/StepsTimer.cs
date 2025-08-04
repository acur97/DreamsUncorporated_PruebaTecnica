using System;
using UnityEngine;

public class StepsTimer : MonoBehaviour
{
    public static Action OnStep;
    public static Action DestroyEnemy;

    [SerializeField] private float stepDuration;
    [SerializeField] private float stepsMultiplier;
    private float timer;
    private float stepsMulti = 1;

    private void Awake()
    {
        DestroyEnemy += SetHoldSteps;
    }

    private void SetHoldSteps()
    {
        timer -= 0.3f;
        stepsMulti += stepsMultiplier;
    }

    private void Update()
    {
        timer += Time.deltaTime * stepsMulti;

        if (timer >= stepDuration)
        {
            timer = 0;
            OnStep?.Invoke();
        }
    }
}