using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor; 

// 敵キャラクタ管理用
// ステータス欄等の共通項目を保持。キャラクタ独自の項目のみ別スクリプトに書き足す。
public class EnemyBasic : MonoBehaviour {
	private LifeBar lifeBar;
	public int enemyLevel = 0;
	public Animator animator;						// Animatorセット用
	public GameObject target;						// プレイヤー認識用
	public GameObject shot;
	public float shotInterval = 0;					// 攻撃間隔計測開始
	public float shotIntervalMax = 1.0F;			// 攻撃間隔（～秒ごとに攻撃）
	public GameObject exprosion;					// 爆発処理
	public GameObject particle;
	public float armorPointMax = 100;				// 最大HP
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
	public GameObject RedSphere;					// アーマーポイント回復用玉（アイテムタグ3）
	public GameObject BlueSphere;					// ブーストポイント回復用玉（アイテムタグ2）
	public GameObject GreenSphere;					// ボス面移行用玉（アイテムタグ3）
	public GameObject YellowSphere;					// プレイヤ巨大＆無敵化
	public GameObject Star;							// ボス面クリア用スター
	public GameObject BigStar;						// ラスボス面クリア用スター
	public int RedEncount = 16;						// RedSphere生成率の分母
	public int BlueEncount = 8;
	public int GreenEncount= 32;
	public int YellowEncount= 32;
	public int bigAttack;
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
	public bool DamageSet;							// ダメージ判定の有無(処理内容は継承先に記載)
	public bool FreezeSet;							// フリーズ判定の有無
	public float Mscale = 1.0f;						// 縮小（第一段階）				
	public float Sscale = 1.0f;						// 縮小（第二段階）
	public GameObject LifeBar;						// 敵HP表示用（頭上に設置）
	public Color DamageColor;						// 被ダメージ時の点滅色
	public Color FreezeColor;
	public Color DeadColor;

	/*[CustomEditor(typeof(EnemyBasic))]
	public class EnemyBasicEditor : Editor	// using UnityEditor; を入れておく
	{
		bool folding = false;

		public override void OnInspectorGUI()
		{
			EnemyBasic En = target as EnemyBasic;
			En.shotIntervalMax = EditorGUILayout.FloatField( "ショット間隔", En.shotIntervalMax);
			En.armorPointMax   = EditorGUILayout.FloatField( "最大HP", En.armorPointMax);
			En.TargetRange	   = EditorGUILayout.FloatField( "ターゲットレンジ", En.TargetRange);
			En.EnemySpeed	   = EditorGUILayout.FloatField( "移動速度", EnEnemySpeed);
			En.JumpForce	   = EditorGUILayout.FloatField( "ジャンプ力", En.JumpForce);
			En.EnemyRotate     = EditorGUILayout.FloatField( "振り向き速度", En.EnemyRotate);
			En.Search          = EditorGUILayout.FloatField( "プレイヤーとの間合い", En.Search);
			En.EnemyAttack	   = EditorGUILayout.IntField("攻撃力", EnEnemyAttack);
			En.InvincibleTime  = EditorGUILayout.FloatField( "無敵時間", En.InvincibleTime);
			En.KnockBackRange  = EditorGUILayout.FloatField( "ノックバック距離", En.KnockBackRange);
			En.DestroyTime 	   = EditorGUILatout.FloatField( "消滅するまでの時間", En.DestroyTime);
			En.EnemyScore	   = EditorGUILayout.IntField( "スコア", En.EnemyScore);
			En.RedEncount      = EditorGUILayout.IntField( "赤玉生成率", En.RedEncount);
			En.BlueEncount	   = EditorGUILayout.IntField( "青玉生成率", En.BlueEncount);
			En.GreenEncount	   = EditorGUILayout.IntField( "緑玉生成率", En.GreenEncount);
			En.YellowEncount   = EditorGUILayout.IntField( "黄玉生成率", En.YellowEncount);
			En.Mscale		   = EditorGUILayout.FloatField( "縮小率第1段階", En.Mscale);
			En.Sscale		   = EditorGUILayout.FloatField( "縮小率第2段階", En.Sscale);
		}
	}*/

	public void Initialize () {
		
	}

	void Start () {
		//lifeBarをオブジェクトの子として取得、基本は非表示（被ダメージ時表示）
		lifeBar = GetComponentInChildren<LifeBar>();
		LifeBar.SetActive (false);
		//被ダメージ時にtrueに返す
		DamageSet = false;
		//敵HP初期値設定(最大)
		armorPoint = armorPointMax;
		// Animator取得
		animator = GetComponent< Animator >();		
		// 被ダメージ時の点滅処理（ModelColorChange参照）
		modelColorChange = gameObject.GetComponent<ModelColorChange>();　
		// Playerタグが付いているオブジェクトをターゲットにする
		target = GameObject.FindWithTag ("Player");	
		// Playerタグが付いているオブジェクトのPlayerLevelをplayerLevelとする
		playerLevel = GameObject.FindWithTag ("Player").GetComponent<PlayerLevel> ();
		// BattleManagerオブジェクトのBattleManagerをbattleManagerとする
		battleManager = GameObject.Find ("BattleManager").GetComponent<BattleManager> ();
		// レイヤーをEnemyにしておく（被ダメージ時、死亡処理時使用）
		gameObject.layer = LayerMask.NameToLayer("Enemy");
		// Rigidbodyを取得し、以後rbと略す
		rb = this.GetComponent<Rigidbody>();
		// 重力を個別に設定する場合場合はデフォルト設定時のGravity設定を無効にする
		rb.useGravity = false;
	}


