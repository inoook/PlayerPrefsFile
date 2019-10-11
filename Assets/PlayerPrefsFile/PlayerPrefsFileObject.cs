using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;


public class PlayerPrefsFileObject
{
    public enum PrefType
    {
        INT, FLOAT, STRING, BOOL, COLOR, VECTOR2, VECTOR3, RECT, ENUM
    }
    public class Pref
    {
        public string key;
        public PrefType type;
        public object v;

        public string comment;

        public Pref(string key_, PrefType type_, object v_, string comment_ = "") {
            key = key_;
            type = type_;
            v = v_;

            comment = comment_;
        }
    }

    private static char NEWLINE = '\n';
    private static string SPLITTER = ",";
    private static string COMMENT = "// ";

    private Dictionary<string, Pref> keyAndValues = null;
    public string dataPath = "";

    public PlayerPrefsFileObject(string path) {
        dataPath = path;
        keyAndValues = null;
    }
    public PlayerPrefsFileObject() {
        keyAndValues = null;
    }

    #region save and load
    public void SavePrepare(string path) {
        //keyAndValues = null;
        dataPath = path;
    }
    public void Save(string path) {
        dataPath = path;
        Save();
    }
    public void Save() {
        WriteToFile();
    }
    public string GetSavedString() {
        return GetAllKeyAndValues();
    }

    void WriteToFile() {
        string str = GetAllKeyAndValues();
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        string path = dataPath;

        Debug.LogWarning("Save: " + path);
        Debug.LogWarning(str);

        File.WriteAllBytes(path, bytes);
    }

    public void Load(string path) {
        dataPath = path;
        keyAndValues = null;
        Load();
    }

    public void Load() {
        if (keyAndValues != null) { return; }

        string path = dataPath;
        LoadFromFile(path);
    }

