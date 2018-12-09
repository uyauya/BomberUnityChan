using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO ※プレイヤーレベル管理	
public class PlayerLevel : MonoBehaviour 
{
	// UserParamをuserParamListとしてリスト化する
	// 順番（pno, level, attackPoint, boostMax, armorMax, scoreの順）にレベルアップ時の数値を設定
	// 値は上からこはく、ゆうこ、みさきの順
	public static List <UserParam> userParamList = new List<UserParam>() 
	{
		new UserParam(0,01,130,3000,3000,2000, string.Empty,0),		//Level01（List0）
		new UserParam(2,01,150,3200,2700,2000, string.Empty,0),		//Level01
		new UserParam(1,01,100,3200,3000,2000, string.Empty,0),		//Level01

		new UserParam(0,02,136,3300,3150,3000, string.Empty,0),		//Level02（List1）
		new UserParam(2,02,165,3360,2830,3000, string.Empty,0),		//Level02
		new UserParam(1,02,105,3520,3150,3000, string.Empty,0),		//Level02

		new UserParam(0,03,150,3500,3460,4400, string.Empty,0),		//Level03
		new UserParam(2,03,181,3530,2970,4400, string.Empty,0),		//Level03
		new UserParam(1,03,110,3870,3310,4400, string.Empty,0),		//Level03

		new UserParam(0,04,157,3800,3630,6600, string.Empty,0),		//Level04
		new UserParam(2,04,200,3700,3120,6600, string.Empty,0),		//Level04
		new UserParam(1,04,115,4260,3470,6600, string.Empty,0),		//Level04

		new UserParam(0,05,172,4000,4000,10000, string.Empty,0),	//Level05
		new UserParam(2,05,220,3890,3280,10000, string.Empty,0),	//Level05
		new UserParam(1,05,121,4690,3650,10000, string.Empty,0),	//Level05

		new UserParam(0,06,180,4200,4200,15000, string.Empty,0),	//Level06
		new UserParam(2,06,240,4080,3440,15000, string.Empty,0),	//Level06
		new UserParam(1,06,127,5150,3830,15000, string.Empty,0),	//Level06

		new UserParam(0,07,198,4600,4620,22500, string.Empty,0),	//Level07
		new UserParam(2,07,265,4280,3620,22500, string.Empty,0),	//Level07
		new UserParam(1,07,134,5670,4020,22500, string.Empty,0),	//Level07

		new UserParam(0,08,208,4800,4850,33800, string.Empty,0),	//Level08
		new UserParam(2,08,292,4500,3800,33800, string.Empty,0),	//Level08
		new UserParam(1,08,140,6230,4220,33800, string.Empty,0),	//Level08

		new UserParam(0,09,228,5300,5340,52000, string.Empty,0),	//Level09
		new UserParam(2,09,320,4730,3900,52000, string.Empty,0),	//Level09
		new UserParam(1,09,148,6860,4430,52000, string.Empty,0),	//Level09

	};

	public Transform muzzle;				//レベルアップオブジェクト出現場所をmuzzleに設定
	public GameObject LevelUpPrefab;		//レベルアップオブジェクト格納
	public GameObject LevelUpObject;
	public int PlayerNo;					//プレイヤーNo取得用(0でこはく、1でゆうこ、2でみさき）
	public int MaxScore = 52000;			//レベル打ち止め用マックススコア設定

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void LevelUp() {
		
		// DataManagerのスコアを代入して判定
		int Score = DataManager.Score;
		// foreachで常にレベルアップ判定する
		foreach(var Param in userParamList)
		{
			//レベルアップ用スコアがレベル上限のスコア以下だったらレベルアップ判定
			if ( Param.Level > DataManager.Level && Param.Score <= Score && Score <= MaxScore) {
				//userParamListの各数値を代入する
				if (Param.PlayerNo == DataManager.PlayerNo) {
					DataManager.Level = Param.Level;
					DataManager.AttackPoint = Param.AttackPoint;
					DataManager.MpMax = Param.MagicPointMax;
					DataManager.HpMax = Param.HitPointMax;
					//（プレイヤーの）muzzleにレベルアップ用エフェクト設置
					LevelUpObject = Instantiate (LevelUpPrefab, muzzle.position, Quaternion.identity);
					if (PlayerNo == 0) {
						SoundManager.Instance.Play (42, gameObject);
					}
					if (PlayerNo == 1) {
						SoundManager.Instance.Play (43, gameObject);
					}
					if (PlayerNo == 2) {
						SoundManager.Instance.Play (44, gameObject);
					}
					DataManager.Level = Param.Level;
				}
				//すでにレベルMaxの場合は判定しない（MaxかどうかはScoreで判定）
			} else if (Param.Score <= Score && Score > MaxScore) {
				return;

			}
		}
	}

	// ゲーム開始時はレベル1と判定し、DataManagerのPlayerNoでキャラクタを判定し、各キャラクタ用のステータスを代入する。
	public void LevelInitialize(int level) {
		DataManager.Level = 1;
		DataManager.AttackPoint = userParamList[0].AttackPoint;
		DataManager.MpMax = userParamList[0].MagicPointMax;
		DataManager.HpMax = userParamList[0].HitPointMax;
		}

	public static UserParam SearchParam(int PlayerNo, int Level) {
		foreach (UserParam Param in userParamList) {
			if (Param.PlayerNo == PlayerNo && Param.Level == Level) {
				return Param;
			}
		}
		return null;
	}

}
