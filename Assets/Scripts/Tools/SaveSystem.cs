using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem
{
    [System.Serializable]
    public class Score
    {
        public void Set(float _height, float _speed)
        {
            height = _height;
            speed = _speed;
        }

        public float height = 0f;
        public float speed = 0f;
    }

    static public void Save(Score scoreData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/PenguinSlide.save";

        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, scoreData);
        stream.Close();
    }

    static public Score Load()
    {
        string path = Application.persistentDataPath + "/PenguinSlide.save";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            Score scoreData = formatter.Deserialize(stream) as Score;

            stream.Close();

            return scoreData;
        }

        return new Score();
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
