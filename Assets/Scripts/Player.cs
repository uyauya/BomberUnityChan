
using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour
{
    //Manager
    //public GlobalStateManager globalManager;
	public BattleManager battleManager;
    //Player parameters
    [Range (1, 2)] //Enables a nifty slider in the editor
    public int playerNumber = 1;		//プレイヤナンバー判別用2
    public float moveSpeed = 5f;		//移動速度
    public bool canDropBombs = true;	//爆弾置ける状態かどうか
	public bool canDropIceBlock = true;	//氷塊置ける状態かどうか
    public bool canMove = true;			//移動できるかどうか
    public bool dead = false;			//死亡判別用


    //private int bombs = 2;
    //Amount of bombs the player has left to drop, gets decreased as the player drops a bomb, increases as an owned bomb explodes

    //Prefabs
    public GameObject bombPrefab;		//爆弾格納用
	public GameObject IceBlockPrefab;	//爆弾格納用
    //Cached components
    private Rigidbody rigidBody;
    private Transform myTransform;
    private Animator animator;

    // Use this for initialization
    void Start ()
    {
        //Cache the attached components for better performance and less typing
        rigidBody = GetComponent<Rigidbody> ();
        myTransform = transform;
        animator = myTransform.Find ("PlayerModel").GetComponent<Animator> ();
		//animator = myTransform.Find ("Model").GetComponent<Animator> ();
    }

    // Update is called once per frame
    void Update ()
    {
        UpdateMovement ();
    }

	//移動処理
    private void UpdateMovement ()
    {
		//
        animator.SetBool ("Walking", false); //Resets walking animation to idle
		//移動してなかったら何もしない（アニメーションしない）
        if (!canMove)
        { //Return if player can't move
            return;
        }

		//各プレイヤーに応じてPlayer1Movement処理(下記参照)
        if (playerNumber == 1)
        {
            Player1Movement ();
		} if (playerNumber == 2)
        {
            Player2Movement ();
        }
    }

    /// <summary>
    /// Updates Player 1's movement and facing rotation using the WASD keys and drops bombs using Space
    /// </summary>
	//1プレイヤー動作
    private void Player1Movement ()
    {
        if (Input.GetKey (KeyCode.W))
        { //Up movement
            rigidBody.velocity = new Vector3 (rigidBody.velocity.x, rigidBody.velocity.y, moveSpeed);
            myTransform.rotation = Quaternion.Euler (0, 0, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.A))
        { //Left movement
            rigidBody.velocity = new Vector3 (-moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler (0, 270, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.S))
        { //Down movement
            rigidBody.velocity = new Vector3 (rigidBody.velocity.x, rigidBody.velocity.y, -moveSpeed);
            myTransform.rotation = Quaternion.Euler (0, 180, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.D))
        { //Right movement
            rigidBody.velocity = new Vector3 (moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler (0, 90, 0);
            animator.SetBool ("Walking", true);
        }

        if (canDropBombs && Input.GetKeyDown (KeyCode.Space))
        { //Drop bomb
            DropBomb ();
        }
    }

    /// <summary>
    /// Updates Player 2's movement and facing rotation using the arrow keys and drops bombs using Enter or Return
    /// </summary>
	//2プレイヤー動作
    private void Player2Movement ()
    {
        if (Input.GetKey (KeyCode.UpArrow))
        { //Up movement
            rigidBody.velocity = new Vector3 (rigidBody.velocity.x, rigidBody.velocity.y, moveSpeed);
            myTransform.rotation = Quaternion.Euler (0, 0, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.LeftArrow))
        { //Left movement
            rigidBody.velocity = new Vector3 (-moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler (0, 270, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.DownArrow))
        { //Down movement
            rigidBody.velocity = new Vector3 (rigidBody.velocity.x, rigidBody.velocity.y, -moveSpeed);
            myTransform.rotation = Quaternion.Euler (0, 180, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.RightArrow))
        { //Right movement
            rigidBody.velocity = new Vector3 (moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler (0, 90, 0);
            animator.SetBool ("Walking", true);
        }

        if (canDropBombs && (Input.GetKeyDown (KeyCode.KeypadEnter) || Input.GetKeyDown (KeyCode.Return)))
        { //Drop Bomb. For Player 2's bombs, allow both the numeric enter as the return key or players without a numpad will be unable to drop bombs
            DropBomb ();
        }
    }

    /// <summary>
    /// Drops a bomb beneath the player
    /// </summary>
	//爆弾生成
    private void DropBomb ()
    {
        if (bombPrefab)
        { //Check if bomb prefab is assigned first
            // Create new bomb and snap it to a tile
            Instantiate (bombPrefab,
                new Vector3 (Mathf.RoundToInt (myTransform.position.x), bombPrefab.transform.position.y, Mathf.RoundToInt (myTransform.position.z)),
                bombPrefab.transform.rotation);
        }
    }

    //衝突判定
	//何かと接触して
	public void OnTriggerEnter (Collider other)
    {
		//死亡してない状態で相手がExplosionのタグ付きだったら
        if (!dead && other.CompareTag ("Explosion"))
        { //Not dead & hit by explosion
            Debug.Log ("P" + playerNumber + " hit by explosion!");
			//死亡判定をtrueにして
            dead = true;
			//プレイヤナンバーを判別して
			battleManager.PlayerDied (playerNumber); //Notify global state manager that this player died
			//プレイヤ消滅
            Destroy (gameObject);
        }
    }
}
