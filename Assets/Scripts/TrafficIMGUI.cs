using UnityEngine;

public class TrafficIMGUI : MonoBehaviour
{
    private Vector2 scrollPos;
    private bool simulationRunning = true;
    private static GUIStyle whiteLabelStyle;

    void OnGUI()
    {
        if (whiteLabelStyle == null)
        {
            whiteLabelStyle = new GUIStyle(GUI.skin.label);
            whiteLabelStyle.richText = true;
            whiteLabelStyle.normal.textColor = Color.white;
        }

        GUI.Box(new Rect(10, 10, 450, Screen.height - 20), "Traffic Lights Dashboard");

        GUILayout.BeginArea(new Rect(20, 40, 430, Screen.height - 60));
        scrollPos = GUILayout.BeginScrollView(scrollPos);

        // Start / Stop
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(simulationRunning ? "Stop Simulation" : "Resume Simulation"))
        {
            simulationRunning = !simulationRunning;
            if (simulationRunning) TrafficLightManager.Instance.StartAllLights();
            else TrafficLightManager.Instance.StopAllLights();
        }

        // Emergency button
        if (GUILayout.Button("Emergency (All Red)"))
        {
            TrafficLightManager.Instance.ForceAllRed();
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        // Display junctions
        foreach (var junction in TrafficLightManager.Instance.junctions)
        {
            GUILayout.Label($"<b>{junction.config.junctionName}</b>", whiteLabelStyle);

            foreach (var light in junction.lights)
            {
                GUILayout.BeginVertical("box");
                DrawLightBox(light);
                GUILayout.EndVertical();
            }

            GUILayout.Space(10);
        }

        // Display individual lights
        GUILayout.Label($"<b>Individual Lights</b>", whiteLabelStyle);
        foreach (var light in TrafficLightManager.Instance.GetAllIndividualTrafficLights())
        {
            GUILayout.BeginVertical("box");
            DrawLightBox(light);
            GUILayout.EndVertical();
        }

        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    void DrawLightBox(TrafficLight light)
    {
        Color prev = GUI.color;

        Color boxColor = light.CurrentColor switch
        {
            TrafficLightColor.Green => Color.green,
            TrafficLightColor.Yellow => Color.yellow,
            _ => Color.red
        };

        GUI.color = boxColor;
        GUILayout.Box(light.CurrentColor.ToString(), GUILayout.Height(20));
        GUI.color = prev;

        // Total wait time calculation
        float totalWaitTime = 0f;
        if (light.BelongsToJunctionGroup && light.junctionGroup != null)
        {
            var group = light.junctionGroup;
            var lights = group.lights;
            int lightCount = lights.Count;
            int myIndex = lights.IndexOf(light);

            // Active light index is the one currently green or yellow
            int currentActiveIndex = group.lights.FindIndex(l => l.CurrentColor == TrafficLightColor.Green || l.CurrentColor == TrafficLightColor.Yellow);

            // If this light is currently green or yellow, wait time is just the remaining time
            if (myIndex == currentActiveIndex)
            {
                totalWaitTime = light.CurrentStateTime;
            }
            // Red light
            else
            {
                // Add remaining time for the current active light
                if (currentActiveIndex >= 0) totalWaitTime += lights[currentActiveIndex].CurrentStateTime;

                // Add full cycle times for all lights between the current and this light
                int idx = (currentActiveIndex + 1) % lightCount;
                while (idx != myIndex)
                {
                    var l = lights[idx];
                    // Use strategy if available, otherwise default durations
                    float green = l.Strategy != null ? l.Strategy.GetGreenTime(group.config) : l.GreenDuration;
                    float yellow = l.Strategy != null ? l.Strategy.GetYellowTime(group.config) : l.YellowDuration;
                    totalWaitTime += green + yellow;
                    idx = (idx + 1) % lightCount; // if idx reaches lightCount, go to 0
                }
            }
        }
        else
        {
            totalWaitTime = light.CurrentStateTime;
        }

        GUILayout.Label($"Wait: {totalWaitTime:F1}s");
        GUILayout.Label($"Strategy: {light.strategyType}");

        if (GUILayout.Button("Request pedestrian pass"))
        {

        }
    }
}
