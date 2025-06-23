using UnityEngine;

[CreateAssetMenu(fileName = "NewJunctionConfig", menuName = "Traffic/Junction Config")]
public class JunctionConfigSO : ScriptableObject
{
    public string junctionName;
    public StrategyType strategyType;
    public float greenTime = 5f;
    public float yellowTime = 2f;
    public float redTime = 5f;

    public static JunctionConfigSO CreateDefaultConfig()
    {
        JunctionConfigSO config = ScriptableObject.CreateInstance<JunctionConfigSO>();
        config.junctionName = "Default Junction";
        config.strategyType = StrategyType.Normal;
        config.greenTime = 5f;
        config.yellowTime = 2f;
        config.redTime = 5f;
        return config;
    }
}
