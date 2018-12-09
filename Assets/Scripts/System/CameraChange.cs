using UnityEngine;
using System.Collections;

// カメラ切替用
public class CameraChange : MonoBehaviour {

	private GameObject MainCam;		// メイン画面用カメラ（正面）
	private GameObject SubCam;		// サブカメラ（上から視点用）
	
	void Start () {
		// 最初にメイン・サブカメラ両方セットしておく
		MainCam = GameObject.Find("MainCamera");
		SubCam = GameObject.Find("SubCamera");
		// 最初はサブカメラは無効にしておく
		SubCam.SetActive(false);
	}
	
	void Update () {
		// Tabキー押した際、メインカメラ無効・サブカメラ有効にして切り替える
		if(Input.GetKeyDown(KeyCode.Tab)){
			if(MainCam.activeSelf){
				MainCam.SetActive (false);
				SubCam.SetActive (true);
			// それ以外の時はメインカメラ有効・サブカメラ無効にしておく
			}else{
				MainCam.SetActive (true);
				SubCam.SetActive (false);
			}
		}
	}
	
}
