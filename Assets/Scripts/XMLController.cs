using UnityEngine;
using System.Collections;
using System.Xml;

public class XMLController : MonoBehaviour 
{
	XmlDocument xmlDoc;

	XmlNodeList languages = null;
	XmlNodeList artworks;
	XmlNodeList games;

	XmlNodeList attributes;
	


	XmlNodeList dialog;

	private int index;
	private bool hasNext;
	private string sprecher;
	private string text;
	private string syncro;
	private string highlight;
	private string input;
	private string action;
	private string dict;
	private bool changeColor;

	public bool HasNext() 			{ return hasNext; }
	public string getSprecher() 	{ return sprecher; }
	public string getText() 		{ return text; }
	public string getAudioSource() 	{ return syncro; }
	public string getHighlight()	{ return highlight; }
	public string getInput()		{ return input; }
	public string getAction()		{ return action; }
	public string getdict()			{ return dict; }
	public bool   getChangeColor()  { return changeColor; }


	public void Awake()
	{
		LoadXml("database");

		getMinispielID(1);
	}



	public void LoadXml(string file)
	{
		xmlDoc = new XmlDocument();
		
		TextAsset xmlAsset = (TextAsset) Resources.Load(file);
		xmlDoc.LoadXml(xmlAsset.text);
		
		languages = xmlDoc.GetElementsByTagName("sprachen")  .Item(0).ChildNodes;
		artworks  = xmlDoc.GetElementsByTagName("kunstwerke").Item(0).ChildNodes;
		games     = xmlDoc.GetElementsByTagName("minispiele").Item(0).ChildNodes;
		
		//index = 0;
		//SetAttributes();
	}

	public string[] getLanguages()
	{	
		string[] array = null;

		if(languages != null)
		{
			array = new string[languages.Count];

			for(int i = 0; i < languages.Count; i++)
			{
				array[i] = languages.Item(i).InnerText;
			}
		}

		return array;
	}

	
	public int getMinispielID(int positionsID)
	{
		bool gefunden;
		int id = 0;
		XmlNodeList artwork;
			
		for(int i = 0; i < artworks.Count; i++)
		{
			artwork = artworks.Item(i).ChildNodes;

			for(int j = 0; j < artwork.Count; j++)
			{
				if(artwork.Item(j).Name == "position")
				{
					if(artwork.Item(j).InnerText == positionsID)
					{
						gefunden = true;
					}
				}
			}
		}

		return id;
	}




	public void Next()
	{
		index++;
		SetAttributes();
	}

	private void SetAttributes()
	{
		hasNext = index < dialog.Count;
		sprecher = "";
		text = "";
		syncro = "";
		highlight = "";
		input = "";
		action = "";
		dict = "";
		changeColor = false;

		if(hasNext)
		{
			attributes = dialog.Item(index).ChildNodes;
			foreach(XmlNode node in attributes)
			{
				switch(node.Name)
				{
					case "sprecher":  sprecher = node.InnerText;    break;
					case "text":	  text = node.InnerText; 	    break;
					case "audio":	  syncro = node.InnerText; 	    break;
					case "highlight": highlight = node.InnerText;   break;
					case "input":	  input = node.InnerText; 		break;
					case "action":	  action = node.InnerText;	    break;
					case "dict":	  dict = node.InnerText;		break;
					case "change":    changeColor = true;			break;
				}
			}

			if(input != "")
			{
				Debug.Log("Wait for Input '" + input + "'");
			}
			if(action != "")
			{
				Debug.Log("Wait for Action '" + action + "'");
			}
		}
	}
}

