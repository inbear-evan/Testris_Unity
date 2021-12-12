using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
[Serializable]
public class Data
{
    public string id;
    public int score;

    public void printData()
    {
        Debug.Log("id : " + id);
        Debug.Log("score : " + score);
    }
}
[Serializable]
public class Serialization<T>
{
    [SerializeField] List<T> target;
    public List<T> toList() { return target; }

    public Serialization(List<T> target)
    {
        this.target = target;
    }
}

public class JsonManager : MonoBehaviour
{
    public static JsonManager instance;
    private void Awake()
    {
        instance = this;
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    [SerializeField] InputField inputId;
    public List<Data> sortData = new List<Data> ();
    [SerializeField] Text[] idUI;
    [SerializeField] Text[] scoreUI;
    [SerializeField] GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        string path = Application.dataPath + "/score.json";
        if (File.Exists(path)) readScoreData(path);
        else File.WriteAllText(path, JsonUtility.ToJson(""));
        canvasShow(false) ;
    }
    public void canvasShow(bool chk)
    {
        canvas.SetActive(chk);
    }
    void readScoreData(string path)
    {
        string readJson = File.ReadAllText(path);
        List<Data> readdata = JsonUtility.FromJson<Serialization<Data>>(readJson).toList();
        //var data = JsonHelper.FromJson<Data>(readJson);

        readdata.Sort(delegate (Data a, Data b)
        {
            if (a.score < b.score) { return 1; }
            else if (a.score > b.score) { return -1; }
            else { return 0; }
        });

        if (readdata.Count < 5)
        {
            int buf = 0;
            for (int i = 0; i < readdata.Count; i++)
            {
                sortData.Add(readdata[i]);
                idUI[i].text = readdata[i].id;
                scoreUI[i].text = readdata[i].score.ToString();
                buf = i+1;
            }
            for (int i = buf; i < 5; i++)
            {
                idUI[i].text = "";
                scoreUI[i].text = "";
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                sortData.Add(readdata[i]);
                idUI[i].text = readdata[i].id;
                scoreUI[i].text = readdata[i].score.ToString();
            }
        }
    }
    public void onSaveScore()
    {
        string path = Application.dataPath + "/score.json";
        Data data = new Data();
        data.id = inputId.text;
        data.score = ScoreManager.instance.SCORELINES;
        sortData.Add (data);
        string saveData = JsonUtility.ToJson(new Serialization<Data>(sortData), prettyPrint:true);
        //Debug.Log(saveData);
        File.WriteAllText(path, saveData);
        sortData.Clear();
        readScoreData(path);

        SceneManager.LoadScene(0);
    }
}
