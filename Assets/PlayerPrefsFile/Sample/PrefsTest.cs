using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TEST_ENUM
{
    A, B, C
}

// 通常のPlayerPrefsと同じように使えるように。保存されるPathは固定で1箇所。
public class PrefsTest : MonoBehaviour
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

    // Use this for initialization
    void Start() {
        PlayerPrefsFile.dataPath = Application.streamingAssetsPath + "/saveData/savedData.txt";
        LoadSetting();

    }

    void LoadSetting() {
        test1 = PlayerPrefsFile.GetInt("test1", test1);
        test2 = PlayerPrefsFile.GetFloat("test2", test2);
        test3 = PlayerPrefsFile.GetString("test3", test3);
        testColor = PlayerPrefsFile.GetColor("testColor", testColor);
        rect = PlayerPrefsFile.GetRect("rect", rect);
        vec2 = PlayerPrefsFile.GetVector2("vec2", vec2);
        vec3 = PlayerPrefsFile.GetVector3("vec3", vec3);
        boolean = PlayerPrefsFile.GetBool("boolean", boolean);

        enumType = (TEST_ENUM)PlayerPrefsFile.GetEnum("enumType", enumType);
    }

    [ContextMenu("Save")]
    void Save() {
        Debug.LogWarning(PlayerPrefsFile.GetSavedString());
        PlayerPrefsFile.SetInt("test1", test1);
        PlayerPrefsFile.SetFloat("test2", test2);
        PlayerPrefsFile.SetString("test3", test3);
        PlayerPrefsFile.SetColor("testColor", testColor);
        PlayerPrefsFile.SetRect("rect", rect);
        PlayerPrefsFile.SetVector2("vec2", vec2);
        PlayerPrefsFile.SetVector3("vec3", vec3);
        PlayerPrefsFile.SetBool("boolean", boolean);

        PlayerPrefsFile.SetEnum("enumType", enumType);

        PlayerPrefsFile.Save();// write file
    }

    [ContextMenu("DeleteAll")]
    void Delete() {
        PlayerPrefsFile.DeleteAll();
    }
}
