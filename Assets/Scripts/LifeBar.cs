using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour {

	public EnemyBasic enemyBasic;
	private Slider hpSlider;

	// Use this for initialization
	void Start () {
		hpSlider = transform.Find ("EnemyAp").GetComponent <Slider>();
		hpSlider.value = (float) enemyBasic.GetarmorPointMax () / (float) enemyBasic.GetarmorPointMax ();
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.LookAt(GameObject.Find("MainCamera").transform);
	}

	public void SetDisable() {
		gameObject.SetActive (false);
	}

	public void UpdateArmorPointValue() {
		//Debug.Log (hpSlider);
		if(hpSlider != null) {
		hpSlider.value = (float) enemyBasic.GetarmorPoint () / (float) enemyBasic.GetarmorPointMax ();
		//Debug.Log ("armorPoint");
		}
	}

}
