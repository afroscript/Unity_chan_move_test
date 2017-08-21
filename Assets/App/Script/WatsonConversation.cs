using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IBM.Watson.DeveloperCloud.Services.TextToSpeech.v1;
using IBM.Watson.DeveloperCloud.Services.SpeechToText.v1;

public class WatsonConversation : MonoBehaviour {


	[SerializeField]
	SpeechToText m_SpeechToText = new SpeechToText();
	TextToSpeech m_TextToSpeech = new TextToSpeech();
	string m_ResString = "おはよう";
	private Animator animator;

	private const string key_isGoodMorning = "isGoodMorning";

	// Use this for initialization
	IEnumerator Start() {

		this.animator = GetComponent<Animator>();
		var audioSource = GetComponent<AudioSource>();
		var n = 3;
		while (n > 0) {
			Debug.Log (n + "回目");
			yield return RecMic(audioSource);
			n--;
		}
		yield return null;
	}

	IEnumerator RecMic(AudioSource audioSource) { // 音声をマイクから4秒間取得
		Debug.Log ("Start record");
		audioSource.clip = Microphone.Start(null, true, 10, 44100);
		audioSource.loop = false;
		audioSource.spatialBlend = 0.0f;
		yield return new WaitForSeconds (4f);
		Microphone.End (null);
		Debug.Log ("Finish record");

		//audioSource.Play ();

		// 音声の認識言語を日本語に指定
		m_SpeechToText.RecognizeModel = "ja-JP_BroadbandModel";
		// 音声をテキストに変換し、関数：HandleOnRecognize()を呼ぶ
		m_SpeechToText.Recognize(audioSource.clip, HandleOnRecognize);

	}

	void HandleOnRecognize(SpeechRecognitionEvent result){
		if (result != null && result.results.Length > 0) {
			foreach (var res in result.results) {
				foreach (var alt in res.alternatives) {
					string text = alt.transcript;
					Debug.Log (string.Format ("{0} ({1}, {2:0.00})\n", text, res.final ? "Final" : "Interim", alt.confidence));

					//textに"おはよう"があれば、おはようと返すしてしゃべる
					if (text.Contains ("おはよう")) {
						m_TextToSpeech.Voice = VoiceType.ja_JP_Emi; //音声タイプを指定
						m_TextToSpeech.ToSpeech (m_ResString, HandleToSpeechCallback);
						//ここにモーションを入れてる
						this.animator.SetBool (key_isGoodMorning, true);

					} else {
//						this.animator.SetBool(key_isGoodMorning, false);
					}
				}
			}
		} else {
			Debug.Log ("何も聞き取ってくれないときもある");
		}
	}

	void HandleToSpeechCallback (AudioClip clip) {
		PlayClip(clip);
	}

	private void PlayClip(AudioClip clip) {
		if (Application.isPlaying && clip != null) {
			GameObject audioObject = new GameObject("AudioObject");
			AudioSource source = audioObject.AddComponent<AudioSource>();
			source.spatialBlend = 0.0f;
			source.loop = false;
			source.clip = clip;
			source.Play();

			GameObject.Destroy(audioObject, clip.length);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
