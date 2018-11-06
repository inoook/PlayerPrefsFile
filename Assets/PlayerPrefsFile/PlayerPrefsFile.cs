using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;


public static class PlayerPrefsFile  {
    
    public enum PrefType
    {
        INT, FLOAT, STRING, BOOL, COLOR, VECTOR2, VECTOR3, RECT, ENUM
    }
    public class Pref
    {
        public string key;
        public PrefType type;
        public object v;

        public Pref(string key_, PrefType type_, object v_)
        {
            key = key_;
            type = type_;
            v = v_;
        }
    }
	
    private static Dictionary<string, Pref> keyAndValues = null;
    public static string dataPath = Application.dataPath + "/savedData.txt";

#region save and load
    public static void Save()
    {
        WriteToFile();
    }
    public static string GetSavedString()
    {
        return GetAllKeyAndValues();
    }

    static void WriteToFile()
    {
        string str = GetAllKeyAndValues();
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        
        string path = dataPath;
        File.WriteAllBytes(path, bytes);
    }

    public static void Load()
    {
        if (keyAndValues != null) { return; }

        string path = dataPath;
        LoadFromFile(path);
    }

    static void LoadFromFile(string path)
    {
        keyAndValues = new Dictionary<string, Pref>();

        if (!File.Exists(path))
        {
            // no file
            return;
        }
        using (System.IO.StreamReader reader = new System.IO.StreamReader(path))
        {
            string str = reader.ReadToEnd();

            string[] splitStr = str.Split('\n');

            for (int i = 0; i < splitStr.Length; i++)
            {
                string[] prefLine = splitStr[i].Split(',');
                if (prefLine.Length > 2)
                {
                    string key = prefLine[0];
                    string t = prefLine[1];
                    PrefType type = PrefType.FLOAT;
                    object v = null;
                    if (t == PrefType.INT.ToString())
                    {
                        type = PrefType.INT;
                        v = int.Parse(prefLine[2]);
                    }
                    else if (t == PrefType.BOOL.ToString())
                    {
                        type = PrefType.BOOL;
                        v = bool.Parse(prefLine[2]);
                    }
                    else if (t == PrefType.FLOAT.ToString())
                    {
                        type = PrefType.FLOAT;
                        v = float.Parse(prefLine[2]);
                    }
                    else if (t == PrefType.STRING.ToString())
                    {
                        type = PrefType.STRING;
                        v = prefLine[2];
                    }
                    else if (t == PrefType.COLOR.ToString())
                    {
                        type = PrefType.COLOR;
                        v = new Color(float.Parse(prefLine[2]), float.Parse(prefLine[3]), float.Parse(prefLine[4]), float.Parse(prefLine[5]));
                    }
                    else if (t == PrefType.VECTOR2.ToString())
                    {
                        type = PrefType.VECTOR2;
                        v = new Vector2(float.Parse(prefLine[2]), float.Parse(prefLine[3]));
                    }
                    else if (t == PrefType.VECTOR3.ToString())
                    {
                        type = PrefType.VECTOR3;
                        v = new Vector3(float.Parse(prefLine[2]), float.Parse(prefLine[3]), float.Parse(prefLine[4]));
                    }
                    else if (t == PrefType.RECT.ToString())
                    {
                        type = PrefType.RECT;
                        v = new Rect(float.Parse(prefLine[2]), float.Parse(prefLine[3]), float.Parse(prefLine[4]), float.Parse(prefLine[5]));
                    }
                    else if (t == PrefType.ENUM.ToString())
                    {
                        type = PrefType.ENUM;
                        Type enumType = Type.GetType(prefLine[3]);
                        v = Enum.Parse(enumType, prefLine[2]);
                    }

                    Pref p = new Pref(key, type, v);

                    keyAndValues.Add(key, p);
                }
            }
            reader.Close();
        }
    }

    static string GetAllKeyAndValues()
    {
        string str = "";
        foreach (KeyValuePair<string, Pref> p in keyAndValues)
        {
            string key = p.Key;
            Pref pref = p.Value;

            PrefType type = pref.type;
            if(key != pref.key){
                Debug.LogWarning("Error");
                continue;
            }

            string vStr = pref.v.ToString();

            if (type == PrefType.COLOR)
            {
                Color c = ((Color)(pref.v));
                vStr = c.r + "," + c.g + "," + c.b + "," + c.a;
            }
            else if (type == PrefType.RECT)
            {
                Rect c = ((Rect)(pref.v));
                vStr = c.x + "," + c.y + "," + c.width + "," + c.height;
            }
            else if (type == PrefType.VECTOR3)
            {
                Vector3 c = ((Vector3)(pref.v));
                vStr = c.x + "," + c.y + "," + c.z;
            }
            else if (type == PrefType.VECTOR2)
            {
                Vector2 c = ((Vector2)(pref.v));
                vStr = c.x + "," + c.y;
            }
            else if (type == PrefType.BOOL)
            {
                bool b = ((bool)(pref.v));
                vStr = b.ToString();
            }
            else if (type == PrefType.ENUM)
            {
                object e = pref.v;
                Type t = pref.v.GetType();
                vStr = e.ToString()+","+t.ToString();
            }
            str += key + "," + type + "," + vStr + "\n";
        }
        return str;
    }
    //
    public static bool HasKey(string key)
    {
        return keyAndValues.ContainsKey(key);
    }

