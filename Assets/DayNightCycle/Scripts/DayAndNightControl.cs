//2016 Spyblood Games

using Assets.Scripts.Settings;
using UnityEngine;



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
    public float currentTime { get; set; } 
    //RPC variables: currentTime, DayLength, pause, 

   
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
		

        //Check if default light prefab exists, and if so, disable it
        if (GameObject.Find("LightSet"))
        {
            GameObject.Find("LightSet").SetActive(false);
        }

        

    }


    // Update is called once per frame
    void Update()
    {
        //The below syncs the field of view of the moon camera and the main camera, and removes unwanted issues with moon rendering
        //(main camera's field of view changes alot, and if the moon camera's doesnt, it distorts the moon's rendering)
        GameObject.Find("MoonCamera").GetComponent<Camera>().fieldOfView = GameObject.Find("MainCamera").GetComponent<Camera>().fieldOfView;
        if (!pause)
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

        if (currentTime < 0.2f)
        {
            RenderSettings.ambientSkyColor = darknightColors.skyColor;
            RenderSettings.ambientEquatorColor = darknightColors.equatorColor;
            RenderSettings.ambientGroundColor = darknightColors.horizonColor;
            GameObject.Find("MainCamera").GetComponent<Skybox>().material = skyBoxNIGHT;
        }
        if (currentTime > 0.2f && currentTime < 0.25f) {
           
            RenderSettings.ambientSkyColor = Color.Lerp(RenderSettings.ambientSkyColor, nightColors.skyColor, 0.001f / (SecondsInAFullDay / 40));
            RenderSettings.ambientEquatorColor = Color.Lerp(RenderSettings.ambientEquatorColor, nightColors.equatorColor, 0.001f / (SecondsInAFullDay / 40));
            RenderSettings.ambientGroundColor = Color.Lerp(RenderSettings.ambientGroundColor, nightColors.horizonColor, 0.001f / (SecondsInAFullDay / 40));
            //RenderSettings.skybox.Lerp(skyBoxNIGHT, skyBoxDAWN, 0.01f);
            //DynamicGI.UpdateEnvironment();
            /*RenderSettings.ambientSkyColor = nightColors.skyColor;
			RenderSettings.ambientEquatorColor = nightColors.equatorColor;
			RenderSettings.ambientGroundColor = nightColors.horizonColor;*/
            GameObject.Find("MainCamera").GetComponent<Skybox>().material=skyBoxDAWN;
           
        }
		if (currentTime > 0.25f && currentTime < 0.40f) {
            RenderSettings.ambientSkyColor = Color.Lerp(RenderSettings.ambientSkyColor, dawnColors.skyColor, 0.001f / (SecondsInAFullDay / 600));

            //RenderSettings.ambientSkyColor = dawnColors.skyColor;
            RenderSettings.ambientEquatorColor = Color.Lerp(RenderSettings.ambientEquatorColor, dawnColors.equatorColor, 0.001f/(SecondsInAFullDay/600));
            //RenderSettings.ambientEquatorColor = dawnColors.equatorColor;
            RenderSettings.ambientGroundColor = Color.Lerp(RenderSettings.ambientGroundColor, dawnColors.horizonColor, 0.001f/(SecondsInAFullDay/600));
            //RenderSettings.ambientGroundColor = dawnColors.horizonColor;
            GameObject.Find("MainCamera").GetComponent<Skybox>().material = skyBoxDAWN;

            if (currentTime > 0.35f)
            {
                RenderSettings.ambientSkyColor = dawnColors.skyColor;
                RenderSettings.ambientEquatorColor = dawnColors.equatorColor;
                RenderSettings.ambientGroundColor = dawnColors.horizonColor;
                GameObject.Find("MainCamera").GetComponent<Skybox>().material = skyBoxDAWN;
            }

        }
		if (currentTime > 0.40f && currentTime < 0.75f) {
            RenderSettings.ambientSkyColor = Color.Lerp(RenderSettings.ambientSkyColor, dayColors.skyColor, 0.001f / (SecondsInAFullDay / 300));
            RenderSettings.ambientEquatorColor = Color.Lerp(RenderSettings.ambientEquatorColor, dayColors.equatorColor, 0.001f / (SecondsInAFullDay / 300));
            RenderSettings.ambientGroundColor = Color.Lerp(RenderSettings.ambientGroundColor, dayColors.horizonColor, 0.001f / (SecondsInAFullDay / 300));
            if (currentTime > 0.50f)
            {
                RenderSettings.ambientSkyColor = dayColors.skyColor;
                RenderSettings.ambientEquatorColor = dayColors.equatorColor;
                RenderSettings.ambientGroundColor = dayColors.horizonColor;
                GameObject.Find("MainCamera").GetComponent<Skybox>().material = skyBoxDAY;
            }
            GameObject.Find("MainCamera").GetComponent<Skybox>().material = skyBoxDAY;
        }
        /* if (currentTime > 0.75f && currentTime < 0.80f)
        {
            RenderSettings.ambientSkyColor = Color.Lerp(RenderSettings.ambientSkyColor, nightColors.skyColor, 0.001f / (SecondsInAFullDay / 40));
            RenderSettings.ambientEquatorColor = Color.Lerp(RenderSettings.ambientEquatorColor, nightColors.equatorColor, 0.001f / (SecondsInAFullDay / 40));
            RenderSettings.ambientGroundColor = Color.Lerp(RenderSettings.ambientGroundColor, nightColors.horizonColor, 0.001f / (SecondsInAFullDay / 40));
            /*RenderSettings.ambientSkyColor = nightColors.skyColor;
            RenderSettings.ambientEquatorColor = nightColors.equatorColor;
            RenderSettings.ambientGroundColor = nightColors.horizonColor;
            GameObject.Find("MainCamera").GetComponent<Skybox>().material = skyBoxNIGHT;

        } */

        if (currentTime > 0.75f && currentTime < 0.99f)
        {
              RenderSettings.ambientSkyColor = Color.Lerp(RenderSettings.ambientSkyColor, darknightColors.skyColor, 0.001f / (SecondsInAFullDay / 1000));//making the 1000 bigger makes lerp faster
                RenderSettings.ambientEquatorColor = Color.Lerp(RenderSettings.ambientEquatorColor, darknightColors.equatorColor, 0.001f / (SecondsInAFullDay / 1000));
                RenderSettings.ambientGroundColor = Color.Lerp(RenderSettings.ambientGroundColor, darknightColors.horizonColor, 0.001f / (SecondsInAFullDay / 1000));

            if(currentTime >  0.875f ) { 
            RenderSettings.ambientSkyColor = darknightColors.skyColor;
			RenderSettings.ambientEquatorColor = darknightColors.equatorColor;
			RenderSettings.ambientGroundColor = darknightColors.horizonColor;
            GameObject.Find("MainCamera").GetComponent<Skybox>().material = skyBoxNIGHT;
            }
            GameObject.Find("MainCamera").GetComponent<Skybox>().material = skyBoxNIGHT;
            
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
