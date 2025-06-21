using UnityEngine;

[CreateAssetMenu(fileName = "NewJunctionConfig", menuName = "Traffic/Junction Config")]
public class JunctionConfigSO : ScriptableObject
{
    public string junctionName;
    public StrategyType strategyType;
    public float greenTime = 5f;
    public float yellowTime = 2f;
    public float redTime = 5f;
}
