using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public class Data
{
    public string question;
    public Choices[] choices;
}

[System.Serializable]
public class Choices
{
    public string choice;
    public int votes;
}

public class JSON_D : MonoBehaviour {

    string path;
    string jsonString;

    //In game things.
    public GameObject bannerObject;
    public Text headingText;
    public Transform parentObject;
    public Button reloadBtn;



    public void ReloadBtnClick()
    {
        reloadBtn.interactable =false ;
        foreach (Transform child in parentObject)
        {
            GameObject.Destroy(child.gameObject);
        }
        StartCoroutine(JsonFromUrl());
    }


	// Use this for initialization
    void JsonOffline()
    {
        path = Application.streamingAssetsPath + "/data.json";
        jsonString = File.ReadAllText(path);
        Processjson(jsonString);
	}


  
    IEnumerator JsonFromUrl()
    {
        string url = "https://private-5b1d8-sampleapi187.apiary-mock.com/questions";
        WWW www = new WWW(url);
        yield return www;
        if (www.error == null)
        {
            Processjson(www.text);
        }
        else
        {
            Debug.Log("ERROR: " + www.error);
            JsonOffline();
        }
  
        reloadBtn.interactable = true;
    }

    private void Processjson(string jsonString)
    {
        string jsonSubString = jsonString.Substring(1, jsonString.Length - 2);
        Debug.Log(jsonSubString);
        Data data = JsonUtility.FromJson<Data>(jsonSubString);

        //putting data to game components.

        headingText.text = data.question;

        for (int i = 0; i < data.choices.Length; i++)
        {
            GameObject temp = Instantiate(bannerObject, parentObject);
            temp.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = data.choices[i].choice;
            temp.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = data.choices[i].votes.ToString();
            
        }

    }
	
}
