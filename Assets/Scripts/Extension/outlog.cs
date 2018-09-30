using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class outlog :MonoBehaviour
{
    //方法1：不带时间输出log，同时把错误信息用GUI显示出来
    //输出GUI的List
    static List<string> mLines = new List<string>();
    //写入文件的list
    static List<string> mWriteTxt = new List<string>();
    private string outpath;
    void Awake()
    {
        outpath = Application.persistentDataPath + "/outLog.txt";
        //每次启动客户端删除之前保存的Log
        if (File.Exists(outpath))
        {
            File.Delete(outpath);
        }
        //在这里做一个Log的监听
        Application.logMessageReceivedThreaded += HandleLog;
        //一个输出
        Debug.Log("Rimworld-outlogstart");
    }
    void Update()
    {
        //因为写入文件的操作必须在主线程中完成，所以在Update中写入文件。
        if (mWriteTxt.Count > 0)
        {
            string[] temp = mWriteTxt.ToArray();
            foreach (string t in temp)
            {
                using (StreamWriter writer = new StreamWriter(outpath, true, Encoding.UTF8))
                {
                    writer.WriteLine(t);
                }
                mWriteTxt.Remove(t);
            }
        }
    }
    void HandleLog(string logString, string stackTrace, LogType type)
    {      
        mWriteTxt.Add(logString);
        if (type == LogType.Error || type == LogType.Exception)
        {
            Log(logString);
            Log(stackTrace);
        }
    }
    
    //错误的信息保存起来，用来输出在手机屏幕上
    static public void Log(params object[] objs)
    {
        string text = "";
        for (int i = 0; i < objs.Length; ++i)
        {
            if (i == 0)
            {
                text += objs[i].ToString();
            }
            else
            {
                text += ", " + objs[i].ToString();
            }
        }
        if (Application.isPlaying)
        {
            if (mLines.Count > 20)
            {
                mLines.RemoveAt(0);
            }
            mLines.Add(text);

        }
    }
    void OnGUI()
    {
        GUI.color = Color.red;
        for (int i = 0, imax = mLines.Count; i < imax; ++i)
        {
            GUILayout.Label(mLines[i]);
        }
    }
    /*
    //方法2：按照时间进行log输出
   
    private string outpath;
    private FileStream stream ;
    private void Start()
    {
        outpath = Application.persistentDataPath + "/outlog.txt";
        if(System.IO.File.Exists(outpath))
        {
            File.Delete(outpath);
        }
        stream = new FileStream(outpath, FileMode.Append);
        Application.logMessageReceivedThreaded += logCallback;
        
    }

    void logCallback(string condition, string stackTrace, LogType type)
    {
        System.DateTime now = System.DateTime.Now;
        string content = null;
        if (type == LogType.Log)
        {
            content = string.Format("[app][{0}][{1}]:{2}\n",
                                    now.ToString("yyyy-MM-dd HH:mm:ss:fff"),
                                    type.ToString(),
                                    condition);
        }
        else
        {
            //错误信息打出栈信息
            content = string.Format("[app][{0}][{1}]:{2}\n\t{3}\n",
                                    now.ToString("yyyy-MM-dd HH:mm:ss:fff"),
                                    type.ToString(),
                                    condition,
                                    stackTrace);
        }
        writeLog(content);
    }

    void writeLog(string str)
    {
        byte[] data = Encoding.Default.GetBytes(str);
        stream.Position = stream.Length;
        stream.Write(data, 0, data.Length);
        
    }
    private void OnDestroy()
    {
        stream.Close();
    }
    */
}