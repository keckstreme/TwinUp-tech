using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunctionGroup : MonoBehaviour
{
    public JunctionConfigSO config;
    public List<TrafficLight> lights = new List<TrafficLight>();

    private int currentIndex = 0;
    private float timer = 0f;
    private enum Phase { Green, Yellow, Switch }
    private Phase currentPhase = Phase.Green;
    private ITrafficStrategy strategy;

    void Start()
    {
        currentIndex = 0;
        ResetAllLights();
        if (config == null)
        {
            config = JunctionConfigSO.CreateDefaultConfig();
        }

        strategy = CreateStrategy(config.strategyType);
        if (lights.Count > 0)
        {
            lights[currentIndex].ChangeState(new GreenState());
            //timer = lights[currentIndex].Strategy.GetGreenTime(lights[currentIndex]);
            timer = strategy.GetGreenTime(config);
        }
    }

    public void EmergencyRed()
    {
        lights[currentIndex].ChangeState(new RedState());
        timer = 5f; // Emergency red light duration
    }

    private ITrafficStrategy CreateStrategy(StrategyType type)
    {
        return type switch
        {
            StrategyType.Adaptive => new AdaptiveStrategy(),
            _ => new NormalStrategy(),
        };
    }

    void Update()
    {
        if (lights.Count == 0) return;
        if (!TrafficLightManager.Instance.simulationRunning) return;

        timer -= Time.deltaTime;

        switch (currentPhase)
        {
            case Phase.Green:
                if (timer <= 0f)
                {
                    lights[currentIndex].ChangeState(new YellowState());
                    currentPhase = Phase.Yellow;
                    timer = strategy.GetYellowTime(config);
                }
                break;

            case Phase.Yellow:
                if (timer <= 0f)
                {
                    lights[currentIndex].ChangeState(new RedState());
                    currentPhase = Phase.Switch;
                    timer = 0.5f;
                }
                break;

            case Phase.Switch:
                currentIndex = (currentIndex + 1) % lights.Count;
                lights[currentIndex].ChangeState(new GreenState());
                currentPhase = Phase.Green;
                //timer = lights[currentIndex].Strategy.GetGreenTime(lights[currentIndex]); // This is strategy of individual light. Not used here.
                timer = strategy.GetGreenTime(config);
                break;
        }

        lights[currentIndex].SetStateTimer(timer);
    }

    void ResetAllLights()
    {
        foreach (var light in lights)
        {
            light.ChangeState(new RedState());
        }
    }
}
