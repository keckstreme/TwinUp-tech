using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    private ITrafficLightState currentState;

    [Header("Durations (s) - these are overriden if part of a junction")]
    public float GreenDuration = 5f;
    public float YellowDuration = 2f;
    public float RedDuration = 5f;

    [Header("Traffic Light Heads")]
    public List<TrafficLightHead> lightHeads;

    public JunctionGroup junctionGroup;

    public bool BelongsToJunctionGroup => junctionGroup != null;

    [Header("Strategy is overriden if part of a junction")]
    public StrategyType strategyType = StrategyType.Normal;
    public ITrafficStrategy Strategy;

    public TrafficLightColor CurrentColor { get; private set; } = TrafficLightColor.Red;
    public float CurrentStateTime { get; private set; } = 0f;

    void Start()
    {
        junctionGroup = GetComponentInParent<JunctionGroup>();
        if (junctionGroup == null)
        {
            ChangeState(new GreenState()); // Default state if no junction group is found
            print("Not a mebmer of any junction group.");
        }
        else
        {
            // If not added in inspector, add here
            if (!junctionGroup.lights.Contains(this)) junctionGroup.lights.Add(this);
        }

        // Pick strategy
        switch (strategyType)
        {
            case StrategyType.Adaptive:
                Strategy = new AdaptiveStrategy();
                break;
            case StrategyType.Normal:
            default:
                Strategy = new NormalStrategy();
                break;
        }
    }

    void Update()
    {
        if (!BelongsToJunctionGroup) currentState?.Update(this);
    }

    public void ChangeStrategy(ITrafficStrategy newStrategy)
    {
        Strategy = newStrategy;
    }

    public void ChangeState(ITrafficLightState newState)
    {
        currentState?.Exit(this);
        currentState = newState;
        currentState.Enter(this);
    }

    public void SetLightColor(TrafficLightColor color)
    {
        foreach (var head in lightHeads)
        {
            head.SetLightColor(color);
        }
        CurrentColor = color;
    }

    public void SetStateTimer(float time)
    {
        CurrentStateTime = time;
    }
}
