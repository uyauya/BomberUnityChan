using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// スライダーと連動させるための処理
[RequireComponent(typeof(Slider))]

//キャラクタ声管理用
public class SoundManagerEnemy : SingletonMonoBehaviour<SoundManagerEnemy> {

	[SerializeField]
	private List<AudioSource> _effectAudioSource;
	[SerializeField]
	private List<AudioClip> audioClipList = new List<AudioClip>(); 
	[SerializeField]
	GameObject effect;
	private float Value;

	// 音を再生する
	public void Play(int number, GameObject go = null)
	{
		//Debug.Log("要素数" + audioClipList.Count);
		AudioClip clip = audioClipList[number];

		if (go != null)
		{
			StartCoroutine(PlayCoroutine(clip, go));
		}
		else
		{
			StartCoroutine(PlayCoroutine(clip, gameObject));
		}
	}

	// 音を遅延再生する
	public void PlayDelayed(int number, float delay, GameObject go = null)
	{
		AudioClip clip = audioClipList[number];

		if (go != null)
		{
			StartCoroutine(PlayDelayedCoroutine(clip, delay, go));
		}
		else
		{
			StartCoroutine(PlayDelayedCoroutine(clip, delay, gameObject));
		}

	}

	// 音を再生する コルーチン
	private IEnumerator PlayCoroutine(AudioClip clip, GameObject go)
	{
		AudioSource audioSource = go.AddComponent<AudioSource>();
		audioSource.volume = Value;
		audioSource.PlayOneShot(clip);
		_effectAudioSource.Add (audioSource);
		yield return new WaitWhile(() => audioSource.isPlaying);
		_effectAudioSource.Remove (audioSource);
		Destroy(audioSource);
	}

	// 音を遅延再生する コルーチン
	private IEnumerator PlayDelayedCoroutine(AudioClip clip, float delay, GameObject go)
	{
		AudioSource audioSource = go.AddComponent<AudioSource>();
		audioSource.clip = clip;
		audioSource.volume = Value;
		audioSource.PlayDelayed(delay);
		_effectAudioSource.Add (audioSource);
		yield return new WaitWhile(() => audioSource.isPlaying);
		_effectAudioSource.Remove (audioSource);
		Destroy(audioSource);
	}

	// 音を停止する
	public void Stop(int number, GameObject go = null)
	{
		//Debug.Log("要素数" + audioClipList.Count);
		AudioClip clip = audioClipList[number];

		if (go != null)
		{
			//StartCoroutine(PlayCoroutine(clip, go));
			go.GetComponent<AudioSource>().Stop();
		}
		else
		{
			//StartCoroutine(PlayCoroutine(clip, gameObject));
			gameObject.GetComponent<AudioSource>().Stop();
		}
	}

	private void Start()
	{
		effect.GetComponent<Slider> ().value = Volume ();
		effect.GetComponent<Slider>().onValueChanged.AddListener((value) =>
			{
				Value = value;
				foreach(var SoundSource in _effectAudioSource)
				{
					SoundSource.volume = value;
				}
			});
	}

	private void Update()
	{

	}

	public void Play()
	{
		AudioSource Effectplay = gameObject.GetComponent<AudioSource> ();
		Effectplay.Play ();
	}

	public void Stop()
	{
		AudioSource Effectstop = gameObject.GetComponent<AudioSource> ();
		Effectstop.Stop ();
	}

	public void Pause()
	{
		AudioSource Effectpause = gameObject.GetComponent<AudioSource> ();
		Effectpause.Pause ();
	}

	public float Volume()
	{
		AudioSource Effectvolume = gameObject.GetComponent<AudioSource> ();
		return Effectvolume.volume;
	}

	public void ChangeVolume()
	{
		Value = effect.GetComponent<Slider>().value;
		foreach(var SoundSource in _effectAudioSource)
		{
			SoundSource.volume = Value;
		}
	}

	public void SampleEffect()
	{
		Play (0);
	}
}

