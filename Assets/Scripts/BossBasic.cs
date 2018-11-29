using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// BOSS管理用
// ステータス欄等の共通項目を保持。キャラクタ独自の項目のみ別スクリプトに書き足す。
public class BossBasic : MonoBehaviour {
	public int enemyLevel = 0;
	public Animator animator;						// Animatorセット用
	public GameObject target;						// プレイヤー認識用
	public GameObject shot;
	public float shotInterval = 0;					// 攻撃間隔計測開始
	public float shotIntervalMax = 1.0F;			// 攻撃間隔（～秒ごとに攻撃）
	public float armorPointMax = 10000;				// 最大HP
	public float armorPoint;					    // HP
	public int TargetRange;							// プレイヤをターゲット認識する距離
	public float EnemySpeed;						// 移動スピード
	public float JumpForce;							// ジャンプ力
	public float EnemyRotate;						// 振り向き速度
	public float Search;							// プレイヤを探すサーチレンジ
	public float timer;
	protected float damage;							// playerからのダメージ判定
	public void Damaged(float damagedPoint){
		this.armorPoint -= damagedPoint;			// Playerから受けたダメージの値
	}
	public int EnemyAttack = 100;					// プレイヤに与えるダメージ
	public float InvincibleTime;					// 無敵時間
	protected ModelColorChange modelColorChange;	// 点滅処理
	public float KnockBackRange;					// 攻撃を受けた際のノックバックの距離
	public float DestroyTime;						// （HP0になった際の）消滅するまでの時間
	public GameObject DestroyEffect;				// 消滅時発生エフェクト
	public float timeElapsed;
	public float timeOut;
	public BattleManager battleManager;
	PlayerLevel playerLevel;
	public int EnemyScore = 1000;					// 敵を倒した時の得点
	public GameObject Star;							// ボス面クリア用スター
	public GameObject BigStar;						// ラスボス面クリア用スター
	public bool isBoss;								// ボスの場合はインスペクタに✔を入れる
	public bool isLastBoss;							// ラスボスの場合はインスペクタに✔を入れる
	bool dead = false;								// 敵死亡時のアニメーション判定用
	private Rigidbody rb;
	public Vector3 localGravity;					// 重力設定(x,y,z)　標準の場合はｙに-9.8を入れておく
	public Transform EffectPoint;					// エフェクト発生元の位置取り
	public GameObject Hit01Prefab;					
	public GameObject Hit01Object;
	public GameObject Hit02Prefab;					
	public GameObject Hit02Object;
	public GameObject Hit03Prefab;					
	public GameObject Hit03Object;
	public GameObject Hit05Prefab;					
	public GameObject Hit05Object;
	public GameObject DeadPrefab;					
	public GameObject DeadObject;
	public bool DamageSet;							// ダメージ判定の有無
	public float Mscale = 1.0f;						// 縮小（第一段階）				
	public float Sscale = 1.0f;						// 縮小（第二段階）
	public GameObject BossLifeBar;
	public Color DamageColor;
	public Color FreezeColor;
	//public float StarHeight = 1.0f;

	/*[CustomEditor(typeof(Zombie))]
	public class ZombieEditor : Editor	// using UnityEditor; を入れておく
	{
		bool folding = false;

		public override void OnInspectorGUI()
		{
			Zombie En = target as Zombie;
			En.armorPointMax = EditorGUILayout.FloatField( "最大HP", En.armorPointMax);
			En.InvincibleTime = EditorGUILayout.FloatField( "無敵時間", En.InvincibleTime);
			En.KnockBackRange = EditorGUILayout.FloatField( "ノックバック距離", En.KnockBackRange);
			En.TargetRange = EditorGUILayout.IntField( "プレイヤー探索範囲", En.TargetRange);
			En.EnemySpeed  = EditorGUILayout.FloatField( "移動スピード", En.EnemySpeed );
			En.EnemyRotate = EditorGUILayout.FloatField( "振り向き速度", En.EnemyRotate);
			En.Search = EditorGUILayout.FloatField( "プレイヤーとの間合い", En.Search);
		}
	}*/

	public void Initialize () {

	}

