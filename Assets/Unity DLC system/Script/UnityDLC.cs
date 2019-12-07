using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UnityDLC : MonoBehaviour
{
    public string dlcstringUrl;
    [Header("UI DLC")]
    public Transform rootDicContainer;
    public DLC dlcPrefeb;

    [Header("UI SCENE LIST")]
    public Transform rootContainer;
    public Button prefeb;
    public Text labelText;

    string[] dlcName, dlcUrls;


    public static UnityDLC main
    {
        get;
        private set;
    }
    public static string dlcPath;

    static List<AssetBundle> assetBundles = new List<AssetBundle>();
    static List<string> scensNames = new List<string>();

    public void Init()
    {
        StartCoroutine(LoadAssets());
    }

    //private IEnumerator Start()
    private void Start()
    {
        main = this;
        dlcPath = (Application.platform == RuntimePlatform.Android ? Application.persistentDataPath : Application.dataPath) + "/DLC/";
        Debug.Log("@@@@@@@@@@@ dlcPath:" + dlcPath);
        //using (WWW www = new WWW(dlcstringUrl))
        //{
        //    yield return www;
        //    if (!string.IsNullOrEmpty(www.error))
        //    {
        //        Debug.LogError(www.error);
        //        yield break;
        //    }
        //    string[] lines = www.text.Split(new string[] { System.Environment.NewLine }, System.StringSplitOptions.RemoveEmptyEntries);
        //    dlcName = System.Array.ConvertAll(lines, dlc => dlc.Split(new string[] { "<url>" }, System.StringSplitOptions.RemoveEmptyEntries)[0]);

        //    dlcUrls = System.Array.ConvertAll(lines, dlc => dlc.Split(new string[] { "<url>" }, System.StringSplitOptions.RemoveEmptyEntries)[1]);

        //}

        dlcName = new string[] { "Unity Demo", "Other Needs" };

        //dlcUrls = new string[] { "https://drive.google.com/uc?export=download&id=1l9Zwbmh66JpcS881MsJbFrvS2tZ43XFQ", "https://drive.google.com/uc?export=download&id=1V4AHKsH786-6TU-_ZfvShcVMs1LxLJbC" };
        //dlcUrls = new string[] { "file:///C:/Users/mbaapp/Documents/Unity%20Project/AppDemo/git%20Proj/New%20folder/DLC/Assets/AssetBundels/mydlc.dlc", "file:///C:/Users/mbaapp/Documents/Unity%20Project/AppDemo/git%20Proj/New%20folder/DLC/Assets/AssetBundels/primitive.dlc" };

        dlcUrls = new string[] { "https://drive.google.com/uc?export=download&id=1CM8IS34glZ674p-5vqd4l3wZqsLjhhjm", "https://drive.google.com/uc?export=download&id=1HgZRUxGdX9--P56IWiJdEOWZvMS1fMBO" };
        Init();
    }
    string checkDown(string url)
    {
        string fileName = dlcPath;
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
        //this.filePath = fileName;
        return fileName;
    }
    IEnumerator LoadAssets()
    {
        if (!Directory.Exists(dlcPath))
        {
            Debug.Log("Tring to create Path:" + dlcPath);
            Directory.CreateDirectory(dlcPath);
        }

        foreach (var item in assetBundles)
        {
            item.Unload(true);

        }
        assetBundles.Clear();
        scensNames.Clear();

        int i = 0;
        while (i < dlcName.Length)
        {
            //string path = dlcPath + Path.GetFileName(dlcUrls[i]);
            string path = checkDown(dlcUrls[i]);
            if (File.Exists(path))
            {
                var bundleRequest = AssetBundle.LoadFromFileAsync(path);
                yield return bundleRequest;

                assetBundles.Add(bundleRequest.assetBundle);

                scensNames.AddRange(bundleRequest.assetBundle.GetAllScenePaths());
            }
            i++;
            yield return null;
        }

        ////DELETE UNused
        string[] dlcFiles = Directory.GetFiles(dlcPath);
        foreach (var item in dlcFiles)
        {
            if (Path.GetExtension(item) != ".meta")
            {
                bool used = false;
                var filedata = dlcUrls.FirstOrDefault(x => x.EndsWith(item));
                if (filedata != null)
                {
                    File.Delete(item);
                }
            }
        }
        RefreshSceneList();
    }
    public void ShowDLC()
    {
        foreach (Transform t in rootDicContainer)
        {
            Destroy(t.gameObject);
        }
        for (int i = 0; i < dlcName.Length; i++)
        {
            var clone = Instantiate(dlcPrefeb.gameObject) as GameObject;

            clone.transform.SetParent(rootDicContainer);

            //clone.GetComponent<DLC>().Inti(dlcName[i], dlcUrls[i]);
            clone.SetActive(true);
        }
    }
    public void RefreshSceneList()
    {
        foreach (Transform item in rootContainer)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in scensNames)
        {
            labelText.text = Path.GetFileNameWithoutExtension(item);
            var clone = Instantiate(prefeb.gameObject) as GameObject;
            clone.GetComponent<Button>().AddEventListener(labelText.text, LoadAssetBundelScens);

            clone.SetActive(true);
            clone.transform.SetParent(rootContainer);
            //Debug.Log(item);
            //Debug.Log(Path.GetFileNameWithoutExtension(item));
        }
    }
    public void LoadAssetBundelScens(string scenName)
    {
        SceneManager.LoadScene(scenName);
    }
}
