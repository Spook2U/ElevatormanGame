using UnityEngine;
using System.Collections;
using System.Xml;

public class XMLController : MonoBehaviour 
{
	XmlDocument xmlDoc;

	XmlNodeList languagesNode = null;
	XmlNodeList artworksNode;
	XmlNodeList gamesNode;
	
	Artwork[] artworks;
	Games[]   games;



	public void Awake()
	{
		LoadXml("database");

//		getMinispielID(1);

	}



	private void LoadXml(string file)
	{
		xmlDoc = new XmlDocument();
		
		TextAsset xmlAsset = (TextAsset) Resources.Load(file);
		xmlDoc.LoadXml(xmlAsset.text);

		languagesNode = xmlDoc.GetElementsByTagName("sprachen")  .Item(0).ChildNodes;
		artworksNode  = xmlDoc.GetElementsByTagName("kunstwerke").Item(0).ChildNodes;
		gamesNode     = xmlDoc.GetElementsByTagName("minispiele").Item(0).ChildNodes;

		PlayerPrefs.SetInt("LanguagesCount", getLanguages().Length);

		artworks = new Artwork[artworksNode.Count];
		for(int i = 0; i < artworksNode.Count; i++) 
		{
			artworks[i] = new Artwork(artworksNode.Item(i).ChildNodes);
		}

		games = new Games[gamesNode.Count];
		for(int i = 0; i < gamesNode.Count; i++)
		{
			games[i] = new Games(gamesNode.Item(i).ChildNodes);
		}
	}

	public string[] getLanguages()
	{	
		string[] array = null;

		if(languagesNode != null)
		{
			array = new string[languagesNode.Count];

			for(int i = 0; i < languagesNode.Count; i++)
			{
				array[i] = languagesNode.Item(i).InnerText;
			}
		}

		return array;
	}
}

class Artwork
{
	public int id = 0;
	public int minispielId = 0;
	public int position = 0;
	public string bild = "";
	private Info[] info;

	public Artwork(XmlNodeList list)
	{
		id 			= int.Parse(list.Item(0).InnerText);
		minispielId = int.Parse(list.Item(1).InnerText);
		position 	= int.Parse(list.Item(2).InnerText);
		bild		=           list.Item(3).InnerText ;

		info = new Info[PlayerPrefs.GetInt("LanguagesCount")];

		for(int i = 0; i < info.Length; i++)
		{
			info[i] = new Info(list.Item(4).ChildNodes.Item(i).ChildNodes);
		}
	}

	public string GetText(int sprache) { return info[sprache].text; }
	public string GetSync(int sprache) { return info[sprache].sync; }
}

class Games
{
	public int id = 0;
	public int reihenfolge = 0;
	private Info[] info;

	public Games(XmlNodeList list)
	{
		id 			= int.Parse(list.Item(0).InnerText);
		reihenfolge = int.Parse(list.Item(1).InnerText);

		info = new Info[PlayerPrefs.GetInt("LanguagesCount")];
		
		for(int i = 0; i < info.Length; i++)
		{
			info[i] = new Info(list.Item(2).ChildNodes.Item(i).ChildNodes);
		}
	}

	public string   GetText(int sprache)  { return info[sprache].text; }
	public string   GetSync(int sprache)  { return info[sprache].sync; }
	public string[] GetTipps(int sprache) { return info[sprache].tipps;}
}

class Info
{
	public string text;
	public string sync;
	public string[] tipps;

	public Info(XmlNodeList list)
	{
		text = list.Item(0).InnerText;
		sync = list.Item(1).InnerText;

		if(list.Item(2) != null)
		{
			XmlNodeList tippsNode = list.Item(2).ChildNodes;
			tipps = new string[tippsNode.Count];

			for(int i = 0; i < tippsNode.Count; i++)
			{
				tipps[i] = tippsNode.Item(i).InnerText;
				Debug.Log(tipps[i]);
			}
		}
	}
}