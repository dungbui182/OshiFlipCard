using GrandDreams.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class ConfigModel
{
    public int TimeDuration = 30;

    public string WindowName = "OshiFlipCard2023";
    public int PreferScreenPosX = 0;
    public int PreferScreenPosY = 0;
    public int PreferScreenSizeWidth = 1920;
    public int PreferScreenSizeHeight = 1080;
    public bool AllowToSwitchScreen = true;
    public float MaxUserDistance = 2.2f;
    public float GuideLineTimeDuration = 3;



    public ConfigModel()
    {
        TimeDuration = 30;

        WindowName = "OshiFlipCard2023";
        PreferScreenPosX = 0;
        PreferScreenPosY = 0;
        PreferScreenSizeWidth = 1920;
        PreferScreenSizeHeight = 1080;
        AllowToSwitchScreen = true;
        MaxUserDistance = 2.2f;
        GuideLineTimeDuration = 3;

    }
}

[Serializable]
public class SVector2
{
    public float X;
    public float Y;

    public SVector2()
    {
        X = 0;
        Y = 0;
    }

    public SVector2(float x, float y)
    {
        X = x;
        Y = y;
    }

    public SVector2(Vector2 vector)
    {
        X = vector.x;
        Y = vector.y;
    }

    public Vector2 ToVector2()
    {
        return new Vector2(X, Y);
    }
}

public static class SVector2Extensions
{
    public static Vector2[] ToArrayVector2(this SVector2[] array)
    {
        return array.Select(x => x.ToVector2()).ToArray();
    }

    public static SVector2[] ToArraySVector2(this Vector2[] array)
    {
        return array.Select(x => new SVector2(x)).ToArray();
    }
}

[Serializable]
public class SVector3
{
    public float X;
    public float Y;
    public float Z;

    public SVector3()
    {
        X = 0;
        Y = 0;
        Z = 0;
    }

    public SVector3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = y;
    }

    public SVector3(float x, float y)
    {
        X = x;
        Y = y;
        Z = 0;
    }

    public SVector3(Vector3 vector)
    {
        X = vector.x;
        Y = vector.y;
        Z = vector.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(X, Y, Z);
    }
}

[Serializable]
public class STransform
{
    public SVector3 Position;
    public SVector3 EulerAngles;
    public SVector3 Scale;

    public STransform()
    {
        Position = new SVector3();
        EulerAngles = new SVector3();
        Scale = new SVector3();
    }

    public STransform(SVector3 position, SVector3 eulerAngles, SVector3 scale)
    {
        Position = position;
        EulerAngles = eulerAngles;
        Scale = scale;
    }

    public STransform(Vector3 position, Vector3 eulerAngles, Vector3 scale)
    {
        Position = new SVector3(position);
        EulerAngles = new SVector3(eulerAngles);
        Scale = new SVector3(scale);
    }

    public void ApplyTo(Transform transform)
    {
        transform.position = Position.ToVector3();
        transform.eulerAngles = EulerAngles.ToVector3();
        transform.localScale = Scale.ToVector3();
    }
}

public static class SVector3Extensions
{
    public static Vector3[] ToArrayVector2(this SVector3[] array)
    {
        return array.Select(x => x.ToVector3()).ToArray();
    }

    public static SVector3[] ToArraySVector3(this Vector3[] array)
    {
        return array.Select(x => new SVector3(x)).ToArray();
    }
}

public class ConfigData<T> where T : new()
{
    #region Declare Variables

    protected static string fileName = "Config.txt";
    protected static string filePath = Application.streamingAssetsPath + "/" + fileName;

    protected static string jsonData;
    public static string JsonData
    {
        get
        {
#if UNITY_STANDALONE_WIN || UNITY_WEBGL

            filePath = Application.streamingAssetsPath + @"\" + fileName;

            if (!System.IO.File.Exists(filePath))
            {
                T data = new T();
                SaveData(data);
                return UnityEngine.JsonUtility.ToJson(data);
            }
            else
            {
                var jsonString = System.IO.File.ReadAllText(filePath);
                if (jsonString.Length <= 2)
                {
                    T data = new T();
                    SaveData(data);
                    return UnityEngine.JsonUtility.ToJson(data);
                }
                else
                {
                    return System.IO.File.ReadAllText(filePath);
                }
            }
#elif UNITY_IOS || UNITY_ANDROID
            return "";
#endif
        }
        set
        {
            jsonData = value;
            System.IO.File.WriteAllText(filePath, jsonData, Encoding.Unicode);
        }
    }

    protected static T data;
    public static T Data
    {
        get
        {
#if UNITY_STANDALONE_WIN
            if (data == null)
            {
                data = UnityEngine.JsonUtility.FromJson<T>(JsonData);
            }
#elif UNITY_IOS || UNITY_ANDROID
            if (data == null)
            {
                //data = new ConfigModel();
            }
#endif
            return data;
        }
        set
        {
            data = value;
        }
    }

    #endregion Declare Variables

    #region Public Function

    public static void SaveData(T configData = default(T))
    {
        if (configData == null)
        {
            JsonData = UnityEngine.JsonUtility.ToJson(data, true);
        }
        else
        {
            JsonData = UnityEngine.JsonUtility.ToJson(configData, true);
        }
    }

    public static bool Initialize()
    {
        fileName = (typeof(T).Name == "ConfigModel" ? "Config.txt" : typeof(T).Name.Replace("Model", "") + ".txt");

        bool result = true;

        int counter = 0;
        while (Data == null)
        {
            result = Data != null;
            counter++;
            if (counter >= 10)
            {
                break;
            }
        }

        //Debug.Log(ConfigData.filePath);

        //Debug.Log(string.Format("ConfigData is {0}initialized.", !result ? "not " : ""));
        return result;
    }

    #endregion Public Function
}

public class ConfigData : ConfigData<ConfigModel>
{

}

