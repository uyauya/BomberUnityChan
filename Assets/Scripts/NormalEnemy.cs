using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : MonoBehaviour {

	protected EnemyBasic enemyBasic;//継承元（protectedにする）のEnemyBasicをenemyBasicとする
	bool dead = false;				//死亡判定
	bool damageSet;					//被ダメージ処理、一時的に移動不可(下記参照)
	public float DamageTime = 0.5f;	//ダメージ処理(硬直)時間
	bool freezeSet;					//フリーズ処理、一時的に移動不可
	public float FreezeTime = 1.0f;	//フリーズ処理(硬直)時間
	public float LastEnemySpeed;	//ダメージ、フリーズ処理する前の敵の基本スピード

	/*[CustomEditor(typeof(Zombie1))]
	public class Zombie1 : Editor	// using UnityEditor; を入れておく
	{
		bool folding = false;

		public override void OnInspectorGUI()
		{
			Zombie1 Zn1 = target as Zombie1;
			Zn1.DamageTime= EditorGUILayout.FloatField( "被ダメージ硬直時間", Zn1.DamageTime);
			Zn1.FreezeTime= EditorGUILayout.FloatField( "フリーズ硬直時間", Zn1.FreezeTime);
		}
	}*/

	// Use this for initialization
	void Start () {
		// EnemyBasicスクリプトのデータを最初に呼び出しenemyBasicとする
		enemyBasic = gameObject.GetComponent<EnemyBasic> ();
		LastEnemySpeed = enemyBasic.EnemySpeed;
	}

	// Update is called once per frame
	void Update () {
		damageSet = GetComponent<EnemyBasic> ().DamageSet;
		freezeSet = GetComponent<EnemyBasic> ().FreezeSet;
		// Animator の dead が true なら Update 処理を抜ける
		if( enemyBasic.animator.GetBool("dead") == true ) return;
		// オブジェクトの場所取りをする
		Vector3 Pog = this.gameObject.transform.position;
		// Y軸（高さ）を発生位置から0.01上で固定（Y軸を固定してオブジェクトの傾きを防ぐ）
		gameObject.transform.position = new Vector3(Pog.x , 0, Pog.z);
		// 横回転設定
		Vector3 Ros = this.gameObject.transform.rotation.eulerAngles;
		gameObject.transform.eulerAngles = new Vector3(1 ,Ros.y, 1);
		enemyBasic.timer += Time.deltaTime;
		// ターゲット（プレイヤ）と自分の距離がTargetRange値以内なら
		if (Vector3.Distance (enemyBasic.target.transform.position, transform.position) <= enemyBasic.TargetRange) {
			//ターゲットの方を徐々に向く
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation 
				(enemyBasic.target.transform.position - transform.position), Time.deltaTime * enemyBasic.EnemyRotate);
			// enemySpeed × 時間でプレイヤに向かって直線的に移動
			transform.position += transform.forward * Time.deltaTime * enemyBasic.EnemySpeed;
		}

		// ターゲット（プレイヤー）との距離がSearch値以内なら
		if (Vector3.Distance (enemyBasic.target.transform.position, transform.position) <= enemyBasic.Search) {
			//ターゲットの方を徐々に向く
			// Quaternion.LookRotation(A位置-B位置）でB位置からA位置を向いた状態の向きを計算
			// Quaternion.Slerp（現在の向き、目標の向き、回転の早さ）でターゲットにゆっくり向く
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation 
				(enemyBasic.target.transform.position - transform.position), Time.deltaTime * enemyBasic.EnemySpeed);
			//アニメーターをattackに変更（攻撃モーション）
			enemyBasic.animator.SetTrigger ("attack");
		}

		//damageSet時、スピードが0なら何もしない。0でないならDamageSetCoroutine起動（下記参照）
		if (damageSet == true) {
			if (LastEnemySpeed == 0) {
				return;
			} else {
				StartCoroutine ("DamageSetCoroutine");
			}
		}
		//freezeSet時、スピードが0なら何もしない。0でないならDamageSetCoroutine起動（下記参照）
		if (freezeSet == true) {
			if (LastEnemySpeed == 0) {
				return;
			} else {
				StartCoroutine ("FreezeSetCoroutine");
			}
		}
	}

	//攻撃が当たったらDamageTime分だけSpeedをゼロにする（動きを止める）
	IEnumerator DamageSetCoroutine (){
		enemyBasic.DamageSet = false;
		LastEnemySpeed = enemyBasic.EnemySpeed;			//直前の動きの速さをLastEnemySpeedとして保存
		enemyBasic.EnemySpeed = 0;						//スピードを0にする（硬直処理）
		yield return new WaitForSeconds(DamageTime);	//DamageTimeが経過したら
		enemyBasic.EnemySpeed = LastEnemySpeed;			//LastEnemySpeedに戻して再び移動可能にする
		//Debug.Log (LastEnemySpeed);
	}


	//攻撃が当たったらFreezeTime分だけSpeedをゼロにする（動きを止める）
	IEnumerator FreezeSetCoroutine (){
		freezeSet = false;
		float LastEnemySpeed = enemyBasic.EnemySpeed;
		enemyBasic.EnemySpeed = 0;
		yield return new WaitForSeconds(FreezeTime);
		enemyBasic.EnemySpeed = LastEnemySpeed;
	}
}