	void Start () {
		BossLifeBar.SetActive (true);
		DamageSet = false;
		armorPoint = armorPointMax;
		// Animator取得
		animator = GetComponent< Animator >();		
		// 被ダメージ時の点滅処理（ModelColorChange参照）
		modelColorChange = gameObject.GetComponent<ModelColorChange>();　
		// Playerタグが付いているオブジェクトをターゲットにする
		target = GameObject.FindWithTag ("Player");	
		// Playerタグが付いているオブジェクトのPlayerLevelをplayerLevelと呼ぶ
		playerLevel = GameObject.FindWithTag ("Player").GetComponent<PlayerLevel> ();
		// BattleManagerオブジェクトのBattleManagerをbattleManagerと呼ぶ
		battleManager = GameObject.Find ("BattleManager").GetComponent<BattleManager> ();
		// レイヤーをEnemyにしておく（死亡処理時使用）
		gameObject.layer = LayerMask.NameToLayer("Enemy");
		// Rigidbodyを取得し、以後rbと略す
		rb = this.GetComponent<Rigidbody>();
		// 重力を個別に設定する場合場合はデフォルト設定時のGravity設定を無効にする
		rb.useGravity = false;
	}


	void Update () {
		float PerArmorpoint = armorPoint / armorPointMax;
		if( PerArmorpoint < 0.8f) {
			gameObject.transform.localScale = new Vector3(
				gameObject.transform.localScale.x * Mscale,
				gameObject.transform.localScale.x * Mscale,
				gameObject.transform.localScale.x * Mscale
			);
		} else if( PerArmorpoint < 0.6f) {
			gameObject.transform.localScale = new Vector3(
				gameObject.transform.localScale.x * Sscale,
				gameObject.transform.localScale.x * Sscale,
				gameObject.transform.localScale.x * Sscale
			);
		}
	}

	// ショット衝突判定
	/*void OnTriggerEnter(Collider collider) {
		if (animator.GetBool ("dead") == true) {
			return;
		}
		// Shotタグが付いているオブジェクトに当たったら
		if (collider.gameObject.tag == "Shot") {
			// Bullet01スクリプトのdamageを受け取る
			damage = collider.gameObject.GetComponent<Bullet01> ().damage;
			// 当たり判定用のHit01ObjectをHit01Prefabにし生成
			Hit01Object = Instantiate (Hit01Prefab, EffectPoint.position, Quaternion.identity);
			// ダメージコルーチン（下記参照）
			StartCoroutine ("DamageCoroutine");
			// Shot接触時敵Animatorを"damaged"へ移行
			// アニメーションした後元に戻すのならSetTriggerの方が単純で良い
			animator.SetTrigger ("damaged");
			// 敵アーマーポイントからBullet01スクリプトのdamage値を差し引く
			armorPoint -= damage;
			BossLifeBar.GetComponent<BossLifeBar> ().UpdateArmorPointValue ();
		} else if (collider.gameObject.tag == "Shot2") {
			damage = collider.gameObject.GetComponent<Bullet02> ().damage;
			Hit02Object = Instantiate (Hit01Prefab, EffectPoint.position, Quaternion.identity);
			StartCoroutine ("DamageCoroutine");
			animator.SetTrigger ("damaged");
			armorPoint -= damage;
			BossLifeBar.GetComponent<BossLifeBar> ().UpdateArmorPointValue ();
		} else if (collider.gameObject.tag == "Shot3") {
			damage = collider.gameObject.GetComponent<Bullet03> ().damage;
			Hit03Object = Instantiate (Hit01Prefab, EffectPoint.position, Quaternion.identity);
			StartCoroutine ("DamageCoroutine");
			animator.SetTrigger ("damaged");
			armorPoint -= damage;
			BossLifeBar.GetComponent<BossLifeBar> ().UpdateArmorPointValue ();
		} else if (collider.gameObject.tag == "Shot5") {
			damage = collider.gameObject.GetComponent<Bullet05> ().damage;
			Hit05Object = Instantiate (Hit01Prefab, EffectPoint.position, Quaternion.identity);
			StartCoroutine ("DamageCoroutine");
			animator.SetTrigger ("damaged");
			armorPoint -= damage;
			BossLifeBar.GetComponent<BossLifeBar> ().UpdateArmorPointValue ();
		}

		//体力が0以下になったら消滅する
		if (collider.gameObject.tag == "Shot" || collider.gameObject.tag == "Shot2" || collider.gameObject.tag == "Shot3"
			|| collider.gameObject.tag == "Shot5") {
			if (armorPoint <= 0) {
				//Debug.Log ("敵"+gameObject.name);
				// Animatorを"dead"へ移行
				// 移行後元に戻さないならBool判定にした方がよい
				animator.SetBool ("dead", true);
				// 死亡アニメーション中に敵が移動しないようにスピードをゼロにする
				EnemySpeed = 0;
				// 敵消滅用エフェクト発生
				// 敵消滅中にプレイヤに接触ダメージがを与えないようにDeadCoroutineで接触判定を無くす
				DeadObject = Instantiate (Hit01Prefab, EffectPoint.position, Quaternion.identity);
				StartCoroutine ("DeadCoroutine");
				Instantiate (DestroyEffect, transform.position, transform.rotation);
				// バトルマネージャーにスコア（EnemyScoreで設定）を加算する
				battleManager = GameObject.Find ("BattleManager").GetComponent<BattleManager> ();
				// プレイヤのレベルアップ判定(PlayerLevel参照)
				// レベルに関係している数値はDataManagedrで管理している
				DataManager.Score += EnemyScore;
				playerLevel.LevelUp ();
				// 敵消滅
				Destroy (gameObject, DestroyTime);	
				// ボス、ラスボス消滅後は必ずクリア用スターを出現させる
				// インスペクタ画面でIsBoss、IsLastBossに✔を付ける
				if (isBoss == true) {
					Instantiate (Star, transform.position, transform.rotation);
				}
				if (isLastBoss == true) {
					Instantiate (BigStar, transform.position, transform.rotation);
					// ボス、ラスボス以外が消滅後は一定確率（0,RedEncountでRedEncount分の1）でアイテム出現
				}
			}
		}
	}*/

