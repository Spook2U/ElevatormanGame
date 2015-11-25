using UnityEngine;
using System.Collections;

public class ImageAreaController : MonoBehaviour {

	XMLController xml;

	int GameId = 0;
	string[] questions;

	void Awake() 
	{
		xml = GameObject.FindObjectOfType<XMLController>();
		questions = xml.GetGame_Questions(GameId, PlayerPrefs.GetInt("language"));
	}
	
	void Update () 
	{

	}
}
