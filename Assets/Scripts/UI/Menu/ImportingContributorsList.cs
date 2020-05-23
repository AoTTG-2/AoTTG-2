using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImportingContributorsList : MonoBehaviour {
	public Text contributorsList;
	public TextAsset contributorsTxtFile;
	void Start () {
		contributorsList.text = contributorsTxtFile.text;
	}
	

}
