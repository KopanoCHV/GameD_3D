using UnityEngine;


[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    
    //References
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Present;

    //Variables
    [SerializeField, Range(0, 24)] private float TimeOfDay;

    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = Present.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Present.FogColor.Evaluate(timePercent);

        if( DirectionalLight != null)
        {

        }
    }

    private void OnValidate()
    {
        if(DirectionalLight != null)
            return;

        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {

                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
    }
}
