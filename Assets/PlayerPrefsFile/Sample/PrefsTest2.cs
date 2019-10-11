using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PrefsTest2, PrefsTest3, 保存するPathを指定する。
/// Load, SavePrepare で呼び出し・保存するPathを指定する。
/// </summary>
public class PrefsTest2 : MonoBehaviour
{

    [SerializeField] string fileName = "savedData2.txt";

    [SerializeField] int test1 = 1;
    [SerializeField] Color testColor = Color.red;
    [SerializeField] Rect rect = new Rect(10, 10, 200, 200);
    [SerializeField] Vector2 vec2;
    [SerializeField] bool boolean;

    // Use this for initialization
    void Start() {

        LoadSetting();

    }

    string GetDataPath() {
        return Application.streamingAssetsPath + "/saveData/" + fileName;
    }

    void LoadSetting() {
        PlayerPrefsFile.Load(GetDataPath());

        test1 = PlayerPrefsFile.GetInt("PrefsTest2.test1", test1);
        testColor = PlayerPrefsFile.GetColor("PrefsTest2.testColor", testColor);
        rect = PlayerPrefsFile.GetRect("PrefsTest2.rect", rect);
        vec2 = PlayerPrefsFile.GetVector2("PrefsTest2.vec2", vec2);
        boolean = PlayerPrefsFile.GetBool("PrefsTest2.boolean", boolean);
    }

    [ContextMenu("Save")]
    void Save() {
        PlayerPrefsFile.SavePrepare(GetDataPath());

        PlayerPrefsFile.SetInt("PrefsTest2.test1", test1);
        PlayerPrefsFile.SetColor("PrefsTest2.testColor", testColor);
        PlayerPrefsFile.SetRect("PrefsTest2.rect", rect);
        PlayerPrefsFile.SetVector2("PrefsTest2.vec2", vec2);
        PlayerPrefsFile.SetBool("PrefsTest2.boolean", boolean);

        Debug.LogWarning(PlayerPrefsFile.GetSavedString());
        PlayerPrefsFile.Save();// write file
    }

    [ContextMenu("DeleteAll")]
    void Delete() {
        PlayerPrefsFile.DeleteAll();
    }
}
