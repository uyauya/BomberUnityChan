
using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public class Bomb : MonoBehaviour
{
    public AudioClip explosionSound;	//爆発音
    public GameObject explosionPrefab;	//爆発オブジェクト格納場所
    public LayerMask levelMask;
    // This LayerMask makes sure the rays cast to check for free spaces only hits the blocks in the level
    private bool exploded = false;		//爆発してるかどうかの判定
	public float ExPlosionTime = 3f;	//爆発までの時間

    // Use this for initialization
    void Start ()
    {
		//ExPlosionTime値後にExplode処理(下記参照)
		Invoke ("Explode", ExPlosionTime); 
    }

    void Explode ()
    {
        //Explosion sound
        AudioSource.PlayClipAtPoint (explosionSound, transform.position);

        //Create a first explosion at the bomb position
        Instantiate (explosionPrefab, transform.position, Quaternion.identity);

        //For every direction, start a chain of explosions
        StartCoroutine (CreateExplosions (Vector3.forward));
        StartCoroutine (CreateExplosions (Vector3.right));
        StartCoroutine (CreateExplosions (Vector3.back));
        StartCoroutine (CreateExplosions (Vector3.left));

        GetComponent<MeshRenderer> ().enabled = false; //Disable mesh
        exploded = true; 
        transform.Find ("Collider").gameObject.SetActive (false); //Disable the collider
        Destroy (gameObject, .3f); //0.3秒後消滅
    }

    public void OnTriggerEnter (Collider other)
    {
        if (!exploded && other.CompareTag ("Explosion"))
        { //If not exploded yet and this bomb is hit by an explosion...
            CancelInvoke ("Explode"); //Cancel the already called Explode, else the bomb might explode twice 
            Explode (); //Finally, explode!
        }
    }

    private IEnumerator CreateExplosions (Vector3 direction)
    {
        for (int i = 1; i < 3; i++)
        { //The 3 here dictates how far the raycasts will check, in this case 3 tiles far
            RaycastHit hit; //Holds all information about what the raycast hits

            Physics.Raycast (transform.position + new Vector3 (0, .5f, 0), direction, out hit, i, levelMask); //Raycast in the specified direction at i distance, because of the layer mask it'll only hit blocks, not players or bombs

            if (!hit.collider)
            { // Free space, make a new explosion
                Instantiate (explosionPrefab, transform.position + (i * direction), explosionPrefab.transform.rotation);
            } else
            { //Hit a block, stop spawning in this direction
                break;
            }

            yield return new WaitForSeconds (.05f); //Wait 50 milliseconds before checking the next location
        }

    }
}
