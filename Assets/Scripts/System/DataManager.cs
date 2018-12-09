
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;	//シーンをまたいで使用する際に使用

// シーンをまたいでデータ保持する処理
public class DataManager : SingletonMonoBehaviour<DataManager> {
	[System.NonSerialized]
	public static int PlayerNo;			  //プレイヤーNo取得用(0でこはく、1でゆうこ、2でみさき）SelectEventスクリプト参照
	public static UserParam userParam;	  // UserParamスクリプトのデータをuserParamとして使用する
	public static bool Continue = false;  //Continue判定（初回はStartのみなのでfalse）
	public static int Level;			  //プレイヤーレベル
	public static int AttackPoint;		　//攻撃力
	public static float MpMax;	　//最大ブーストポイント
	public static float HpMax;	　//最大HP
	public static int Score;			　//スコア（兼経験値）
	public static string SceneName;		　//ステージ名
	public static int ClearScene;		　//クリアしたステージ（判定用）
	public static bool FarstLevel;		　//ゲーム開始時のプレイヤーレベル

	// Use this for initialization
	void Start () {
		// シーン移動してもプレイヤーのステータスを残しておく
		// レベルでステータスを管理する
		DontDestroyOnLoad (this.gameObject);
		FarstLevel = false;
		userParam = new UserParam ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// セーブ時の処理（Json使用）
	// プレイヤーステータス更新（＝は"イコール"でなく"代入"を意味する）
	public void SaveData (string SlotName) 
	{
		UserData userData = new UserData ();	// ユーザーデータに新しいユーザーデータを入れる（更新する）
		userData.PlayerNo = PlayerNo;			// ユーザーデータの（キャラ判別用）プレイヤナンバーを更新する
		userData.Level = Level;
		userData.AttackPoint = AttackPoint;
		userData.MPMax = MpMax;
		userData.HPMax = HpMax;
		userData.Score = Score;
		// StageManagerスクリプトのステージナンバーを呼び出し
		userData.SceneName = StageManager.Instance.StageName[StageManager.Instance.StageNo];
		userData.ClearScene = ClearScene;		// クリアしたステージ情報を更新する

		//UserParamインスタンスを文字列に変換（Jsonを使ってセーブする為の処理）
		string UserParamSaveJson = JsonUtility.ToJson(userData);
		Debug.Log (UserParamSaveJson);
		//セーブ画面のセーブスロット番号とプレイヤーステータスを紐づけてUserParamに保存
		PlayerPrefs.SetString("UserParam" + SlotName,UserParamSaveJson);
		//Debug.Log (UserParamSaveJson);
	}

	// ロード時の処理（Json使用）
	// 基本的にSave処理の逆。UserParamに保存したデータを呼び出して更新
	public void LoadData(string SlotName)
	{
		// Jsonの文字列データをUserParamインスタンスに変換
		string UserParamLoadJson = PlayerPrefs.GetString ("UserParam" + SlotName);
		//データを変数に設定してロード
		UserData instance = JsonUtility.FromJson<UserData> (UserParamLoadJson);
		PlayerNo = instance.PlayerNo;
		Level = instance.Level;

		UserParam Param = PlayerLevel.SearchParam (PlayerNo, Level);
		AttackPoint = Param.AttackPoint;
		MpMax = Param.MagicPointMax;
		HpMax = Param.HitPointMax;
		Score = instance.Score;
		SceneName = instance.SceneName;
		ClearScene = instance.ClearScene;
	}
}
