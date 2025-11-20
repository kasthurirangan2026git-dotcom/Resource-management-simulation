using System.IO;

using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;
    public Color TeamColor;

    private void Awake()
    {   
       if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
       
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadColor();
    } 
    
    [System.Serializable]
class SaveData
{
   public Color TeamColor ;
}

 public void SaveColor()
    {
        SaveData Data = new SaveData();
        Data.TeamColor = TeamColor;

        string json = JsonUtility.ToJson(Data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json",json);
        Debug.Log(Application.persistentDataPath);
    }

    public void LoadColor()
        {
            string path = UnityEngine.Application.persistentDataPath + "/savefile.json";

            if (File.Exists(path))
            {
                string json =File.ReadAllText(path);
                SaveData data = JsonUtility.FromJson<SaveData>(json);
                TeamColor = data.TeamColor;

            }
        }

}
