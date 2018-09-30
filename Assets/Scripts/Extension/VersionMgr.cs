/*
 *
 *
 *
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppCtrl
{
    public class VersionMgr : MonoBehaviour
    {
        public string appLocalVersion = "0.0.1";

        public string resLocalVersion = "0.0.1";

        private readonly static int[] versionCodeNumber = { 10000000, 10000, 1 };

        private readonly static int versionLenght = 3;

        public enum VersionCompareResult
        {
            eVersionError,
            eResUpdate,
            eAppUpdate,
            eNoNeedUpdate,
        }

        public class CheckUpdateResult 
        {
            public string newVersion;
            public int totalSize = 0;
            public int filesCount = 0;
            public int versionsCount = 0;
            public List<string> remoteVersionList;
            public Dictionary<string, List<string>> downloadFiles = new Dictionary<string, List<string>>();
            public string newestVersion;
            public VersionCompareResult versionCompareResult = VersionCompareResult.eVersionError;
            public string error = null;
            public bool isDone = false;
        }

        private CheckUpdateResult checkUpdateResult;

        public class DownloadResult
        {
            public int currentDownloadSize = 0;
            public string error = null;
            public bool isDone = false;
        }

        private DownloadResult downloadResult;

        private VersionUpdateConfig versionUpdateConfig;

        public struct VersionUpdateConfig
        {
            public string channel;//渠道
            public string URL;//版本下载地址
            public string sizeUrlFormat;//更新资源总大小格式
            public string fileListUrlFormat;//更新文件列表下载地址格式
            public string localVersionPathFileName;//本地版本文件地址
            public string resDownloadPathFormat;//资源文件下载地址格式
            public string localResPtah;//下载资源本地保存目录
        }

        private void Awake()
        {
            //初始化一些目录信息
            string rootUrl = XLuaMgr.instance.GUEnv.Global.GetInPath<string>("AppCtrl.AppConfig.versionMgrRootUrl");

            versionUpdateConfig.channel = XLuaMgr.instance.GUEnv.Global.GetInPath<string>("AppCtrl.AppConfig.channel");
            versionUpdateConfig.URL = rootUrl + versionUpdateConfig.channel + "/ResVersion.txt";
            versionUpdateConfig.sizeUrlFormat = rootUrl + versionUpdateConfig.channel + "/{0}/size.txt";
            versionUpdateConfig.fileListUrlFormat = rootUrl + versionUpdateConfig.channel + "/{0}/list.txt";
            versionUpdateConfig.localVersionPathFileName = XLuaMgr.instance.GUEnv.Global.GetInPath<string>("AppCtrl.AppConfig.resLocalVersionPath");
            versionUpdateConfig.resDownloadPathFormat = rootUrl + versionUpdateConfig.channel + "/{0}/{1}";
            versionUpdateConfig.localResPtah = XLuaMgr.instance.GUEnv.Global.GetInPath<string>("AppCtrl.AppConfig.localResPtah");

            InitLocalversion();
        }

        private void InitLocalversion()
        {
            //check app version
            if (!CheckVersionValid(appLocalVersion))
            {
                Debug.LogError("The app version invalid:" + appLocalVersion);
                return;
            }
            //res version;
            if (System.IO.File.Exists(versionUpdateConfig.localVersionPathFileName))
            {
                resLocalVersion = System.Text.Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(versionUpdateConfig.localVersionPathFileName));
            }
            else
            {
                System.IO.File.WriteAllBytes(versionUpdateConfig.localVersionPathFileName, System.Text.Encoding.UTF8.GetBytes(appLocalVersion));
                resLocalVersion = appLocalVersion;
            }
            if (!CheckVersionValid(resLocalVersion))
            {
                Debug.LogError("The res version invalid:" + appLocalVersion);
                return;
            }

            Debug.Log("app version: " + appLocalVersion + " res version: " + resLocalVersion);
        }

        //检查更新
        public CheckUpdateResult CheckUpdate()
        {
            checkUpdateResult = new CheckUpdateResult();
            StartCoroutine(CheckUpdateInCoroutine());
            return checkUpdateResult;
        }

        private IEnumerator CheckUpdateInCoroutine()
        {
            //从远端获取最新版本号
            yield return StartCoroutine(InitNewVersionFormNet());
            //至此，远端新版本已获取
            if (!CheckVersionValid(checkUpdateResult.newestVersion))
            {
                Debug.LogError("The new version invalid:" + appLocalVersion);
                yield break;
            }

            yield return StartCoroutine(InitNetVersionInfo());
            

            checkUpdateResult.versionCompareResult = CompareVersion(resLocalVersion, checkUpdateResult.newestVersion);
            //检查完成
            checkUpdateResult.isDone = true;
        }

        private IEnumerator InitNewVersionFormNet()
        {
            using (var www = new UnityEngine.WWW(versionUpdateConfig.URL))
            {
                yield return www;
                if (www.error != null && www.isDone)
                {
                    checkUpdateResult.remoteVersionList = VersionMgr.GetAllLines(www.bytes);
                    checkUpdateResult.newestVersion = checkUpdateResult.remoteVersionList[checkUpdateResult.remoteVersionList.Count - 1];
                }
                else
                {
                    checkUpdateResult.error = www.error + versionUpdateConfig.URL;
                    Debug.LogError("Get net version error:" + www.error);
                }
            }
        }

        private static VersionCompareResult CompareVersion(string localVersion,string newVersion)
        {
            var newVersionCode = GetVersionCode(newVersion);
            var resVersionCode = GetVersionCode(localVersion);

            var deltaCode = newVersionCode - resVersionCode;
            if(deltaCode >= versionCodeNumber[1])
            {
                return VersionCompareResult.eAppUpdate;
            }
            else if(deltaCode > 0)
            {
                return VersionCompareResult.eResUpdate;
            }
            else if (deltaCode == 0)
            {
                return VersionCompareResult.eNoNeedUpdate;
            }
            else
            {
                return VersionCompareResult.eVersionError;
            }

        }

        private static int GetVersionCode(string version)
        {
            int ret = 0;
            var verSplit = version.Split('.');

            for (int i = 0; i < verSplit.Length; i++)
            {
                ret += int.Parse(verSplit[i]) * versionCodeNumber[i];
            }

            return ret;
        }

        private static bool CheckVersionValid(string version)
        {
            var verSplit = version.Split('.');

            if (verSplit.Length != versionLenght)
            {
                return false;
            }

            foreach(var one in verSplit)
            {
                int result;
                bool isNumber = int.TryParse(one,out result);
                if (!(isNumber && result >= 0))
                {
                    return false;
                }
            }
            return true;
        }

        public IEnumerator InitNetVersionInfo()
        {
            yield return StartCoroutine(InitTotalSize());

            yield return StartCoroutine(InitTotalFileList());
        }

        private IEnumerator InitTotalSize()
        {
            int index = checkUpdateResult.remoteVersionList.IndexOf(resLocalVersion);
            //获取总的下载大小
            for (int i = index + 1; i < checkUpdateResult.remoteVersionList.Count; i++)
            {
                string sizeUrl = string.Format(versionUpdateConfig.sizeUrlFormat, checkUpdateResult.remoteVersionList[i]);
                using (WWW www = new WWW(sizeUrl))
                {
                    yield return www;

                    if (www.error == null && www.isDone)
                    {
                        checkUpdateResult.totalSize += System.Convert.ToInt32(System.Text.Encoding.UTF8.GetString(www.bytes));
                    }
                    else
                    {
                       checkUpdateResult.error = www.error + sizeUrl;
                    }
                   
                }
            }
        }

        private IEnumerator InitTotalFileList()
        {
            int index = checkUpdateResult.remoteVersionList.IndexOf(resLocalVersion);
            for (int i = index + 1; i < checkUpdateResult.remoteVersionList.Count; i++)
            {
                var version = checkUpdateResult.remoteVersionList[i];
                string listUrl = string.Format(versionUpdateConfig.fileListUrlFormat, checkUpdateResult.remoteVersionList[i]);
                using (WWW list = new WWW(listUrl))
                {
                    yield return list;
                    if (list.error != null || !list.isDone)
                    {
                        checkUpdateResult.error = list.error + listUrl;

                    }
                    else
                    {
                        checkUpdateResult.downloadFiles[version] = VersionMgr.GetAllLines(list.bytes);
                        checkUpdateResult.filesCount += checkUpdateResult.downloadFiles[version].Count;
                    }
                }
            }
            //需要更新的版本个数
            checkUpdateResult.versionsCount = checkUpdateResult.downloadFiles.Count;
        }

        private static List<string> GetAllLines(byte[] data)
        {
            string[] dlist = System.Text.Encoding.UTF8.GetString(data).Split('\n');
            List<string> ret = new List<string>();
            ret.AddRange(dlist);

            for (int i = 0; i < ret.Count; i++)
            {
                //容错文件操作用的"rb"和"r"的模式
                if (ret[i].LastIndexOf('\r') >= 0)
                {
                    ret[i] = ret[i].Remove(ret[i].LastIndexOf('\r'));
                }
                //Debug.Log("ResUpdate download file:" + dlist[i]);
                if (ret[i].Length <= 0)
                {
                    ret.RemoveAt(i);
                }
            }

            return ret;

        }


        

        public DownloadResult DownloadRes(Dictionary<string, List<string>> downloadFiles)
        {
            downloadResult = new DownloadResult();

            StartCoroutine(DownloadResCoroutine(downloadFiles));

            return downloadResult;
        }

        public IEnumerator DownloadResCoroutine(Dictionary<string, List<string>> downloadFiles)
        {
            //逐个下载所有文件
            foreach (var oneVersion in downloadFiles)
            {
                foreach (var oneFile in oneVersion.Value)
                {
                    yield return StartCoroutine(downloadFile(oneFile, oneVersion.Key));
                }
            }
            downloadResult.isDone = true;
        }


        private IEnumerator downloadFile(string filename, string version)
        {
            string fuf = string.Format(versionUpdateConfig.resDownloadPathFormat, version, filename);

            using (WWW www = new WWW(fuf))
            {
                yield return www;
                if (www.error != null || !www.isDone)
                {
                    downloadResult.error = www.error + "[filename:]" + fuf;
                }
                try
                {
                    System.IO.File.WriteAllBytes(System.IO.Path.Combine(Application.persistentDataPath, filename), www.bytes);
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Exception on function downloadFile: " + Application.persistentDataPath + " " + filename);
                }
                downloadResult.currentDownloadSize += www.bytes.Length;
            }
        }

    }
}
