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
	public float MagicPointMax;	// ブーストポイント最大値（PlayerController参照）
	public float HitPointMax;	// プレイヤー体力最大値（PlayerAp参照）
	public int Score;			// 点数兼経験値（BattleManager参照）
	public int PlayerNo;		// プレイヤーNo取得用(0でこはく、1でゆうこ、2でみさき）SelectEventスクリプト参照
	public string SceneName;	// 面の名前
	public int StageNo;
	public int ClearNo;

	public UserParam(int Pno, int level, int attackPoint, float magicPointMax, float hitPointMax, int score, string sceneName, int stageNo )
	{
		PlayerNo = Pno;				 
		Level = level;
		AttackPoint = attackPoint;
		MagicPointMax = magicPointMax;
		HitPointMax = hitPointMax;
		Score = score;
		SceneName = sceneName;
		StageNo = stageNo;
		instanse = this;
	}

	public UserParam(){
	}
}	
