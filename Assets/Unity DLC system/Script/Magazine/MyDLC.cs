using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class MyDLC : MonoBehaviour
{
    [Header("UI Staff")]
    public Image image;
    public Image background;
    public Text nameText;
    public Slider progressbar;
    public Button downloadbutton;

    [Header("Appearance")]
    public Color avilavleColor, downloadedColore;

    string bundalUrl, filePath;

    public void Inti(Magazine magazine)
    {
        string dlcName = magazine.name;
        string dlcUrl = magazine.url;
        nameText.text = dlcName;
        bundalUrl = dlcUrl;
        filePath = UnityDLC.dlcPath + magazine.fileName;
        bool downloaded = File.Exists(filePath);
        background.color = downloaded ? downloadedColore : avilavleColor;
        progressbar.value = downloaded ? 1 : 0;
        downloadbutton.gameObject.SetActive(!downloaded);
    }
    public void Download()
    {
        StartCoroutine(CoDownload());
    }
    public void Delete()
    {
        if (File.Exists(filePath))
        {
            if (Path.GetExtension(filePath) != ".meta")
            {
                File.Delete(filePath);
            }
        }
        else if (Directory.Exists(filePath))
        {
            string[] dlcFiles = Directory.GetFiles(filePath);
            foreach (var item in dlcFiles)
            {
                if (Path.GetExtension(item) != ".meta")
                {
                    File.Delete(item);
                    //bool used = false;
                    //var filedata = dlcUrls.FirstOrDefault(x => x.EndsWith(item));
                    //if (filedata != null)
                    //{
                    //}
                }
            }
        }
        LoadMagazine.main.ShowDLC();
    }
    IEnumerator CoDownload()
    {
        downloadbutton.gameObject.SetActive(false);
        using (WebClient webClient = new WebClient())
        {
            try
            {
                webClient.DownloadFile(bundalUrl, filePath);

                //#if !UNITY_WEBPLAYER
                //                File.WriteAllBytes(filePath, byt);
                //#endif
                LoadMagazine.main.ShowDLC();

            }
            catch (Exception ex)
            {
            }
            yield return null;

        }
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //request.Method = "PUT";
        //request.ContentType = "application/octet-stream";
        //request.ContentLength = data.Length;
        //Stream stream = request.GetRequestStream();
        //stream.Write(data, 0, data.Length);
        //stream.Close();
        //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //StreamReader reader = new StreamReader(response.GetResponseStream());
        //temp = reader.();
        //reader.Close();

        //        using (WWW www = new WWW(bundalUrl))
        //        {
        //            while (!www.isDone && string.IsNullOrEmpty(www.error))
        //            {
        //                progressbar.value = www.progress;
        //                yield return null;
        //            }

        //            if (!string.IsNullOrEmpty(www.error))
        //            {
        //                Debug.LogError(www.error);
        //                yield break;
        //            }
        //#if !UNITY_WEBPLAYER
        //            File.WriteAllBytes(filePath, www.bytes);
        //#endif
        //        }
        LoadMagazine.main.ShowDLC();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
