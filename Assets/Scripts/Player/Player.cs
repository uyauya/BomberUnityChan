using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class Player : MonoBehaviour
{
	public BattleManager battleManager;
	private ModelColorChange modelColorChange;	
    [Range (1, 2)] //Enables a nifty slider in the editor
    public int PlayerNumber = 1;		//プレイヤナンバー判別用2
    public float MoveSpeed = 5f;		//移動速度
    public bool CanDropBombs = true;	//爆弾置ける状態かどうか
	public bool CanDropIceBlock = true;	//氷塊置ける状態かどうか
    public bool CanMove = true;			//移動できるかどうか
    public bool Dead = false;			//死亡判別用
    public GameObject BombPrefab;		//爆弾格納用
	public GameObject IceBlockPrefab;	//爆弾格納用
    private Rigidbody rigidBody;
    private Transform MyTransform;		//プレイヤーの向き
    private Animator animator;
	public float Mp;					//マジックポイント
	public float DisplayMp;				//マジックポイント（画面表示用）
	public int MpDown = 20;				//マジックポイント消費値
	public float MpUp = 0.2f;			//マジックポイント回復値
	private Slider MpSlider;			//マジックポイントゲージ（画面表示用）
	public Text MpText;					//マジックポイント最大・現在数値（画面表示用）
	public float Hp;					//体力
	public float DisplayHp;				//体力（画面表示用）
	private Slider HpSlider;			//体力ゲージ（画面表示用）
	public Text HpText;					//体力最大・現在数値（画面表示用）
	public Color MyGreen;				//体力上
	public Color MyYellow;				//体力中
	public Color MyRed;					//体力下
	public Color DamageColor;			//ダメージ点滅色
	public Color InvisibleColor;		//無敵状態時色
	public Color PoisonColor;			//毒ダメージ色
	public int GetStar = 0;				//スター獲得判定用
	public float FlashTime;				//点滅時間
	public float KnockBackRange;		//ノックバック距離（ダメージ受けた際に使用）

    // Use this for initialization
    void Start ()
    {
        rigidBody = GetComponent<Rigidbody> ();
        MyTransform = transform;
        animator = MyTransform.Find ("PlayerModel").GetComponent<Animator> ();
		gameObject.layer = LayerMask.NameToLayer("Player");
		modelColorChange = gameObject.GetComponent<ModelColorChange>();
		Mp 	   = DataManager.MpMax;												// Mpを最大値に設定
		MpSlider = transform.Find ("Slider").GetComponent <Slider>();
		MpText = GameObject.Find ("TextMp").GetComponent<Text> ();
		Hp 	   = DataManager.HpMax;												// Hpを最大値に設定
		HpSlider = transform.Find ("Slider").GetComponent <Slider>();
		HpText = GameObject.Find ("TextHp").GetComponent<Text> ();
    }

    // Update is called once per frame
    void Update ()
    {
		//移動処理（下記参照）
        UpdateMovement ();
    }

	//移動処理
    private void UpdateMovement ()
    {
		//
        animator.SetBool ("Walking", false); 
		//移動してなかったら何もしない（アニメーションしない）
        if (!CanMove)
        { 
            return;
        }

		//各プレイヤーに応じてPlayer1Movement処理(下記参照)
        if (PlayerNumber == 1)
        {
            Player1Movement ();
		} if (PlayerNumber == 2)
        {
            Player2Movement ();
        }
    }
		
	//1プレイヤー動作
    private void Player1Movement ()
    {
		//上移動
        if (Input.GetKey (KeyCode.W))
        {
			//Z方向(上)にMoveSpeedで移動
            rigidBody.velocity = new Vector3 (rigidBody.velocity.x, rigidBody.velocity.y, MoveSpeed);
			//プレイヤー方向変えない（デフォルトで上向き）
            MyTransform.rotation = Quaternion.Euler (0, 0, 0);
			//animatorをWalkingに切り替え
            animator.SetBool ("Walking", true);
        }
		//左移動
        if (Input.GetKey (KeyCode.A))
        {
			//Xにマイナス方向(左)にMoveSpeedで移動
            rigidBody.velocity = new Vector3 (-MoveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
			//プレイヤー方向Y軸左向き(時計回りで270度で左向き)
            MyTransform.rotation = Quaternion.Euler (0, 270, 0);
            animator.SetBool ("Walking", true);
        }
		//下移動
        if (Input.GetKey (KeyCode.S))
        {
			//Zにマイナス方向(下)にMoveSpeedで移動
            rigidBody.velocity = new Vector3 (rigidBody.velocity.x, rigidBody.velocity.y, -MoveSpeed);
            MyTransform.rotation = Quaternion.Euler (0, 180, 0);
            animator.SetBool ("Walking", true);
        }
		//右移動
        if (Input.GetKey (KeyCode.D))
        { 
			//X方向(右)にMoveSpeedで移動
            rigidBody.velocity = new Vector3 (MoveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            MyTransform.rotation = Quaternion.Euler (0, 90, 0);
            animator.SetBool ("Walking", true);
        }
		//爆弾置き
		//爆弾置ける状態でSpaceキー押して爆弾配置
        if (CanDropBombs && Input.GetKeyDown (KeyCode.Space))
        { 
            DropBomb ();
        }
    }
		
	//2プレイヤー動作
    private void Player2Movement ()
    {
        if (Input.GetKey (KeyCode.UpArrow))
        { //Up movement
            rigidBody.velocity = new Vector3 (rigidBody.velocity.x, rigidBody.velocity.y, MoveSpeed);
            MyTransform.rotation = Quaternion.Euler (0, 0, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.LeftArrow))
        { //Left movement
            rigidBody.velocity = new Vector3 (-MoveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            MyTransform.rotation = Quaternion.Euler (0, 270, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.DownArrow))
        { //Down movement
            rigidBody.velocity = new Vector3 (rigidBody.velocity.x, rigidBody.velocity.y, -MoveSpeed);
            MyTransform.rotation = Quaternion.Euler (0, 180, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.RightArrow))
        { //Right movement
            rigidBody.velocity = new Vector3 (MoveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            MyTransform.rotation = Quaternion.Euler (0, 90, 0);
            animator.SetBool ("Walking", true);
        }

        if (CanDropBombs && (Input.GetKeyDown (KeyCode.KeypadEnter) || Input.GetKeyDown (KeyCode.Return)))
        { //Drop Bomb. For Player 2's bombs, allow both the numeric enter as the return key or players without a numpad will be unable to drop bombs
            DropBomb ();
        }
    }
		
	//爆弾生成
    private void DropBomb ()
    {
        if (BombPrefab)
        { 
            Instantiate (BombPrefab,
                new Vector3 (Mathf.RoundToInt (MyTransform.position.x), BombPrefab.transform.position.y, 
					Mathf.RoundToInt (MyTransform.position.z)),BombPrefab.transform.rotation);
        }
    }

    //衝突判定
	//何かと接触して
	public void OnTriggerEnter (Collider other)
    {
		//死亡してない状態で相手がExplosionのタグ付きだったら
        if (!Dead && other.CompareTag ("Explosion"))
        { //Not dead & hit by explosion
            Debug.Log ("P" + PlayerNumber + " hit by explosion!");
			//死亡判定をtrueにして
            Dead = true;
			//プレイヤナンバーを判別して
			battleManager.PlayerDied (PlayerNumber); //Notify global state manager that this player died
			//プレイヤ消滅
            Destroy (gameObject);
        }
    }

	// Itweenを使ってコルーチン作成（Itweenインストール必要あり）
	// 敵接触時の点滅（オブジェクトの色をStandardなどにしておかないと点滅しない場合がある）
	IEnumerator BombDamageCoroutine ()
	{
		// プレイヤのレイヤーをInvincibleに変更
		// Edit→ProjectSetting→Tags and LayersでInvicibleを追加
		// Edit→ProjectSetting→Physicsで衝突させたくない対象と交差している所の✔を外す
		// ここではBombと衝突させたくない（すり抜ける）為、Bombのレイヤーも追加
		// BombとPlayerの交差してる✔を外す（プレイヤのLayerをPlayer、BombのLayerをBombに設定しておく）
		gameObject.layer = LayerMask.NameToLayer("Invincible");
		//while文を10回ループ
		int count = 10;
		iTween.MoveTo(gameObject, iTween.Hash(
			//KnockBackRange値だけ後に吹っ飛ぶ
			"position", transform.position - (transform.forward * KnockBackRange),
			"time", FlashTime, // 点滅時間（秒）
			"easetype", iTween.EaseType.linear
		));
		while (count > 0){
			//透明にする
			//modelColorChange.ColorChange(new Color (1,0,0,1));
			modelColorChange.ColorChange(DamageColor);
			//0.1秒待つ
			yield return new WaitForSeconds(0.1f);
			//元に戻す
			modelColorChange.ColorChange(new Color (1,1,1,1));
			//0.1秒待つ
			yield return new WaitForSeconds(0.1f);
			count--;
		}
		//レイヤーをPlayerに戻す
		gameObject.layer = LayerMask.NameToLayer("Player");
	}
}
