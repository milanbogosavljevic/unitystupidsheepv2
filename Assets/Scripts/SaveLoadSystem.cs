using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadSystem : MonoBehaviour
{
    private GameData _data;
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        GameObject[] objs = GameObject.FindGameObjectsWithTag("SaveLoadSystem");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        _data = new GameData();
        LoadFile();
    }

    public GameData GetGameData()
    {
        return _data;
    }

    public void SaveScore(float score)
    {
        _data.highScore = score;
        SaveFile();
    }

    public void SaveDontShowInfo(bool dontShow)
    {
        _data.dontShowInfo = dontShow;
        SaveFile();
    }

    public void SaveSoundState(bool isOn)
    {
        _data.soundIsOn = isOn;
        SaveFile();
    }

    public void SaveMusicState(bool isOn)
    {
        _data.musicIsOn = isOn;
        SaveFile();
    }

    public void SaveFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;
 
        if(File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);
 
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, _data);
        file.Close();
    }

    public void LoadFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;
 
        if(File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.Log("File not found");
            return;
        }
 
        BinaryFormatter bf = new BinaryFormatter();
        _data = (GameData) bf.Deserialize(file);
        file.Close();
    }
}
