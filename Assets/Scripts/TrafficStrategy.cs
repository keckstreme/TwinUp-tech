using UnityEngine;

public enum StrategyType
{
    Normal,
    Adaptive
}

public interface ITrafficStrategy
{
    float GetGreenTime(TrafficLight light);
    float GetGreenTime(JunctionConfigSO config);
    float GetYellowTime(JunctionConfigSO config);
}

[System.Serializable]
public class NormalStrategy : ITrafficStrategy
{
    public float fixedGreenTime = 5f;

    public float GetGreenTime(TrafficLight light)
    {
        return fixedGreenTime;
    }

    public float GetGreenTime(JunctionConfigSO config)
    {
        return config.greenTime;
    }

    public float GetYellowTime(JunctionConfigSO config)
    {
        return config.yellowTime;
    }
}

[System.Serializable]
public class AdaptiveStrategy : ITrafficStrategy
{
    public float baseTime = 4f;
    public float maxExtraTime = 6f;

    public float GetGreenTime(TrafficLight light)
    {
        int simulatedCarCount = Random.Range(0, 10);
        float multiplier = simulatedCarCount / 10f;
        float adaptiveTime = baseTime + (maxExtraTime * multiplier);
        Debug.Log($"{light.name} Araç sayýsý: {simulatedCarCount} - {adaptiveTime:F1}s");
        return adaptiveTime;
    }

    public float GetGreenTime(JunctionConfigSO config)
    {
        int simulatedCarCount = Random.Range(0, 10);
        float multiplier = simulatedCarCount / 10f;
        float adaptiveTime = config.greenTime + ((config.greenTime * 0.5f) * multiplier);
        Debug.Log($"{config.junctionName} | Araç sayýsý: {simulatedCarCount} - {adaptiveTime:F1}s");
        return adaptiveTime;
    }

    public float GetYellowTime(JunctionConfigSO config)
    {
        return config.yellowTime;
    }
}
