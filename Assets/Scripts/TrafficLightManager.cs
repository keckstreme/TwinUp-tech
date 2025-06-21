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
        TryStartAllJunctions();
    }

    private List<JunctionGroup> junctions = new List<JunctionGroup>();
    private void FindAllJunctions()
    {
        junctions.Clear();
        junctions.AddRange(FindObjectsByType<JunctionGroup>(FindObjectsSortMode.None));
    }

    private List<TrafficLight> trafficLights = new List<TrafficLight>();
    private void FindAllLights()
    {
        trafficLights.Clear();
        trafficLights.AddRange(FindObjectsByType<TrafficLight>(FindObjectsSortMode.None));
    }

    public void TryStartAllJunctions()
    {
        IEnumerator waitForAllLightsInitialize()
        {
            while (!AllLightsInitialized())
            {
                yield return null;
                print("Waiting for lights to initialize...");
            }
            StartAllJunctions();
        }
        StartCoroutine(waitForAllLightsInitialize());
    }

    public void StartAllJunctions()
    {
        foreach (var junction in junctions)
        {
            //junction.StartSynchronization();
        }
    }

    public bool AllLightsInitialized()
    {
        foreach (var item in trafficLights)
        {
            if (!item.initializationComplete) return false;
        }
        return true;
    }

    public void StartAllLights()
    {
    }

    public void StopAllLights()
    {
    }

    public TrafficLight GetTrafficLightById(int id)
    {
        return null;
    }
}
