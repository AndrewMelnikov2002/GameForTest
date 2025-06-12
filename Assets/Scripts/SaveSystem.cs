using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    public void SaveProgress(int global_score, List<InventoryItem> inventory)
    {
        SaveData new_save = new SaveData();

        new_save.score = global_score;

        new_save.inventory = inventory;

        string json_save = JsonUtility.ToJson(new_save);

        File.WriteAllText(Application.persistentDataPath + "/save.json", json_save);

        //Debug.Log("Game Progress Saved");
    }

    public void CleanSave()
    {
        SaveData new_save = new SaveData();

        new_save.score = 0;

        new_save.inventory = null;

        string json_save = JsonUtility.ToJson(new_save);

        File.WriteAllText(Application.persistentDataPath + "/save.json", json_save);

        SceneManager.LoadScene(0);

    }

    public SaveData LoadProgress()
    {
        SaveData save = null;

        string path = Application.persistentDataPath + "/save.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            save = JsonUtility.FromJson<SaveData>(json);
        }

        return save;
    }

}


[System.Serializable]
public class SaveData
{
    public int score;

    public List<InventoryItem> inventory;
}
