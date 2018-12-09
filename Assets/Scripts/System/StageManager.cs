
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ステージ管理用
public class StageManager : SingletonMonoBehaviour<StageManager>  {

	// インスペクタのElementにScene名を記入
	public string[]	StageName;
	// ステージ管理用No
	public int StageNo;

	// Use this for initialization
	void Start () {
		// シーンをまたいでデータ保持する処理
		DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
