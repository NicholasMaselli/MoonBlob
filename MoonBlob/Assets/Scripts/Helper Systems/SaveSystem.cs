using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SavePlayer(PlayerData playerData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.bob";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, playerData);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.bob";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            
            PlayerData playerData = (PlayerData)formatter.Deserialize(stream);
            stream.Close();

            return playerData;
        }
        else
        {
            Debug.Log("Save file not found in" + path);
            return null;
        }
    }
}
