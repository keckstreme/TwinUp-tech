using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

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
        simulationRunning = true;
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
            junction.EmergencyRed();
        }

        foreach (var light in GetAllIndividualTrafficLights())
        {
            light.ChangeState(new RedState());
        }
    }

    private List<TrafficLight> trafficLights = new List<TrafficLight>();
    private void FindAllLights()
    {
        trafficLights.Clear();
        trafficLights.AddRange(FindObjectsByType<TrafficLight>(FindObjectsSortMode.None));
    }

    public List<TrafficLight> GetAllIndividualTrafficLights()
    {
        // Can be optimized
        return trafficLights.Where(x => !x.BelongsToJunctionGroup).ToList();
    }

    public bool simulationRunning = true;
    public void StartAllLights()
    {
        simulationRunning = true;
    }

    public void StopAllLights()
    {
        simulationRunning = false;
    }
}
