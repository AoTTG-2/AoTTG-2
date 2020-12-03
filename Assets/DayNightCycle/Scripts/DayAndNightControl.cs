//2016 Spyblood Games

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DayColors
{
	public Color skyColor;
	public Color equatorColor;
	public Color horizonColor;
}

public class DayAndNightControl : MonoBehaviour {

    public GameObject Camera;
	public GameObject moonState;
	public GameObject moon;
	public DayColors dawnColors;
	public DayColors dayColors;
    public DayColors nightColors;
    public DayColors darknightColors;
    public Material skyBoxDAWN;
    public Material skyBoxDAY;
    public Material skyBoxNIGHT;
    public float currentTime { get; set; } //for TBG: make any variable you want to change into the same format as here

    public int currentDay = 0; 
	public Light directionalLight;
    
    private float SecondsInAFullDay = 120f;//default value is 120 seconds in one day
    public bool pause { get; set; }
    public float DayLength
    {
        get { return SecondsInAFullDay; }

        set { SecondsInAFullDay=value; }
    }

	[HideInInspector]
	public float timeMultiplier = 1f; //how fast the day goes by regardless of the secondsInAFullDay var. lower values will make the days go by longer, while higher values make it go faster. This may be useful if you're siumulating seasons where daylight and night times are altered.
	public bool showUI;
	float lightIntensity; //static variable to see what the current light's insensity is in the inspector
	

	Camera targetCam;

	// Use this for initialization
	void Start () {
		foreach (Camera c in GameObject.FindObjectsOfType<Camera>())
		{
			if (c.isActiveAndEnabled) {
				targetCam = c;
			}
		}
		lightIntensity = directionalLight.intensity; //what's the current intensity of the light
		

        //Check if default light prefab exists, and if so, delete it
        if (GameObject.Find("LightSet(Clone)"))
        {
            Destroy(GameObject.Find("LightSet(Clone)"));
        }

        

    }


    // Update is called once per frame
    void Update()
    {
        Debug.Log(pause);
        if (pause == false)
        { 
        foreach (Camera c in GameObject.FindObjectsOfType<Camera>())
        {
            if (c.isActiveAndEnabled)
            {
                targetCam = c;
            }
            UpdateLight();
            currentTime += (Time.deltaTime / SecondsInAFullDay) * timeMultiplier;
            if (currentTime >= 1)
            {
                currentTime = 0;//once we hit "midnight"; any time after that sunrise will begin.
                currentDay++; //make the day counter go up
            }
        }
        }
    }
	

    public float GetTime() => currentTime;

	void UpdateLight()
	{
		
		moon.transform.LookAt (targetCam.transform);
		directionalLight.transform.localRotation = Quaternion.Euler ((currentTime * 360f) - 90, 170, 0);
		moonState.transform.localRotation = Quaternion.Euler ((currentTime * 360f) - 100, 170, 0);
		//^^ we rotate the sun 360 degrees around the x axis, or one full rotation times the current time variable. we subtract 90 from this to make it go up
		//in increments of 0.25.

		//the 170 is where the sun will sit on the horizon line. if it were at 180, or completely flat, it would be hard to see. Tweak this value to what you find comfortable.

		float intensityMultiplier = 1;

		if (currentTime <= 0.23f || currentTime >= 0.75f) 
		{
			intensityMultiplier = 0; //when the sun is below the horizon, or setting, the intensity needs to be 0 or else it'll look weird
			
		}
		else if (currentTime <= 0.25f) 
		{
			intensityMultiplier = Mathf.Clamp01((currentTime - 0.25f) * (1 / 0.02f));
			
		}
		else if (currentTime <= 0.75f) 
		{
			intensityMultiplier = Mathf.Clamp01(1 - ((currentTime - 0.75f) * (1 / 0.02f)));
		}


		//change env colors to add mood

		if (currentTime > 0.2f && currentTime < 0.25f) {
			RenderSettings.ambientSkyColor = nightColors.skyColor;
			RenderSettings.ambientEquatorColor = nightColors.equatorColor;
			RenderSettings.ambientGroundColor = nightColors.horizonColor;
            GameObject.Find("MainCamera").GetComponent<Skybox>().material = skyBoxNIGHT;
            
        }
		if (currentTime > 0.25f && currentTime < 0.40f) {
			RenderSettings.ambientSkyColor = dawnColors.skyColor;
			RenderSettings.ambientEquatorColor = dawnColors.equatorColor;
			RenderSettings.ambientGroundColor = dawnColors.horizonColor;
            GameObject.Find("MainCamera").GetComponent<Skybox>().material = skyBoxDAWN;
            
            Debug.Log("dawn running");
        }
		if (currentTime > 0.40f && currentTime < 0.75f) {
			RenderSettings.ambientSkyColor = dayColors.skyColor;
			RenderSettings.ambientEquatorColor = dayColors.equatorColor;
			RenderSettings.ambientGroundColor = dayColors.horizonColor;
            GameObject.Find("MainCamera").GetComponent<Skybox>().material = skyBoxDAY;
            Debug.Log("day runniing");
        }
        if (currentTime > 0.75f && currentTime < 0.80f)
        {
            RenderSettings.ambientSkyColor = nightColors.skyColor;
            RenderSettings.ambientEquatorColor = nightColors.equatorColor;
            RenderSettings.ambientGroundColor = nightColors.horizonColor;
            GameObject.Find("MainCamera").GetComponent<Skybox>().material = skyBoxNIGHT;

        }
        if (currentTime > 0.80f && currentTime < 0.99f) {
			RenderSettings.ambientSkyColor = darknightColors.skyColor;
			RenderSettings.ambientEquatorColor = darknightColors.equatorColor;
			RenderSettings.ambientGroundColor = darknightColors.horizonColor;
            GameObject.Find("MainCamera").GetComponent<Skybox>().material = skyBoxNIGHT;
            Debug.Log("night runniing");
        }

		directionalLight.intensity = lightIntensity * intensityMultiplier;
	}

	public string TimeOfDay ()
	{
	string dayState = "";
		if (currentTime > 0f && currentTime < 0.1f) {
			dayState = "Midnight";
		}
		if (currentTime < 0.5f && currentTime > 0.1f)
		{
			dayState = "Morning";

		}
		if (currentTime > 0.5f && currentTime < 0.6f)
		{
			dayState = "Mid Noon";
		}
		if (currentTime > 0.6f && currentTime < 0.8f)
		{
			dayState = "Evening";

		}
		if (currentTime > 0.8f && currentTime < 1f)
		{
			dayState = "Night";
		}
		return dayState;
	}


}
