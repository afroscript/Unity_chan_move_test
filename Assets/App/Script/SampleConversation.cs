﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IBM.Watson.DeveloperCloud.Services.Conversation.v1;

public class SampleConversation : MonoBehaviour {

	private Conversation m_Conversation = new Conversation();
	private string m_WorkspaceID = "268ca9d8-7287-440a-9259-836c1aa0c1a0";
	private string m_Input = "空腹だ";

	// Use this for initialization
	void Start () {
		Debug.Log("User: " + m_Input);
		m_Conversation.Message(OnMessage, m_WorkspaceID, m_Input);
	}

	void OnMessage (MessageResponse resp, string customData)
	{
		if (resp != null) {
			foreach (Intent mi in resp.intents)
				Debug.Log ("intent: " + mi.intent + ", confidence: " + mi.confidence);

			Debug.Log ("response: " + resp.output.text[0]);
		}
	}

	
	// Update is called once per frame
	void Update () {
		
	}
}