	void Update () {
		//重力をカスタムする時用
		setLocalGravity ();
		//体力を最大体力で割った割合をPerArmorpointとする
		float PerArmorpoint = armorPoint / armorPointMax;
		// ArmoPointの減少によりキャラクタの大きさを変更
		// PerArmorpointの割合が0.8を下回ったなら縦、横、高さにMscale割合分縮小する
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
		// Animatorがdead(死亡)だったら判定しない(何もしない)
		if (animator.GetBool ("dead") == true) {
			return;
		}
		// Shotタグが付いているオブジェクトに当たったら
		if (collider.gameObject.tag == "Shot") {
			// 相手を硬直させる（下記DamageSet参照）
			DamageSet = true;
			// ライフバー表示（下記参照）
			StartCoroutine ("LifeBarCoroutine");
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
			//ライフバーからもダメージ分ゲージを減らす
			LifeBar.GetComponent<LifeBar> ().UpdateArmorPointValue ();
			//DamageSet = false;
		} else if (collider.gameObject.tag == "Shot2") {
			DamageSet = true;
			StartCoroutine ("LifeBarCoroutine");
			damage = collider.gameObject.GetComponent<Bullet02> ().damage;
			Hit02Object = Instantiate (Hit01Prefab, EffectPoint.position, Quaternion.identity);
			StartCoroutine ("DamageCoroutine");
			animator.SetTrigger ("damaged");
			armorPoint -= damage;
			LifeBar.GetComponent<LifeBar> ().UpdateArmorPointValue ();
		} else if (collider.gameObject.tag == "Shot3") {
			//Debug.Log (collider.gameObject.name);
			//DamageSet = true;
			FreezeSet = true;
			StartCoroutine ("LifeBarCoroutine");
			damage = collider.gameObject.GetComponent<Bullet03> ().damage;
			Hit03Object = Instantiate (Hit01Prefab, EffectPoint.position, Quaternion.identity);
			StartCoroutine ("DamageCoroutine");
			animator.SetTrigger ("damaged");
			armorPoint -= damage;
			LifeBar.GetComponent<LifeBar> ().UpdateArmorPointValue ();
		} else if (collider.gameObject.tag == "Shot5") {
			DamageSet = true;
			StartCoroutine ("LifeBarCoroutine");
			damage = collider.gameObject.GetComponent<Bullet05> ().damage;
			Hit05Object = Instantiate (Hit01Prefab, EffectPoint.position, Quaternion.identity);
			StartCoroutine ("DamageCoroutine");
			animator.SetTrigger ("damaged");
			armorPoint -= damage;
			LifeBar.GetComponent<LifeBar> ().UpdateArmorPointValue ();
		}

		//プレイヤーのいずれかの弾に当たって体力が0以下になったら消滅する
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
				// 敵消滅中にプレイヤに接触ダメージがを与えないようにDeadCoroutine(下記参照)で接触判定を無くす
				DeadObject = Instantiate (Hit01Prefab, EffectPoint.position, Quaternion.identity);
				StartCoroutine ("DeadCoroutine");
				Instantiate (DestroyEffect, transform.position, transform.rotation);
				// バトルマネージャーにスコア（EnemyScoreで設定）を加算する
				battleManager = GameObject.Find ("BattleManager").GetComponent<BattleManager> ();
				DataManager.Score += EnemyScore;
				// プレイヤのレベルアップ判定(PlayerLevel参照)
				// レベルに関係している数値はDataManagedrで管理している
				playerLevel.LevelUp ();
				// DestroyTime時間後敵消滅
				Destroy (gameObject, DestroyTime);	
				// ボス、ラスボス消滅後は必ずクリア用スターを出現させる
				// インスペクタ画面でIsBoss、IsLastBossに✔を付ける
				if (isBoss == true) {
					Instantiate (Star, transform.position, transform.rotation);
				}
				if (isLastBoss == true) {
					Instantiate (BigStar, transform.position, transform.rotation);
				// ボス、ラスボス以外が消滅後は一定確率（0,RedEncountでRedEncount分の1）でアイテム出現
				} else if (Random.Range (0, RedEncount) == 0) {
					Instantiate (RedSphere, transform.position, transform.rotation);
					//Vector3 Pog = this.gameObject.transform.position;
					//Instantiate (RedSphere, transform.position += new Vector3(0 , SphereHeight, 0), transform.rotation);
				} else if (Random.Range (0, BlueEncount) == 0) {
					Instantiate (BlueSphere, transform.position, transform.rotation);
					//Vector3 Pog = this.gameObject.transform.position;
					//Instantiate (RedSphere, transform.position += new Vector3(0 , SphereHeight, 0), transform.rotation);
				} else if (Random.Range (0, GreenEncount) == 0) {
					Instantiate (GreenSphere, transform.position, transform.rotation);
					//Vector3 Pog = this.gameObject.transform.position;
					//Instantiate (RedSphere, transform.position += new Vector3(0 , SphereHeight, 0), transform.rotation);
				} else if (Random.Range (0, YellowEncount) == 0) {
					Instantiate (YellowSphere, transform.position, transform.rotation);
					//Vector3 Pog = this.gameObject.transform.position;
					//Instantiate (RedSphere, transform.position += new Vector3(0 , SphereHeight, 0), transform.rotation);
				}
			}
		}
	}*/
		
	//巨大化したプレイヤに接触したらダメージ
	void OnCollisionEnter(Collision collider) {
		//PlayerApのisBig(巨大化)状態をisbigとする
		bool isbig = GameObject.FindWithTag ("Player").GetComponent<PlayerAp> ().isBig;
		//プレイヤのタグがついていてisbig状態(巨大化中)なら
		if (collider.gameObject.tag == "Player" && isbig == true) {
			//Debug.Log ("一撃");
			//armorPointからプレイヤのbigAttack値を差し引く
			bigAttack = GameObject.FindWithTag ("Player").GetComponent<PlayerAp> ().BigAttack;
			armorPoint -= bigAttack;
			//それ以外(巨大化してない)ならDamageSet状態にする
		} else if (collider.gameObject.tag == "Player") {
			DamageSet = true;
			animator.SetTrigger ("damaged");
		}
			//ライフバーからダメージ分ゲージを減らす
			LifeBar.GetComponent<LifeBar>().UpdateArmorPointValue();

		//
		if (collider.gameObject.tag == "Player" ) {
			//体力が0以下になったら消滅する
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
				//Instantiate(exprosion, transform.position, transform.rotation);
				// ボス、ラスボス消滅後は必ずクリア用スターを出現させる
				// インスペクタ画面でIsBoss、IsLastBossに✔を付ける
				if (isBoss == true) {
					Instantiate (Star, transform.position, transform.rotation);
				}
				if (isLastBoss == true) {
					Instantiate (BigStar, transform.position, transform.rotation);
					// ボス、ラスボス以外が消滅後は一定確率（0,RedEncountでRedEncount分の1）でアイテム出現
				} else if (Random.Range (0, RedEncount) == 0) {
					Instantiate (RedSphere, transform.position, transform.rotation);
				} else if (Random.Range (0, BlueEncount) == 0) {
					Instantiate (BlueSphere, transform.position, transform.rotation);
				} else if (Random.Range (0, GreenEncount) == 0) {
					Instantiate (GreenSphere, transform.position, transform.rotation);
				} else if (Random.Range (0, YellowEncount) == 0) {
					Instantiate (YellowSphere, transform.position, transform.rotation);
				}
			}
		}

	}

	// Itweenを使ってコルーチン作成（Itweenインストール必要あり）
	// ダメージ時の点滅処理(DamageColor色で点滅)
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
			//modelColorChange.ColorChange(new Color (1,0,0,1));
			modelColorChange.ColorChange(DamageColor);
			//0.03秒待つ
			yield return new WaitForSeconds(0.3f);
			//元に戻す
			modelColorChange.ColorChange(new Color (1,1,1,1));
			//0.03月秒待つ
			yield return new WaitForSeconds(0.3f);
			count--;
		}
	}

	// フリーズ時の点滅処理
	IEnumerator FreezeCoroutine ()
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
		// 無敵(ダメージ判定なし)にして
		while (count > 0){
			//透明にする(ModelColorChange参照)
			modelColorChange.ColorChange(FreezeColor);
			//0.03秒待つ
			yield return new WaitForSeconds(0.1f);
			//元に戻す
			modelColorChange.ColorChange(new Color (1,1,1,1));
			//0.03秒待つ
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
			// 無敵(ダメージ判定なし)時間設定（秒）
			"time", InvincibleTime, 
			"easetype", iTween.EaseType.linear
		));
		while (count > 0){
			//透明にする(ModelColorChange参照)
			modelColorChange.ColorChange(DeadColor);
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

	//HPが0になったらLifeBar非表示
	public void Setarmorpoint(float armorPoint) {
		this.armorPoint = armorPoint;
		lifeBar.UpdateArmorPointValue ();
		if (armorPoint <= 0) {
			lifeBar.SetDisable ();
		}
	}

	//LifeBarのHPとの連動用
	public float GetarmorPoint() {
		return armorPoint;
	}

	//LifeBarの最大HPとの連動用
	public float GetarmorPointMax() {
		return armorPointMax;
	}

	//攻撃が当たった時WaitForSeconds数値分だけLifeBarを表示
	IEnumerator LifeBarCoroutine (){
		LifeBar.SetActive (true);
		yield return new WaitForSeconds (0.5f);
		LifeBar.SetActive (false);
	}
}
