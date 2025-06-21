using UnityEngine;

public class TrafficLightHead : MonoBehaviour
{
    [Header("Lights")]
    public GameObject redLight;
    public GameObject yellowLight;
    public GameObject greenLight;

    public void SetLightColor(TrafficLightColor color)
    {
        redLight.SetActive(color == TrafficLightColor.Red);
        yellowLight.SetActive(color == TrafficLightColor.Yellow);
        greenLight.SetActive(color == TrafficLightColor.Green);
    }
}
