using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TrafficLightManager : MonoBehaviour
{
    public static TrafficLightManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        Application.targetFrameRate = 60;
        FindAllJunctions();
        FindAllLights();
    }

    public List<JunctionGroup> junctions = new List<JunctionGroup>();
    private void FindAllJunctions()
    {
        junctions.Clear();
        junctions.AddRange(FindObjectsByType<JunctionGroup>(FindObjectsSortMode.None));
    }

    public void ForceAllRed()
    {
        foreach (var junction in junctions)
        {
            foreach (var light in junction.lights)
            {
                light.ChangeState(new RedState());
            }
        }
    }

    private List<TrafficLight> trafficLights = new List<TrafficLight>();
    private void FindAllLights()
    {
        trafficLights.Clear();
        trafficLights.AddRange(FindObjectsByType<TrafficLight>(FindObjectsSortMode.None));
    }

    public void StartAllLights()
    {
    }

    public void StopAllLights()
    {
    }
}
