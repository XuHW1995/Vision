using UnityEngine;
using UnityEditor;

public class MyBuild : EditorWindow
{
    static MyBuild window;
    string version = "";
    bool bWindows = false;
    bool bAndroid = false;
    bool bIOS = false;
    bool cleanCache = true;
    [MenuItem("Tools/Build")]
    static void Init()
    {
        window = (MyBuild)EditorWindow.GetWindow(typeof(MyBuild));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Config:", EditorStyles.boldLabel);
        version = EditorGUILayout.TextField("version:", version);
        GUILayout.Label("Platform:", EditorStyles.boldLabel);
        bWindows = EditorGUILayout.Toggle("windows", bWindows);
        bAndroid =  EditorGUILayout.Toggle("android", bAndroid);
        bIOS =  EditorGUILayout.Toggle("ios", bIOS);
        GUILayout.Label("Build:", EditorStyles.boldLabel);
        cleanCache = EditorGUILayout.Toggle("总是在打包前清理缓存", cleanCache);
        bool isPress = GUI.Button(new Rect(0, position.height-30, position.width, 30), "开始打包");
       
        //打包键按下
        if (isPress)
        {
            //是否输入版本号
            if (isValidVersion(version))
            {
                if (bWindows)
                {
                    EditorTools.makeproj.BuildWINProject(version, cleanCache);
                }
                if (bAndroid)
                {
                    EditorTools.makeproj.BuildAndroidProject(version, cleanCache);
                }
                if (bIOS)
                {
                    EditorTools.makeproj.BuildIOSProject(version, cleanCache);
                }
            }
            else
            {
                Debug.LogError("Build rersion invalid!");
            }
        }
    }

    bool isValidVersion(string versiong)
    {
        string[] a = versiong.Split('.');
        if (a.Length != 3)
        {
            return false;
        }
        return true;
    }
}