    public static void DeleteKey(string key)
    {
        keyAndValues.Remove(key);
    }

    public static void DeleteAll()
    {
        keyAndValues.Clear();
    }

    #endregion
    static void Init()
    {
        if (keyAndValues == null) {
            keyAndValues = new Dictionary<string, Pref>();
        }
    }

	// original method
	public static void SetInt(string key, int v)
	{
        Init();

        Pref pref = new Pref(key, PrefType.INT, v);
        if (!keyAndValues.ContainsKey(key)){
            keyAndValues.Add(key, pref);
        }else{
            keyAndValues[key] = pref;
        }
	}

    public static int GetInt(string key, int defaultValue)
	{
        Load();

        if(keyAndValues.ContainsKey(key)){
            return (int)(keyAndValues[key].v);
        }
        return defaultValue;
	}
	
	public static void SetFloat(string key, float v)
	{
        Init();

        Pref pref = new Pref(key, PrefType.FLOAT, v);
        if (!keyAndValues.ContainsKey(key)){
            keyAndValues.Add(key, pref);
        } else{
            keyAndValues[key] = pref;
        }
	}

	public static float GetFloat(string key, float defaultValue)
	{
        Load();

        if (keyAndValues.ContainsKey(key)){
            return (float)(keyAndValues[key].v);
        }
        return defaultValue;
	}
	
	public static void SetString(string key, string str)
	{
        Init();

        Pref pref = new Pref(key, PrefType.STRING, str);
        if (!keyAndValues.ContainsKey(key)){
            keyAndValues.Add(key, pref);
        } else{
            keyAndValues[key] = pref;
        }
	}

    public static string GetString(string key, string defaultValue)
	{
        Load();

        if (keyAndValues.ContainsKey(key)) {
            return (string)(keyAndValues[key].v);
        }
        return defaultValue;
	}
    
    // bool
    public static void SetBool(string key, bool b)
	{
        Init();

        Pref pref = new Pref(key, PrefType.BOOL, b);
        if (!keyAndValues.ContainsKey(key)) {
            keyAndValues.Add(key, pref);
        } else {
            keyAndValues[key] = pref;
        }
	}

	public static bool GetBool(string key, bool defaultValue)
	{
        Load();

        if (keyAndValues.ContainsKey(key)) {
            return (bool)(keyAndValues[key].v);
        }
        return defaultValue;
	}

	// Vector3
    public static void SetVector3(string key, Vector3 v)
	{
        Init();

        Pref pref = new Pref(key, PrefType.VECTOR3, v);
        if (!keyAndValues.ContainsKey(key)){
            keyAndValues.Add(key, pref);
        } else {
            keyAndValues[key] = pref;
        }
	}
	
    public static Vector3 GetVector3(string key, Vector3 defaultValue)
	{
        Load();

        if (keyAndValues.ContainsKey(key)){
            return (Vector3)(keyAndValues[key].v);
        }
        return defaultValue;
	}
	
	// Vector2
    public static void SetVector2(string key, Vector2 v)
	{
        Init();

        Pref pref = new Pref(key, PrefType.VECTOR2, v);
        if (!keyAndValues.ContainsKey(key)) {
            keyAndValues.Add(key, pref);
        }else{
            keyAndValues[key] = pref;
        }
	}
	
    public static Vector2 GetVector2(string key, Vector2 defaultValue)
	{
        Load();

        if (keyAndValues.ContainsKey(key)) {
            return (Vector2)(keyAndValues[key].v);
        }
        return defaultValue;
	}

	// Rect
    public static void SetRect(string key, Rect v)
	{
        Init();

        Pref pref = new Pref(key, PrefType.RECT, v);
        if (!keyAndValues.ContainsKey(key)){
            keyAndValues.Add(key, pref);
        } else{
            keyAndValues[key] = pref;
        }
	}

    public static Rect GetRect(string key, Rect defaultValue)
	{
        Load();

        if (keyAndValues.ContainsKey(key)) {
            return (Rect)(keyAndValues[key].v);
        }
        return defaultValue;
	}

	// Color
	public static void SetColor(string key, Color v)
	{
        Init();

        Pref pref = new Pref(key, PrefType.COLOR, v);
        if (!keyAndValues.ContainsKey(key)) {
            keyAndValues.Add(key, pref);
        } else {
            keyAndValues[key] = pref;
        }
	}

    public static Color GetColor(string key, Color defaultValue)
	{
        Load();

        if (keyAndValues.ContainsKey(key)) {
            return (Color)(keyAndValues[key].v);
        }
        return defaultValue;
	}

	// Enum
	public static void SetEnum(string key, object v)
	{
        Init();

        Pref pref = new Pref(key, PrefType.ENUM, v);
        if (!keyAndValues.ContainsKey(key)) {
            keyAndValues.Add(key, pref);
        } else {
            keyAndValues[key] = pref;
        }
	}

    public static object GetEnum(string key, object defaultValue)
	{
        Load();

        if (keyAndValues.ContainsKey(key)) {
            return (keyAndValues[key].v);
        }
        return defaultValue;
	}
}
