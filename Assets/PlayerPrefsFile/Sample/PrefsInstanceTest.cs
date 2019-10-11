using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PrefsTest2, PrefsTest3, と同じであるが、static関数ではなくInstanceを使用したもの。
/// PlayerPrefsFileも内部では PlayerPrefsFileObject を使用している。
/// </summary>
public class PrefsInstanceTest : MonoBehaviour
{

    [SerializeField] int test1 = 1;
    [SerializeField] float test2 = 2f;
    [SerializeField] string test3 = "test";
    [SerializeField] Color testColor = Color.red;
    [SerializeField] Rect rect = new Rect(10, 10, 200, 200);
    [SerializeField] Vector2 vec2;
    [SerializeField] Vector3 vec3;
    [SerializeField] bool boolean;

    [SerializeField] TEST_ENUM enumType;

    [SerializeField] string fileName = "savedDataInstance.txt";

    PlayerPrefsFileObject prefsFileInstance;

    // Use this for initialization
    void Start() {

        LoadSetting();

    }

    string GetDataPath() {
        return Application.streamingAssetsPath + "/saveData/" + fileName;
    }

    void LoadSetting() {
        prefsFileInstance = new PlayerPrefsFileObject();
        prefsFileInstance.Load(GetDataPath());

        test1 = prefsFileInstance.GetInt("test1", test1);
        test2 = prefsFileInstance.GetFloat("test2", test2);
        test3 = prefsFileInstance.GetString("test3", test3);
        testColor = prefsFileInstance.GetColor("testColor", testColor);
        rect = prefsFileInstance.GetRect("rect", rect);
        vec2 = prefsFileInstance.GetVector2("vec2", vec2);
        vec3 = prefsFileInstance.GetVector3("vec3", vec3);
        boolean = prefsFileInstance.GetBool("boolean", boolean);

        enumType = (TEST_ENUM)prefsFileInstance.GetEnum("enumType", enumType);
    }

    [ContextMenu("Save")]
    void Save() {
        //prefsFileInstance.SavePrepare(GetDataPath());

        prefsFileInstance.SetInt("test1", test1);
        prefsFileInstance.SetFloat("test2", test2);
        prefsFileInstance.SetString("test3", test3);
        prefsFileInstance.SetColor("testColor", testColor, "色のテスト");
        prefsFileInstance.SetRect("rect", rect);
        prefsFileInstance.SetVector2("vec2", vec2, "Vectors");
        prefsFileInstance.SetVector3("vec3", vec3);
        prefsFileInstance.SetBool("boolean", boolean);

        prefsFileInstance.SetEnum("enumType", enumType);

        prefsFileInstance.Save();// write file
    }

    [ContextMenu("DeleteAll")]
    void Delete() {
        prefsFileInstance.DeleteAll();
    }
}
