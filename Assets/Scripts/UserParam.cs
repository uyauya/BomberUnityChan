using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;	//シーンをまたいで使用する際に使用

// プレイヤーパラメータ管理（セーブ時使用）
public class UserParam
{
	public static UserParam instanse;

	public int Level;			// プレイヤーレベル
	public int AttackPoint;		// 攻撃力（PlayerController参照）
	public float boostPointMax;	// ブーストポイント最大値（PlayerController参照）
	public float armorPointMax;	// プレイヤー体力最大値（PlayerAp参照）
	public int Score;			// 点数兼経験値（BattleManager参照）
	public int PlayerNo;		// プレイヤーNo取得用(0でこはく、1でゆうこ、2でみさき）SelectEventスクリプト参照
	public string SceneName;	// 面の名前
	public int StageNo;
	public int ClearNo;

	public UserParam(int Pno, int level, int attackPoint, float boostMax, float armorMax, int score, string sceneName, int stageNo )
	{
		PlayerNo = Pno;				 
		Level = level;
		AttackPoint = attackPoint;
		boostPointMax = boostMax;
		armorPointMax = armorMax;
		Score = score;
		SceneName = sceneName;
		StageNo = stageNo;
		instanse = this;
	}

	public UserParam(){
	}

	// セーブ時の処理
	/*public void SaveData () 
	{
		UserParam instanse = new UserParam (
			                     DataManager.PlayerNo,
			                     DataManager.Level,
			                     DataManager.AttackPoint,
			                     DataManager.BoostPointMax,
			                     DataManager.ArmorPointMax,
			                     DataManager.Score,
			                     SceneManager.GetActiveScene ().name,
								 StageManager.Instance.StageNo
		                     );
		//UserParamインスタンスを文字列に変換
		string UserParamSaveJson = JsonUtility.ToJson(instanse);
		//セーブ
		PlayerPrefs.SetString("UserParam",UserParamSaveJson);
		//Debug.Log (UserParamSaveJson);
	}

	// ロード時の処理
	public void LoadData()
	{
		// Jsonの文字列データをUserParamインスタンスに変換
		string UserParamLoadJson = PlayerPrefs.GetString ("UserParam");
		//データを変数に設定してロード
		instanse = JsonUtility.FromJson<UserParam> (UserParamLoadJson);
		DataManager.PlayerNo = instanse.PlayerNo;
		DataManager.Level = instanse.Level;
		DataManager.AttackPoint = instanse.AttackPoint;
		DataManager.BoostPointMax = instanse.boostPointMax;
		DataManager.ArmorPointMax = instanse.armorPointMax;
		DataManager.Score = instanse.Score;
		SceneManager.LoadScene(StageManager.Instance.StageName[StageManager.Instance.StageNo]);
	}*/
}	
