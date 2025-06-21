using UnityEngine;

public class TrafficIMGUI : MonoBehaviour
{
    public Rect windowRect = new Rect(20, 20, 120, 50);

    void OnGUI()
    {
        // Register the window. Notice the 3rd parameter
        windowRect = GUI.Window(0, windowRect, DoMyWindow, "My Window");
    }

    // Make the contents of the window
    void DoMyWindow(int windowID)
    {
        GUI.DragWindow(new Rect(0, 0, 10000, 20)); // Make the window draggable
        if (GUI.Button(new Rect(10, 20, 100, 20), "Hello World"))
        {
            print("Got a click");
        }
    }
}
