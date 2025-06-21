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

        // Baþlat / Durdur
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(simulationRunning ? "Stop Simulation" : "Start Simulation"))
        {
            simulationRunning = !simulationRunning;
            if (simulationRunning) TrafficLightManager.Instance.StartAllLights();
            else TrafficLightManager.Instance.StopAllLights();
        }

        if (GUILayout.Button("Emergency (All Red)"))
        {
            TrafficLightManager.Instance.ForceAllRed();
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

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

        GUILayout.Label($"Remaining: {light.CurrentStateTime:F1}s");
        GUILayout.Label($"Strategy: {light.strategyType}");

        if (GUILayout.Button("Request pedestrian pass"))
        {
            //light.RequestPedestrianCrossing();
        }
    }
}
