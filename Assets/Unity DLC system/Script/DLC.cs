using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class DLC : MonoBehaviour
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

    public void Inti(string dlcName, string dlcUrl)
    {
        nameText.text = dlcName;
        bundalUrl = dlcUrl;
        filePath = UnityDLC.dlcPath + Path.GetFileName(dlcUrl);
        bool downloaded = checkDown(dlcUrl);
        background.color = downloaded ? downloadedColore : avilavleColor;
        progressbar.value = downloaded ? 1 : 0;
        downloadbutton.gameObject.SetActive(!downloaded);
    }
    bool checkDown(string url)
    {
        string fileName = UnityDLC.dlcPath;
        if (url.Contains("drive.google.com/"))
        {
            if (url.Contains("1CM8IS34glZ674p-5vqd4l3wZqsLjhhjm"))
            {
                fileName = fileName + "mydlc.dlc";
            }
            else
            {
                fileName = fileName + "primitive.dlc";
            }
        }
        else
        {
            fileName = UnityDLC.dlcPath + Path.GetFileName(url);
        }
        this.filePath = fileName;
        return File.Exists(fileName);
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
        UnityDLC.main.ShowDLC();
    }
    IEnumerator CoDownload()
    {
        Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ bundalUrl:" + bundalUrl);
        downloadbutton.gameObject.SetActive(false);
        using (WebClient webClient = new WebClient())
        {
            try
            {
                Debug.Log("bundalUrl:" + bundalUrl);
                if (bundalUrl.Contains("drive.google.com/"))
                {
                    if (bundalUrl.Contains("1CM8IS34glZ674p-5vqd4l3wZqsLjhhjm"))
                    {
                        filePath = UnityDLC.dlcPath + "mydlc.dlc";
                    }
                    else
                    {
                        filePath = UnityDLC.dlcPath + "primitive.dlc";
                    }
                }
                webClient.DownloadFile(bundalUrl, filePath);

                //#if !UNITY_WEBPLAYER
                //                File.WriteAllBytes(filePath, byt);
                //#endif
                UnityDLC.main.ShowDLC();

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
        UnityDLC.main.ShowDLC();
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
