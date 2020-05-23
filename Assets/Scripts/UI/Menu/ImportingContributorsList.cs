using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImportingContributorsList : MonoBehaviour 
{
	public Text ContributorsList;
	public TextAsset ContributorsTxtFile;
	void Start () 
	{
		ContributorsList.text = ContributorsTxtFile.text;
	}
	

}
