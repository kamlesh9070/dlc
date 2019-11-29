using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//using JsonUtility;
using System;
using UnityEngine.Networking;
using System.IO;
using System.Net;

public class WSCall
{
    public WSCall()
    {

    }
    public static MagazineList GetMagazineList()
    {
        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://youthapi.dbf.ooo:8000/get_magazine_list");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string jsonResponse = reader.ReadToEnd();
            Debug.Log("######################### " + jsonResponse);
            return JsonUtility.FromJson<MagazineList>(jsonResponse);
        } catch(Exception ex)
        {
            Debug.Log(ex.StackTrace);
        }
        return null;
    }
}