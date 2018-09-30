using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine.UI;
public class MyTest 
{
    [MenuItem("Test/ChangeName")]
    static void testc()
    {
        ChangeFileName(rootPath);
        Debug.Log("转换结束");
    }

    public static string rootPath = "L:/VisionSourceVideo";

    static void ChangeFileName(string path)
    {       
        DirectoryInfo folder = new DirectoryInfo(path);
        DirectoryInfo[] diis = folder.GetDirectories();
        FileInfo[] fils = folder.GetFiles("*.mp4");
        foreach (FileInfo file in fils)
        {         
            string newname = file.FullName.Replace("__", "");
            
            file.MoveTo(newname);
            //Debug.Log(newname);
        }
        foreach (DirectoryInfo dic in diis)
        {
            ChangeFileName(dic.FullName);
        }    
    }    
}