    void LoadFromFile(string path) {
        if (keyAndValues != null) { return; } // 既にロード済み

        Debug.LogWarning("LoadFromPath: " + path);
        keyAndValues = new Dictionary<string, Pref>();

        if (!File.Exists(path)) {
            // no file
            return;
        }
        using (System.IO.StreamReader reader = new System.IO.StreamReader(path)) {
            string str = reader.ReadToEnd();

            string[] splitStr = str.Split(NEWLINE);

            for (int i = 0; i < splitStr.Length; i++) {
                if (splitStr[i].IndexOf(COMMENT, StringComparison.OrdinalIgnoreCase) > -1) {
                    // コメントアウトは無視
                    continue;
                }
                string[] prefLine = splitStr[i].Split(SPLITTER[0]);
                if (prefLine.Length > 2) {
                    string key = prefLine[0];
                    string t = prefLine[1];
                    PrefType type = PrefType.FLOAT;
                    object v = null;
                    if (t == PrefType.INT.ToString()) {
                        type = PrefType.INT;
                        v = int.Parse(prefLine[2]);
                    }
                    else if (t == PrefType.BOOL.ToString()) {
                        type = PrefType.BOOL;
                        v = bool.Parse(prefLine[2]);
                    }
                    else if (t == PrefType.FLOAT.ToString()) {
                        type = PrefType.FLOAT;
                        v = float.Parse(prefLine[2]);
                    }
                    else if (t == PrefType.STRING.ToString()) {
                        type = PrefType.STRING;
                        v = prefLine[2];
                    }
                    else if (t == PrefType.COLOR.ToString()) {
                        type = PrefType.COLOR;
                        v = new Color(float.Parse(prefLine[2]), float.Parse(prefLine[3]), float.Parse(prefLine[4]), float.Parse(prefLine[5]));
                    }
                    else if (t == PrefType.VECTOR2.ToString()) {
                        type = PrefType.VECTOR2;
                        v = new Vector2(float.Parse(prefLine[2]), float.Parse(prefLine[3]));
                    }
                    else if (t == PrefType.VECTOR3.ToString()) {
                        type = PrefType.VECTOR3;
                        v = new Vector3(float.Parse(prefLine[2]), float.Parse(prefLine[3]), float.Parse(prefLine[4]));
                    }
                    else if (t == PrefType.RECT.ToString()) {
                        type = PrefType.RECT;
                        v = new Rect(float.Parse(prefLine[2]), float.Parse(prefLine[3]), float.Parse(prefLine[4]), float.Parse(prefLine[5]));
                    }
                    else if (t == PrefType.ENUM.ToString()) {
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

    public string GetAllKeyAndValues() {
        string str = "";
        foreach (KeyValuePair<string, Pref> p in keyAndValues) {
            string key = p.Key;
            Pref pref = p.Value;

            PrefType type = pref.type;
            if (key != pref.key) {
                Debug.LogWarning("Error");
                continue;
            }

            string vStr = pref.v.ToString();

            if (type == PrefType.COLOR) {
                Color c = ((Color)(pref.v));
                vStr = c.r + SPLITTER + c.g + SPLITTER + c.b + SPLITTER + c.a;
            }
            else if (type == PrefType.RECT) {
                Rect c = ((Rect)(pref.v));
                vStr = c.x + SPLITTER + c.y + SPLITTER + c.width + SPLITTER + c.height;
            }
            else if (type == PrefType.VECTOR3) {
                Vector3 c = ((Vector3)(pref.v));
                vStr = c.x + SPLITTER + c.y + SPLITTER + c.z;
            }
            else if (type == PrefType.VECTOR2) {
                Vector2 c = ((Vector2)(pref.v));
                vStr = c.x + SPLITTER + c.y;
            }
            else if (type == PrefType.BOOL) {
                bool b = ((bool)(pref.v));
                vStr = b.ToString();
            }
            else if (type == PrefType.ENUM) {
                object e = pref.v;
                Type t = pref.v.GetType();
                vStr = e.ToString() + SPLITTER + t.ToString();
            }

            string comment = pref.comment;
            if (comment != "") {
                str += NEWLINE + COMMENT + comment + NEWLINE;
            }
            str += key + SPLITTER + type + SPLITTER + vStr + NEWLINE;
        }
        return str;
    }
    //
    public bool HasKey(string key) {
        return keyAndValues.ContainsKey(key);
    }

    public void DeleteKey(string key) {
        keyAndValues.Remove(key);
    }

    public void DeleteAll() {
        keyAndValues.Clear();
    }

    #endregion
    void Init() {
        if (keyAndValues == null) {
            keyAndValues = new Dictionary<string, Pref>();
        }
    }

    // original method
    public void SetInt(string key, int v, string comment = "") {
        Init();

        Pref pref = new Pref(key, PrefType.INT, v, comment);
        if (!keyAndValues.ContainsKey(key)) {
            keyAndValues.Add(key, pref);
        }
        else {
            keyAndValues[key] = pref;
        }
    }

    public int GetInt(string key, int defaultValue) {
        Load();

        if (keyAndValues.ContainsKey(key)) {
            return (int)(keyAndValues[key].v);
        }
        return defaultValue;
    }

    public void SetFloat(string key, float v, string comment = "") {
        Init();

        Pref pref = new Pref(key, PrefType.FLOAT, v, comment);
        if (!keyAndValues.ContainsKey(key)) {
            keyAndValues.Add(key, pref);
        }
        else {
            keyAndValues[key] = pref;
        }
    }

    public float GetFloat(string key, float defaultValue) {
        Load();

        if (keyAndValues.ContainsKey(key)) {
            return (float)(keyAndValues[key].v);
        }
        return defaultValue;
    }

    public void SetString(string key, string str, string comment = "") {
        Init();

        Pref pref = new Pref(key, PrefType.STRING, str, comment);
        if (!keyAndValues.ContainsKey(key)) {
            keyAndValues.Add(key, pref);
        }
        else {
            keyAndValues[key] = pref;
        }
    }

    public string GetString(string key, string defaultValue) {
        Load();

        if (keyAndValues.ContainsKey(key)) {
            return (string)(keyAndValues[key].v);
        }
        return defaultValue;
    }

    // bool
    public void SetBool(string key, bool b, string comment = "") {
        Init();

        Pref pref = new Pref(key, PrefType.BOOL, b, comment);
        if (!keyAndValues.ContainsKey(key)) {
            keyAndValues.Add(key, pref);
        }
        else {
            keyAndValues[key] = pref;
        }
    }

    public bool GetBool(string key, bool defaultValue) {
        Load();

        if (keyAndValues.ContainsKey(key)) {
            return (bool)(keyAndValues[key].v);
        }
        return defaultValue;
    }

    // Vector3
    public void SetVector3(string key, Vector3 v, string comment = "") {
        Init();

        Pref pref = new Pref(key, PrefType.VECTOR3, v, comment);
        if (!keyAndValues.ContainsKey(key)) {
            keyAndValues.Add(key, pref);
        }
        else {
            keyAndValues[key] = pref;
        }
    }

    public Vector3 GetVector3(string key, Vector3 defaultValue) {
        Load();

        if (keyAndValues.ContainsKey(key)) {
            return (Vector3)(keyAndValues[key].v);
        }
        return defaultValue;
    }

    // Vector2
    public void SetVector2(string key, Vector2 v, string comment = "") {
        Init();

        Pref pref = new Pref(key, PrefType.VECTOR2, v, comment);
        if (!keyAndValues.ContainsKey(key)) {
            keyAndValues.Add(key, pref);
        }
        else {
            keyAndValues[key] = pref;
        }
    }

    public Vector2 GetVector2(string key, Vector2 defaultValue) {
        Load();

        if (keyAndValues.ContainsKey(key)) {
            return (Vector2)(keyAndValues[key].v);
        }
        return defaultValue;
    }

    // Rect
    public void SetRect(string key, Rect v, string comment = "") {
        Init();

        Pref pref = new Pref(key, PrefType.RECT, v, comment);
        if (!keyAndValues.ContainsKey(key)) {
            keyAndValues.Add(key, pref);
        }
        else {
            keyAndValues[key] = pref;
        }
    }

    public Rect GetRect(string key, Rect defaultValue) {
        Load();

        if (keyAndValues.ContainsKey(key)) {
            return (Rect)(keyAndValues[key].v);
        }
        return defaultValue;
    }

    // Color
    public void SetColor(string key, Color v, string comment = "") {
        Init();

        Pref pref = new Pref(key, PrefType.COLOR, v, comment);
        if (!keyAndValues.ContainsKey(key)) {
            keyAndValues.Add(key, pref);
        }
        else {
            keyAndValues[key] = pref;
        }
    }

    public Color GetColor(string key, Color defaultValue) {
        Load();

        if (keyAndValues.ContainsKey(key)) {
            return (Color)(keyAndValues[key].v);
        }
        return defaultValue;
    }

    // Enum
    public void SetEnum(string key, object v, string comment = "") {
        Init();

        Pref pref = new Pref(key, PrefType.ENUM, v, comment);
        if (!keyAndValues.ContainsKey(key)) {
            keyAndValues.Add(key, pref);
        }
        else {
            keyAndValues[key] = pref;
        }
    }

    public object GetEnum(string key, object defaultValue) {
        Load();

        if (keyAndValues.ContainsKey(key)) {
            return (keyAndValues[key].v);
        }
        return defaultValue;
    }
}
