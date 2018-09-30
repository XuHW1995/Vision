using UnityEditor;
using UnityEngine;
using System.IO;

public class Tools
{
    //删除目录和子目录下所有后缀为suffix值的文件
    public static void deleteFiles(string filePath,string suffix) //suffix  例如 *.meta
    {
        foreach (string file in Directory.GetFiles(filePath, suffix/* "*.manifest" */, SearchOption.AllDirectories))
        {
            File.Delete(file);
        }
    }
    //拷贝文件
    public static void CopyFile(string fromFile, string toFile)
    {
        File.Copy(fromFile, toFile,true);
        //File.WriteAllBytes(toFile, File.ReadAllBytes(fromFile));
    }
    //拷贝目录和子目录，除了.meta文件和.svn文件夹
    public static void CopyFolder(string from, string to,bool withMateFiles = false)
    {
        if (!Directory.Exists(to))
            Directory.CreateDirectory(to);

        // 子文件夹
        foreach (string sub in Directory.GetDirectories(from))
        {
            if (Path.GetFileName(sub) != ".svn")
            {
                CopyFolder(sub + "/", to + Path.GetFileName(sub) + "/");
            }
        }

        // 文件
        foreach (string file in Directory.GetFiles(from))
        {
            var ex = Path.GetExtension(file);
            if (withMateFiles)
            {
                    File.Copy(file, to + Path.GetFileName(file), true);
            }
            else
            {
                if (ex != ".meta")
                    File.Copy(file, to + Path.GetFileName(file), true);
            }
        }
    }
    //暂时没用到
    public static void BATCMD(string cmdstr)
    {
        Debug.Log(cmdstr);
        string cmd = Application.dataPath + "/../ToolsBAT/" + cmdstr;
        System.Diagnostics.Process.Start(cmd, "ToolsBAT/").WaitForExit();
        Debug.Log("DONE!");
    }
    //把lua文件转换为.txt文件
    public static void LuatoTxt()
    {
        string luaDirPath = EditorTools.PathConfig.LUA+"/";
        string luaTxtDirPath = EditorTools.PathConfig.LUATXT+"/";
        if (!Directory.Exists(luaDirPath)) return;
        if (Directory.Exists(luaTxtDirPath)) Directory.Delete(luaTxtDirPath, true);
        Directory.CreateDirectory(luaTxtDirPath);
        string[] files = Directory.GetFiles(luaDirPath, "*.lua", SearchOption.AllDirectories);
        foreach (string file in files)
        {
            string newPath = luaTxtDirPath + file.Replace(luaDirPath, "").Replace("/", "_").Replace("\\", "_") + ".txt";
            File.Copy(file, newPath, true);
        }
        AssetDatabase.Refresh();
    }
    //拷贝目录下的.xml文件到streamingassets
    public static void CopyXmlFolder()
    {
        string from = EditorTools.PathConfig.BEHAVIORTREE;
        string to = Application.dataPath+ "/Resources/behaviac/exported/";
        if (!Directory.Exists(to))
            Directory.CreateDirectory(to);

        //.svn和behaviac_generated文件不拷贝
        foreach (string sub in Directory.GetDirectories(from))
        {
            if (Path.GetFileName(sub) != ".svn" && Path.GetFileName(sub) != "behaviac_generated" )
            {
                CopyFolder(sub + "/", to + Path.GetFileName(sub) + "/");
            }
        }
        // 拷贝除.meta文件文件
        foreach (string file in Directory.GetFiles(from))
        {
            var ex = Path.GetExtension(file);
            if (ex != ".meta")
            {
               File.Copy(file, to + Path.GetFileName(file), true);
            }
        }
    }
}

