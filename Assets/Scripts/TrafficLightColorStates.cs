using UnityEngine;

public enum TrafficLightColor
{
    Red,
    Yellow,
    Green
}

public interface ITrafficLightState
{
    void Enter(TrafficLight light);
    void Update(TrafficLight light);
    void Exit(TrafficLight light);
}

public class GreenState : ITrafficLightState
{
    private float timer;

    public void Enter(TrafficLight light)
    {
        timer = light.GreenDuration;
        light.SetLightColor(TrafficLightColor.Green);
        light.SetStateTimer(timer);
    }

    public void Update(TrafficLight light)
    {
        if (!TrafficLightManager.Instance.simulationRunning) return;
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            light.ChangeState(new YellowState());
        }
        light.SetStateTimer(timer);
    }

    public void Exit(TrafficLight light) { }
}

public class YellowState : ITrafficLightState
{
    private float timer;

    public void Enter(TrafficLight light)
    {
        timer = light.YellowDuration;
        light.SetLightColor(TrafficLightColor.Yellow);
        light.SetStateTimer(timer);
    }

    public void Update(TrafficLight light)
    {
        if (!TrafficLightManager.Instance.simulationRunning) return;
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            light.ChangeState(new RedState());
        }
        light.SetStateTimer(timer);
    }

    public void Exit(TrafficLight light) { }
}

public class RedState : ITrafficLightState
{
    private float timer;

    public void Enter(TrafficLight light)
    {
        timer = light.RedDuration;
        light.SetLightColor(TrafficLightColor.Red);
        light.SetStateTimer(timer);
    }

    public void Update(TrafficLight light)
    {
        if (!TrafficLightManager.Instance.simulationRunning) return;
        timer -= Time.deltaTime;
        if (timer <= -light.YellowDuration) // Wait for other light to turn red
        {
            light.ChangeState(new GreenState());
        }
        light.SetStateTimer(timer);
    }

    public void Exit(TrafficLight light) { }
}
