using UnityEngine;
using System.Collections;
using System.Xml;

public class XMLController : MonoBehaviour 
{
	public int maxTipps;

	XmlDocument xmlDoc;

	XmlNodeList languagesNode = null;
	XmlNodeList artworksNode;
	XmlNodeList gamesNode;
	
	Artwork[] artworks;
	Games[]   games;

	int[] tippIndices;


	public void Awake()
	{
		LoadXml("database");

		tippIndices = new int[games.Length];
		for(int i = 0; i < tippIndices.Length; i++) { tippIndices[i] = -1; }

		if(maxTipps == 0) maxTipps = 1;
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

	public int GetArt_GameID(int pos) 
	{  
		int i = 0;
		for(i = 0; i < artworks.Length; i++)
		{
			if(artworks[i].position == pos) break;
		}

		return artworks[i].gameId;
	}

	public string GetArt_Pic(int pos) 
	{  
		int i = 0;
		for(i = 0; i < artworks.Length; i++)
		{
			if(artworks[i].position == pos) break;
		}
		
		return artworks[i].pic;
	}

	public string GetArt_Text(int pos, int language) 
	{  
		int i = 0;
		for(i = 0; i < artworks.Length; i++)
		{
			if(artworks[i].position == pos) break;
		}

		return artworks[i].GetText(language);
	}

	public string GetArt_Sync(int pos, int language) 
	{  
		int i = 0;
		for(i = 0; i < artworks.Length; i++)
		{
			if(artworks[i].position == pos) break;
		}
		
		return artworks[i].GetSync(language);
	}



	public int GetGame_Order(int gameId) 
	{  
		int i = 0;
		for(i = 0; i < games.Length; i++)
		{
			if(games[i].id == gameId) break;
		}
		
		return games[i].order;
	}

	public string GetGame_Text(int gameId, int language) 
	{  
		int i = 0;
		for(i = 0; i < games.Length; i++)
		{
			if(games[i].id == gameId) break;
		}
		
		return games[i].GetText(language);
	}
	
	public string GetGame_Sync(int gameId, int language) 
	{  
		int i = 0;
		for(i = 0; i < games.Length; i++)
		{
			if(games[i].id == gameId) break;
		}

		return games[i].GetSync(language);
	}

	public string GetGame_NextTipp(int gameId, int language)
	{
		int i = 0;
		for(i = 0; i < games.Length; i++)
		{
			if(games[i].id == gameId) break;
		}

		tippIndices[gameId] = (tippIndices[gameId] += 1) % maxTipps;

		return games[i].GetTipp(language)[tippIndices[gameId]];
	}
}

class Artwork
{
	public int position = 0;
	public int gameId = 0;
	public string pic = "";
	private Info[] info;

	public Artwork(XmlNodeList list)
	{
		info = new Info[PlayerPrefs.GetInt("LanguagesCount")];

		for(int i = 0; i < list.Count; i++)
		{
			switch(list.Item(i).Name)
			{
				case "position": 	 position = int.Parse(list.Item(i).InnerText); break;
				case "minispiel-id": gameId   = int.Parse(list.Item(i).InnerText); break;
				case "bild": 		 pic 	  =           list.Item(i).InnerText ; break;
				case "info":
				{
					for(int j = 0; j < info.Length; j++)
					{
						info[j] = new Info(list.Item(i).ChildNodes.Item(j).ChildNodes);
					}
				}
				break;
			}
		}
	}

	public string GetText(int sprache) { return info[sprache].text; }
	public string GetSync(int sprache) { return info[sprache].sync; }
}

class Games
{
	public int id = 0;
	public int order = 0;
	private Info[] info;

	public Games(XmlNodeList list)
	{
		info = new Info[PlayerPrefs.GetInt("LanguagesCount")];

		for(int i = 0; i < list.Count; i++)
		{
			switch(list.Item(i).Name)
			{
				case "id": 			id    = int.Parse(list.Item(i).InnerText); break;
				case "reihenfolge": order = int.Parse(list.Item(i).InnerText); break;
				case "info":
				{
					for(int j = 0; j < info.Length; j++)
					{
						info[j] = new Info(list.Item(i).ChildNodes.Item(j).ChildNodes);
					}
				}
				break;
			}
		}
	}

	public string   GetText(int sprache) { return info[sprache].text; }
	public string   GetSync(int sprache) { return info[sprache].sync; }
	public string[] GetTipp(int sprache) { return info[sprache].tipps;}
}

class Info
{
	public string text;
	public string sync;
	public string[] tipps;

	public Info(XmlNodeList list)
	{
		for(int i = 0; i < list.Count; i++)
		{
			switch(list.Item(i).Name)
			{
				case "text": text = list.Item(i).InnerText; break;
				case "sync": sync = list.Item(i).InnerText; break;
				case "tipps":
				{
					XmlNodeList tippsNode = list.Item(i).ChildNodes;
					tipps = new string[tippsNode.Count];
					
					for(int j = 0; j < tippsNode.Count; j++)
					{
						tipps[j] = tippsNode.Item(j).InnerText;
					}
				}
				break;
			}
		}
	}



}