using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;


public static class PlayerPrefsFile
{
    private static Dictionary<string, PlayerPrefsFileObject> keyAndInstance = null;
    public static string dataPath = Application.streamingAssetsPath + "/savedData.txt";

    private static PlayerPrefsFileObject currentInstance = null;

    #region save and load
    public static void SavePrepare(string path) {
        CreateInstanceByPath(path);
    }
    public static void Save() {
        string path = dataPath; // default Data Path
        currentInstance.Save(path);
    }

    public static string GetSavedString() {
        return currentInstance.GetAllKeyAndValues();
    }

    public static void Load(string path) {
        CreateInstanceByPath(path);
        currentInstance.Load();
    }

    static void GetPrepare() {
        // 最初にLoadされていない時に defaultDataPath からLoad
        if (currentInstance == null) {
            CreateInstanceByPath(dataPath);
            currentInstance.Load();
        }
    }

    static void CreateInstanceByPath(string path = "") {
        if (keyAndInstance == null) {
            keyAndInstance = new Dictionary<string, PlayerPrefsFileObject>();
        }
        if (path == "") {
            path = dataPath;
        }
        if (keyAndInstance.ContainsKey(path)) {
            currentInstance = keyAndInstance[path];
        }
        else {
            currentInstance = new PlayerPrefsFileObject(path);
            keyAndInstance[path] = currentInstance;
        }
    }
    #endregion

    //
    //
    public static bool HasKey(string key) {
        return currentInstance.HasKey(key);
    }

    public static void DeleteKey(string key) {
        currentInstance.DeleteKey(key);
    }

    public static void DeleteAll() {
        currentInstance.DeleteAll();
    }
    // original method
    public static void SetInt(string key, int v) {
        CreateInstanceByPath();

        currentInstance.SetInt(key, v);
    }

    public static int GetInt(string key, int defaultValue) {
        GetPrepare();

        return currentInstance.GetInt(key, defaultValue);
    }

    public static void SetFloat(string key, float v) {
        CreateInstanceByPath();

        currentInstance.SetFloat(key, v);
    }

    public static float GetFloat(string key, float defaultValue) {
        GetPrepare();

        return currentInstance.GetFloat(key, defaultValue);
    }

    public static void SetString(string key, string str) {
        CreateInstanceByPath();

        currentInstance.SetString(key, str);
    }

    public static string GetString(string key, string defaultValue) {
        GetPrepare();

        return currentInstance.GetString(key, defaultValue);
    }

    // bool
    public static void SetBool(string key, bool b) {
        CreateInstanceByPath();

        currentInstance.SetBool(key, b);
    }

    public static bool GetBool(string key, bool defaultValue) {
        GetPrepare();

        return currentInstance.GetBool(key, defaultValue);
    }

    // Vector3
    public static void SetVector3(string key, Vector3 v) {
        CreateInstanceByPath();

        currentInstance.SetVector3(key, v);
    }

    public static Vector3 GetVector3(string key, Vector3 defaultValue) {
        GetPrepare();

        return currentInstance.GetVector3(key, defaultValue);
    }

    // Vector2
    public static void SetVector2(string key, Vector2 v) {
        CreateInstanceByPath();

        currentInstance.SetVector2(key, v);
    }

    public static Vector2 GetVector2(string key, Vector2 defaultValue) {
        GetPrepare();

        return currentInstance.GetVector2(key, defaultValue);
    }

    // Rect
    public static void SetRect(string key, Rect v) {
        CreateInstanceByPath();

        currentInstance.SetRect(key, v);
    }

    public static Rect GetRect(string key, Rect defaultValue) {
        GetPrepare();

        return currentInstance.GetRect(key, defaultValue);
    }

    // Color
    public static void SetColor(string key, Color v) {
        CreateInstanceByPath();

        currentInstance.SetColor(key, v);
    }

    public static Color GetColor(string key, Color defaultValue) {
        GetPrepare();

        return currentInstance.GetColor(key, defaultValue);
    }

    // Enum
    public static void SetEnum(string key, object v) {
        CreateInstanceByPath();

        currentInstance.SetEnum(key, v);
    }

    public static object GetEnum(string key, object defaultValue) {
        GetPrepare();

        return currentInstance.GetEnum(key, defaultValue);
    }
}
