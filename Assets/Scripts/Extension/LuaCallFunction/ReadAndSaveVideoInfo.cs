using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System.Security.Cryptography;
using System.Text;

public class ReadAndSaveVideoInfo
{
    //------------------------------------时间读写-----------------------------------------------
    //获取视频时长
    private static SerializableDictionary<string, int> serializableDictionary = GetDictionary();

    //读取视频时长
    public static int ReadVideoTime(string videourl)
    {
        string savePath = GetRelativePath(GetFullPath(videourl));
        return serializableDictionary.ContainsKey(savePath) ? serializableDictionary[savePath] : 0;
    }

    //保存time到xml
    public static void SaveVideoTime(string videourl, int videotime)
    {
        string savePath = GetRelativePath(GetFullPath(videourl));
        ////初始化
        //SerializableDictionary<string, int> serializableDictionary;
        //if (!File.Exists(PathConfig.VideoTimeXmlPath))
        //{
        //    serializableDictionary = new SerializableDictionary<string, int>();
        //    serializableDictionary.Add("初始数据", 0);
        //}
        //else
        //{
        //    serializableDictionary = GetDictionary();
        //}

        if (!serializableDictionary.ContainsKey(savePath))
        {
            serializableDictionary.Add(savePath, videotime);
            //Debug.Log("加入XML缓存："+ videourl);
            using (FileStream fileStream = new FileStream(PathConfig.VideoTimeXmlPath, FileMode.Create))
            {
                XmlSerializer xmlFormatter = new XmlSerializer(typeof(SerializableDictionary<string, int>));
                xmlFormatter.Serialize(fileStream, serializableDictionary);
                fileStream.Dispose();
            }
        }
    }

    //获取xml反序列化后的dic
    static SerializableDictionary<string, int> GetDictionary()
    {     
        //初始化
        SerializableDictionary<string, int> serializableDictionary;
        if (!File.Exists(PathConfig.VideoTimeXmlPath))
        {
            serializableDictionary = new SerializableDictionary<string, int>();
            serializableDictionary.Add("初始数据", 0);
            using (FileStream fileStream = new FileStream(PathConfig.VideoTimeXmlPath, FileMode.Create))
            {
                XmlSerializer xmlFormatter = new XmlSerializer(typeof(SerializableDictionary<string, int>));
                xmlFormatter.Serialize(fileStream, serializableDictionary);
                fileStream.Dispose();
            }
        }
        else
        {
            using (FileStream fileStream = new FileStream(PathConfig.VideoTimeXmlPath, FileMode.Open))
            {
                XmlSerializer xmlFormatter = new XmlSerializer(typeof(SerializableDictionary<string, int>));
                serializableDictionary = (SerializableDictionary<string, int>)xmlFormatter.Deserialize(fileStream);
                fileStream.Dispose();
            }
        }
        return serializableDictionary;
    }

    //-----------------------------------缩略图读写--------------------------------------------------
    //加载缩略图
    public static Sprite GetShowImageSpriteByUrl(string videourl)
    {
        string resPath = GetShowImagePath(videourl);
        Sprite movieSprite = Resources.Load<Sprite>(resPath);
        if (null == movieSprite)
        {
            movieSprite = Resources.Load<Sprite>(PathConfig.MovieShowImageRootPath + "UnKnowImage");
        }   
        return movieSprite;
    }

    //获取图片保存路径,保存图片名为对应的md5码
    public static string GetImageSavePathByUrl(string videourl)
    {
        string videofullpath = GetFullPath(videourl);
        string Relativepath = GetRelativePath(videofullpath);

        string movieName = Path.GetFileNameWithoutExtension(Relativepath);  //"1美分如何让我自觉像百万富翁 (类型：心理 TED)"
        string RelativeRoot = Path.GetDirectoryName(Relativepath);  //"TED/心理"
        string md5Name = StrToMD5(movieName); //efad4b5402f8243290d6312c15fb6891

        string tempPath = PathConfig.ImageSaveResourcesPath + RelativeRoot + "/" + md5Name;
        string savePath = Path.ChangeExtension(tempPath, ".png");
        return savePath;
    }

    //获取图片加载路径
    static string GetShowImagePath(string videourl)
    {
            string videofullpath = GetFullPath(videourl);
            string Relativepath = GetRelativePath(videofullpath);
            string movieName = Path.GetFileNameWithoutExtension(Relativepath);  //"1美分如何让我自觉像百万富翁 (类型：心理 TED)"
            string RelativeRoot = Path.GetDirectoryName(Relativepath);  //"TED/心理"
            string md5Name = StrToMD5(movieName); //efad4b5402f8243290d6312c15fb6891     
            string tempPath = PathConfig.MovieShowImageRootPath + RelativeRoot + "/" + md5Name;
            return tempPath;      
    }

    //获取全路径
    static string GetFullPath(string url)
    {
        string fullpath = new System.Uri(url).LocalPath;//去掉url里的file://
        return fullpath;
    }

    //获取相对路径
    static string GetRelativePath(string fullPath)
    {
        fullPath = Path.GetFullPath(fullPath).Replace('\\', '/');
        return fullPath.Substring(21); //"L:/VisionSourceVideo/"
    }

    //字符串转md5
    static string StrToMD5(string str)
    {
        byte[] data = Encoding.GetEncoding("GB2312").GetBytes(str);
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] OutBytes = md5.ComputeHash(data);

        string OutString = "";
        for (int i = 0; i < OutBytes.Length; i++)
        {
            OutString += OutBytes[i].ToString("x2");
        }
        return OutString.ToLower();
    }    
}