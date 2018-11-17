
using UnityEngine;
using System.Collections;

//オブジェクト自動消滅
public class DestroySelf : MonoBehaviour
{
    public float Delay = 3f;	//消滅までの時間

    void Start ()
    {
		//Delay値後にオブジェクト消滅
        Destroy (gameObject, Delay);
    }
}
