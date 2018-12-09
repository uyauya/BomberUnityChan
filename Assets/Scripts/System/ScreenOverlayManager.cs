using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;		//ImageEffectsを使う際に必要

public class ScreenOverlayManager : MonoBehaviour {

	ScreenOverlay screenOverlay;
	// インスペクターでScreenOverlayのBlendModeをAdditiveにしてIntensity（発光）の値を０にする
	public static float intensity;		// 発光調節

	void Start () {
		screenOverlay = GetComponent<ScreenOverlay> ();
		intensity = 0;	// 明るさの度合
	}

	void Update () {
		// 最初にフラッシュ
		screenOverlay.intensity = intensity;
		// 徐々に収束
		intensity -= Time.deltaTime;
		// 明るさの限度（０～１まで）
		intensity = Mathf.Clamp (intensity, 0, 1.0F);
	}
}
