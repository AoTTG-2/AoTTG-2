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
	
	public GameObject StarDome;
	public GameObject moonState;
	public GameObject moon;
	public DayColors dawnColors;
	public DayColors dayColors;
    public DayColors nightColors;
    public DayColors darknightColors;

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
	Material starMat;

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
		starMat = StarDome.GetComponentInChildren<MeshRenderer> ().material;
        
            starMat.color = new Color(1f, 1f, 1f, 0f);

        //Check if default light prefab exists, and if so, delete it
        if (GameObject.Find("LightSet(Clone)"))
        {
            Destroy(GameObject.Find("LightSet(Clone)"));
        }

        //Duplication check
        int numDayNightControllers = FindObjectsOfType<DayAndNightControl>().Length;
        if (numDayNightControllers != 1)
        {
            Destroy(this.gameObject);
        }

        else
        {
            DontDestroyOnLoad(gameObject);
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
		StarDome.transform.Rotate (new Vector3 (0, 2f * Time.deltaTime, 0));
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
			starMat.color = new Color(1,1,1,Mathf.Lerp(1,0,Time.deltaTime));
		}
		else if (currentTime <= 0.25f) 
		{
			intensityMultiplier = Mathf.Clamp01((currentTime - 0.25f) * (1 / 0.02f));
			starMat.color = new Color(1,1,1,Mathf.Lerp(0,1,Time.deltaTime));
		}
		else if (currentTime <= 0.75f) 
		{
			intensityMultiplier = Mathf.Clamp01(1 - ((currentTime - 0.75f) * (1 / 0.02f)));
		}


		//change env colors to add mood

		if (currentTime > 0.95f && currentTime < 0.1f) {
			RenderSettings.ambientSkyColor = darknightColors.skyColor;
			RenderSettings.ambientEquatorColor = darknightColors.equatorColor;
			RenderSettings.ambientGroundColor = darknightColors.horizonColor;
		}
		if (currentTime > 0.25f && currentTime < 0.5f) {
			RenderSettings.ambientSkyColor = dawnColors.skyColor;
			RenderSettings.ambientEquatorColor = dawnColors.equatorColor;
			RenderSettings.ambientGroundColor = dawnColors.horizonColor;
		}
		if (currentTime > 0.5f && currentTime < 0.75f) {
			RenderSettings.ambientSkyColor = dayColors.skyColor;
			RenderSettings.ambientEquatorColor = dayColors.equatorColor;
			RenderSettings.ambientGroundColor = dayColors.horizonColor;
		}
		if (currentTime > 0.75f) {
			RenderSettings.ambientSkyColor = nightColors.skyColor;
			RenderSettings.ambientEquatorColor = nightColors.equatorColor;
			RenderSettings.ambientGroundColor = nightColors.horizonColor;
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
