using UnityEngine; 
using UnityEngine.UI; 
using System.Collections; 

// 時間経過表示用
public class LimitedTimer : MonoBehaviour { 
	private float time = 0.0f; 
	
	void Start () { 
		// 初期値を表示 
		// float型からint型へCastし、String型に変換して表示 
		// 60で割った値を分、余りを秒とする
		int minute = (int)time / 60;
		int second = (int)time % 60;
		// D2は十進法(Decimal)の２桁(00)で表示
		GetComponent<Text> ().text = minute.ToString("D2") + ":" + second.ToString("D2") + "";
	} 

	void Update (){ 
		int minute = (int)time / 60;
		int second = (int)time % 60;
		// 1秒に1ずつ増やしていく場合（減らす場合はtime -= Time.deltaTime;）
		time += Time.deltaTime;
		// 0未満だった0とする（マイナスは表示しない）
		if (time < 0) time = 0; 
		GetComponent<Text> ().text = minute.ToString("D2") + ":" + second.ToString("D2") + "";
	} 
} 