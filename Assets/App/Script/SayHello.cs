using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IBM.Watson.DeveloperCloud.Services.TextToSpeech.v1;
using IBM.Watson.DeveloperCloud.Services.SpeechToText.v1;

public class SayHello : MonoBehaviour {

	public static void aaa() {
		Debug.Log ("aaa");
	}

	[SerializeField]
	TextToSpeech m_TextToSpeech = new TextToSpeech();
	string m_ResString = "おはよう";
	private SpeechToText m_SpeechToText = new SpeechToText();

	IEnumerator Start() {
		// 音声をマイクから3秒間取得
		Debug.Log ("Start record");
		var audioSource = GetComponent<AudioSource>();
		audioSource.clip = Microphone.Start(null, true, 10, 44100);
		audioSource.loop = false;
		audioSource.spatialBlend = 0.0f;
		yield return new WaitForSeconds (4f);
		Microphone.End (null);
		Debug.Log ("Finish record");

		// ためしに録音内容を再生してみる
		audioSource.Play ();

		// 音声をテキストに変換
		m_SpeechToText.RecognizeModel = "ja-JP_BroadbandModel";
		m_SpeechToText.Recognize(audioSource.clip, HandleOnRecognize);

	}

	void HandleOnRecognize(SpeechRecognitionEvent result){
		if (result != null && result.results.Length > 0){
			foreach (var res in result.results){
				foreach (var alt in res.alternatives){
					string text = alt.transcript;
					Debug.Log(string.Format("{0} ({1}, {2:0.00})\n", text, res.final ? "Final" : "Interim", alt.confidence));

					//textに"おはよう"があれば、おはようと返すしてしゃべる
					if (text.Contains("おはよう")) {
						m_TextToSpeech.Voice = VoiceType.ja_JP_Emi; //音声タイプを指定
						m_TextToSpeech.ToSpeech(m_ResString, HandleToSpeechCallback);

					}
				}
			}
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
	//	void Update () {
	//		
	//	}
}
