using UnityEngine;

// Probably Spoonie (2019) Unity DAY AND NIGHT in 6 Minutes!. 5 October. [Online] Available at : https://www.youtube.com/watch?v=m9hj9PdO328 (Accessed: 8 August 2025)
[ExecuteAlways]
public class LightingManager : MonoBehaviour
{

    //References
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Present;

    //Variables
    [SerializeField, Range(0, 224)] private float TimeOfDay;

   

    private void Update()
    {
        if (Present == null)
            return;

        if (Application.isPlaying)
        {
            TimeOfDay += Time.deltaTime;
            TimeOfDay %= 448; //Clamp between 0 - 24
            UpdateLighting(TimeOfDay / 448); 

        }
        else
        {
            UpdateLighting(TimeOfDay / 448 );  
        }
    }

    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = Present.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Present.FogColor.Evaluate(timePercent);

        if (DirectionalLight != null)
        {
            DirectionalLight.color = Present.DirectionalColor.Evaluate(timePercent);
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170, 0));
        }
    }

    private void OnValidate()
    {
        if (DirectionalLight != null)
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
}
