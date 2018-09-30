using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

//返回的movieinfo结构
public struct VideoStruct
{
    public string url;
    public string name;
}

public static class MySearch
{
    //模糊搜索
    public static List<VideoStruct> SearchByInputText(string matchstr)
    {
        List<VideoStruct> videolist = new List<VideoStruct>();
        foreach (string rootpath in PathConfig.GetAllRootPath())
        {
            if (Directory.Exists(rootpath))
            {
                RecursiveSearchByMatchStr(matchstr, rootpath, videolist);
            }            
        }         
        return videolist;
    }
    static void RecursiveSearchByMatchStr(string matchstr, string path, List<VideoStruct> videolist)
    {            
        DirectoryInfo folder = new DirectoryInfo(path);
        FileInfo[] fils = folder.GetFiles("*.mp4");
        DirectoryInfo[] diis = folder.GetDirectories();
        Regex r = new Regex(matchstr);
        foreach (FileInfo file in fils)
        {
            Match m = r.Match(file.Name);
            if (m.Success)
            {
                VideoStruct tempvideo = new VideoStruct();
                tempvideo.url = "file://" + file.FullName;
                tempvideo.name = Path.GetFileNameWithoutExtension(file.Name);
                videolist.Add(tempvideo);
            }
        }
        foreach (DirectoryInfo dic in diis)
        {
            RecursiveSearchByMatchStr(matchstr, dic.FullName, videolist);
        }
    }

    //检索搜索
    public static List<VideoStruct> SearchByConditions(string menuname, string con1, string con2)
    {
        List<VideoStruct> videolist = new List<VideoStruct>();
        foreach (string rootpath in PathConfig.GetAllRootPath())
        {
            string fullSearchPath = rootpath + GetSearchRelativePathByConditions(menuname, con1, con2);
            if (Directory.Exists(fullSearchPath))
            {
                RecursiveSearchOnlyByPath(fullSearchPath, videolist);
            }           
        }
        return videolist;
    }
    static string GetSearchRelativePathByConditions(string menuname, string con1, string con2)
    {
        string searchpath;
        if (con1 != null && con2 == null)
        {
            if (con1 == "全部")
            {
                searchpath =  menuname;
            }
            else
            {
                searchpath =  menuname + "/" + con1;
            }
        }
        else if (con1 != null && con2 != null)
        {

            if (con1 == "全部" && con2 == "全部")
            {
                searchpath =  menuname;
            }
            else if (con1 != "全部" && con2 == "全部")
            {
                searchpath =  menuname + "/" + con1;
            }
            else
            {
                searchpath =  menuname + "/" + con1 + "/" + con2;
            }
        }
        else
        {
            searchpath = menuname;
        }
        return searchpath;
    }
    
    //获取随机数据
    public static List<VideoStruct> GetRandomDataByMenuname(int count, string menuname)
    {
        List<VideoStruct> randomdata = new List<VideoStruct>();
        List<VideoStruct> menudata = GetAllDataBymenuname(menuname);
        for (int i = 0; i < count; i++)
        {
            int randomindex = UnityEngine.Random.Range(0, menudata.Count - 1);
            randomdata.Add(menudata[randomindex]);
            menudata.Remove(menudata[randomindex]);
        }
        return randomdata;
    }
    static List<VideoStruct> GetAllDataBymenuname(string menuname)
    {
        List<VideoStruct> videolist = new List<VideoStruct>();
        foreach (string rootpath in PathConfig.GetAllRootPath())
        {
            string fullSearchPath;
            if (menuname != "推荐")
            {
                fullSearchPath = rootpath + menuname;
            }
            else
            {
                fullSearchPath = rootpath;
            }
            if (Directory.Exists(fullSearchPath))
            {
                RecursiveSearchOnlyByPath(fullSearchPath, videolist);
            }          
        }
        return videolist;
    }

    //获取相关数据，够10个则返回10个，不够则返回全部
    public static List<VideoStruct> GetRelatedDataByUrl(string url)
    {
        string localpath = GetFullPath(url);
        string searchpath = Path.GetDirectoryName(localpath);
        List<VideoStruct> relateddata = new List<VideoStruct>();
        RecursiveSearchOnlyByPath(searchpath, relateddata);
        List<VideoStruct> randomdata = new List<VideoStruct>();
        int count;
        if (relateddata.Count >= 10)
        {
            count = 10;
        }
        else
        {
            count = relateddata.Count;
        }
        for (int i = 0; i < count; i++)
        {
            int randomindex = UnityEngine.Random.Range(0, relateddata.Count - 1);
            randomdata.Add(relateddata[randomindex]);
            relateddata.Remove(relateddata[randomindex]);
        }

        return randomdata;
    }
    public static string GetFullPath(string url)
    {
        string fullpath = new Uri(url).LocalPath;//去掉url里的file://
        return fullpath;
    }

    //删除不能播放的视频
    public static void DeleteMovieByUrl(string url)
    {
        string fullpath = new Uri(url).LocalPath;//去掉url里的file://
        if (File.Exists(fullpath))
        {
            File.Delete(fullpath);
        }
    }

    //递归path下的所有.mp4文件，并存入videolist中
    static void RecursiveSearchOnlyByPath(string path, List<VideoStruct> videolist)
    {
        DirectoryInfo folder = new DirectoryInfo(path);
        FileInfo[] fils = folder.GetFiles("*.mp4");
        DirectoryInfo[] diis = folder.GetDirectories();

        foreach (FileInfo file in fils)
        {
            VideoStruct tempvideo = new VideoStruct();
            tempvideo.url = "file://" + file.FullName;
            tempvideo.name = Path.GetFileNameWithoutExtension(file.Name);
            videolist.Add(tempvideo);
        }
        foreach (DirectoryInfo dic in diis)
        {
            RecursiveSearchOnlyByPath(dic.FullName, videolist);
        }
    }
}
