using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// スライダーと連動させるための処理
[RequireComponent(typeof(Slider))]

//BGM全般管理用
public class BGMManager : MonoBehaviour {

	public AudioSource SoundSource;
	private List<AudioSource> _bgmAudioSource;
	[SerializeField]
	GameObject bgm;
	// Use this for initialization
	void Start () 
	{
		bgm.GetComponent<Slider> ().value = Volume ();
		bgm.GetComponent<Slider>().onValueChanged.AddListener((value) =>
			{
				//foreach(var SoundSource in _bgmAudioSource)
				//{
					SoundSource.volume = value;
				//}
			});
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void Play()
	{
		AudioSource BGMplay = gameObject.GetComponent<AudioSource> ();
		BGMplay.Play ();
	}

	public void Stop()
	{
		AudioSource BGMstop = gameObject.GetComponent<AudioSource> ();
		BGMstop.Stop ();
	}

	public void Pause()
	{
		AudioSource BGMpause = gameObject.GetComponent<AudioSource> ();
		BGMpause.Pause ();
	}

	public float Volume()
	{
		AudioSource BGMvolume = gameObject.GetComponent<AudioSource> ();
		return BGMvolume.volume;
	}
}