	// Itweenを使ってコルーチン作成（Itweenインストール必要あり）
	// ダメージ時の点滅処理
	IEnumerator DamageCoroutine ()
	{
		//while文を10回ループ
		int count = 10;
		iTween.MoveTo(gameObject, iTween.Hash(
			// その場からKnockBackRange数値分後(-transform.forwardで後)に移動
			"position", transform.position - (transform.forward * KnockBackRange),
			// 無敵(ダメージ判定なし)時間設定（秒）
			"time", InvincibleTime, 
			"easetype", iTween.EaseType.linear
		));
		while (count > 0){
			//透明にする(ModelColorChange参照)
			modelColorChange.ColorChange(new Color (1,0,0,1));
			//0.05秒待つ
			yield return new WaitForSeconds(0.1f);
			//元に戻す
			modelColorChange.ColorChange(new Color (1,1,1,1));
			//0.05秒待つ
			yield return new WaitForSeconds(0.1f);
			count--;
		}
	}

	// 死亡時処理
	IEnumerator DeadCoroutine ()
	{
		//レイヤーをInvincibleに変更して死亡処理時にプレイヤと接触しないようにする。
		// Edit→ProjectSetting→Tags and LayersでInvicibleを追加
		// Edit→ProjectSetting→Physicsで衝突させたくない対象(Player)と交差している所の✔を外
		gameObject.layer = LayerMask.NameToLayer("Invincible");
		//while文を10回ループ
		int count = 10;
		iTween.MoveTo(gameObject, iTween.Hash(
			// その場からKnockBackRange数値分後(-transform.forwardで後)に移動
			//"position", transform.position - (transform.forward * KnockBackRange),
			// 無敵(ダメージ判定なし)時間設定（秒）
			"time", InvincibleTime, 
			"easetype", iTween.EaseType.linear
		));
		while (count > 0){
			//透明にする(ModelColorChange参照)
			modelColorChange.ColorChange(new Color (1,0,0,1));
			//0.05秒待つ
			yield return new WaitForSeconds(0.1f);
			//元に戻す
			modelColorChange.ColorChange(new Color (1,1,1,1));
			//0.05秒待つ
			yield return new WaitForSeconds(0.1f);
			count--;
		}
	}

	// 重力設定を個別で設定
	// 常に一定の割合で処理を続ける場合はFixedUpdateを使う。操作時などの場合はUpdateの方がよい
	void FixedUpdate () {
		setLocalGravity ();
	}

	// インスペクタのlocalGravityに数値を設定（デフォルト通りならYを-9.8にする）
	void setLocalGravity(){
		rb.AddForce (localGravity, ForceMode.Acceleration);
	}

	public void Setarmorpoint(float armorPoint) {
		this.armorPoint = armorPoint;
		BossLifeBar.GetComponent<BossLifeBar>().UpdateArmorPointValue ();
		if (armorPoint <= 0) {
			BossLifeBar.GetComponent<BossLifeBar>().SetDisable ();
		}
	}

	public float GetarmorPoint() {
		return armorPoint;
	}

	public float GetarmorPointMax() {
		return armorPointMax;
	}
}

