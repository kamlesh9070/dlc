using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadMagazine : MonoBehaviour
{
    [Header("UI DLC")]
    public Transform rootDicContainer;
    public DLC dlcPrefeb;

    [Header("UI SCENE LIST")]
    public Transform rootContainer;
    public Button prefeb;
    public Text labelText;

    Magazine[] magazineList;

    public static LoadMagazine main
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
        MagazineList magazineListObj = WSCall.GetMagazineList();
        magazineList = magazineListObj.data;
        Init();
    }
    IEnumerator LoadAssets()
    {
        if (!Directory.Exists(dlcPath))
        {
            Directory.CreateDirectory(dlcPath);
        }

        foreach (var item in assetBundles)
        {
            item.Unload(true);

        }
        assetBundles.Clear();
        scensNames.Clear();

        int i = 0;
        while (i < magazineList.Length)
        {
            string path = dlcPath + magazineList[i].fileName;
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
                var filedata = magazineList.FirstOrDefault(x => x.url.EndsWith(item));
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
        for (int i = 0; i < magazineList.Length; i++)
        {
            var clone = Instantiate(dlcPrefeb.gameObject) as GameObject;

            clone.transform.SetParent(rootDicContainer);

            clone.GetComponent<MyDLC>().Inti(magazineList[i]);
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
