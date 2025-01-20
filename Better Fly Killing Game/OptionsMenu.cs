using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Toggle debugToggle;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("DebugStats") == 1) { debugToggle.isOn = true; } else { debugToggle.isOn = false; }
    }

    public void SaveButton()
    {
        if (debugToggle.isOn) { PlayerPrefs.SetInt("DebugStats", 1); } else { PlayerPrefs.SetInt("DebugStats", 0); }
    }
}
