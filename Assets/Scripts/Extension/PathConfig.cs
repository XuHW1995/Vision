using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public class PathConfig {
    
    //配置文件路径
    public static string PathConfigPath = Application.dataPath + "/../PathConfig.txt";
    //缓存保存路径 
    public static string VideoTimeXmlPath = Application.streamingAssetsPath + "/Vision_Info/VideoTimeXml.xml";  
    public static string ImageSaveResourcesPath = Application.dataPath + "/Resources/MovieShowImage/";
    //缩略图加载Resoureces根目录
    public static string MovieShowImageRootPath = "MovieShowImage/";
    
    //多盘
    public static string[] GetAllRootPath()
    {       
        string[] lines = Regex.Split(File.ReadAllText(PathConfigPath), @"\s{1,}");
        return lines;
    }   
}
