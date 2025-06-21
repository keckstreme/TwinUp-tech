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
    }

    public void Update(TrafficLight light)
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            light.ChangeState(new YellowState());
        }
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
    }

    public void Update(TrafficLight light)
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            light.ChangeState(new RedState());
        }
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
    }

    public void Update(TrafficLight light)
    {
        timer -= Time.deltaTime;
        if (timer <= -light.YellowDuration) // Wait for other light to turn red
        {
            light.ChangeState(new GreenState());
        }
    }

    public void Exit(TrafficLight light) { }
}
