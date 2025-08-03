using System;
using UnityEngine;

public class StepsTimer : MonoBehaviour
{
    public static Action OnStep;
    public static Action<float> HoldSteps;

    [SerializeField] private float stepDuration;
    private float timer;

    private void Awake()
    {
        HoldSteps += SetHoldSteps;
    }

    private void SetHoldSteps(float hold)
    {
        timer -= hold;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= stepDuration)
        {
            timer = 0;
            OnStep?.Invoke();
        }
    }
}