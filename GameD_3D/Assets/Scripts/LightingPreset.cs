using UnityEngine;

// Probably Spoonie (2019) Unity DAY AND NIGHT in 6 Minutes!. 5 October. [Online] Available at : https://www.youtube.com/watch?v=m9hj9PdO328 (Accessed: 8 August 2025)
[System.Serializable]
[CreateAssetMenu(fileName = "Lighting Preset" , menuName = "Scriptables/Lighting Preset" , order = 1)]
public class LightingPreset : ScriptableObject
{

    public Gradient AmbientColor;
    public Gradient DirectionalColor;
    public Gradient FogColor;
}
