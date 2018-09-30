using UnityEditor;
using UnityEngine;
using System.IO;
using CSObjectWrapEditor;

namespace EditorTools
{
    //路径规划
    public class PathConfig
    {
        public static string[] MAINSCENE = { "Assets/Res/Scenes/Main.unity" };
        public static string PROJECT = Application.dataPath + "/..";
        public static string FRAMEWORK = Application.dataPath + "/Res";
        public static string SCRIPTS = Application.dataPath + "/Scripts";
        //Assetbundle存放路径
        public static string AB = Application.streamingAssetsPath;
        //资源路径
        static public string PREFAB = Application.dataPath + "/Res/Prefabs";
        static public string IMAGE = Application.dataPath + "/Res/Images";
        static public string SCENE = Application.dataPath + "/Res/Scenes";
        static public string MATERIAL = Application.dataPath + "/Res/Materials";
        static public string FONT = Application.dataPath + "/Res/Font";
        static public string FBX = Application.dataPath + "/Res/FBX";
        static public string LUA = Application.dataPath + "/Scripts/Lua";
        static public string LUATXT = Application.dataPath + "/Scripts/Luatxt";
        static public string ANIMATION = Application.dataPath + "/Res/Animation";
        static public string PBCFILE = Application.dataPath + "/Res/Config";
        public static string LuatxtPackagePath = Application.streamingAssetsPath + "/lua/" + "luatxt.ab";
        //行为树xml数据存放路径
        static public string BEHAVIORTREE = Application.dataPath + "/Scripts/CSharp/AllPlugins/behaviac/Exported/";
        //Resources目录
        static public string RESOURCES = Application.dataPath + "/Resources";
        //输出路径
        public static string PROJECT_IOS_Xcode = Application.dataPath + "/../ExportProject/IOS/Xcodeproject";
        public static string PROJECT_ANDROID = Application.dataPath + "/../ExportProject/Android/";
        public static string PROJECT_ANDROID_P = Application.dataPath + "/../ExportProject/Android/" + PlayerSettings.productName;
        public static string PROJECT_ANDROIDSTUDIO = Application.dataPath + "/../ExportProject/Android/proj.android-studio";
        public static string PROJECT_WIN = Application.dataPath + "/../ExportProject/Windows/";
        public static string PRODUCT_NAME = PlayerSettings.productName;
    }
    //资源打包
    public class makeproj
    {
        //-------------------------三平台打包-------------------------------------------------
        //打安卓APK
        public static void BuildAndroidProject(string version, bool bCleanCache )
        {
            //生成Xlua代码
            Generator.GenAll();
            if (Directory.Exists(PathConfig.PROJECT_ANDROID_P))
            {
                Directory.Delete(PathConfig.PROJECT_ANDROID_P, true);
            }
            if (bCleanCache)
                CleanABCache();
            //.lua转换为.txt
            Tools.LuatoTxt();
            //拷贝行为树xml文件
            Tools.CopyXmlFolder();
            //刷新界面
            AssetDatabase.Refresh();
            //为资源打标签
            SetAllImageAssetBundle();
            SetAllPrefabAssetBundle();
            SetAllSceneAssetBundle();
            SetAllMaterialAssetBundle();
            SetAllFontAssetBundle();
            SetAllLuaAssetBundle();
            SetAllAnimationAssetBundle();
            SetAllPbcAssetbundle();
            //设置版本号
            setAndroidVersion(version);
            
            //打包Assetbundle
            BuildPipeline.BuildAssetBundles(PathConfig.AB, BuildAssetBundleOptions.None, BuildTarget.Android);           
            //打包APK 
            BuildPipeline.BuildPlayer(PathConfig.MAINSCENE, PathConfig.PROJECT_ANDROID + PlayerSettings.productName + ".apk", BuildTarget.Android, BuildOptions.None);
            //删除Xlua代码
            Generator.ClearAll();
        }
        //打IOS，Xcode工程
         public static void BuildIOSProject(string version, bool bCleanCache )
        {
            //生成Xlua代码
            Generator.GenAll();
            if (Directory.Exists(PathConfig.PROJECT_IOS_Xcode))
            {
                Directory.Delete(PathConfig.PROJECT_IOS_Xcode,true);
            }
            if (bCleanCache)
                CleanABCache();
            //.lua转换为.txt
            Tools.LuatoTxt();
            //拷贝行为树xml文件
            Tools.CopyXmlFolder();
            //刷新界面
            AssetDatabase.Refresh();
            //为资源打标签
            SetAllImageAssetBundle();
            SetAllPrefabAssetBundle();
            SetAllSceneAssetBundle();
            SetAllMaterialAssetBundle();
            SetAllFontAssetBundle();
            SetAllLuaAssetBundle();
            SetAllAnimationAssetBundle();
            SetAllPbcAssetbundle();
            //打包Assetbundle
            BuildPipeline.BuildAssetBundles(PathConfig.AB, BuildAssetBundleOptions.None, BuildTarget.iOS);
            //打包Xcode工程
            BuildPipeline.BuildPlayer(PathConfig.MAINSCENE, PathConfig.PROJECT_IOS_Xcode, BuildTarget.iOS, BuildOptions.None);
            //删除Xlua代码
            Generator.ClearAll();
        }
        //打Windows工程exe
         public static void BuildWINProject(string version, bool bCleanCache )
        {
            //生成Xlua代码
            Generator.GenAll();
            if (Directory.Exists(PathConfig.PROJECT_WIN))
            {
              Directory.Delete(PathConfig.PROJECT_WIN, true);
            }
            //清理AB文件
            if (bCleanCache)
                CleanABCache();
            //.lua转换为.txt
            Tools.LuatoTxt();
            //拷贝行为树xml文件
            Tools.CopyXmlFolder();
            //刷新界面
            AssetDatabase.Refresh();           
            //为资源打标签
            SetAllImageAssetBundle();
            SetAllPrefabAssetBundle();
            SetAllSceneAssetBundle();
            SetAllMaterialAssetBundle();
            SetAllFontAssetBundle();
            SetAllLuaAssetBundle();
            SetAllAnimationAssetBundle();
            SetAllPbcAssetbundle();
            //把所有有标记的资源打成Assetbundle
            BuildPipeline.BuildAssetBundles(PathConfig.AB, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
            //生成Rmworld.exe
            BuildPipeline.BuildPlayer( PathConfig.MAINSCENE  , PathConfig.PROJECT_WIN +  PathConfig.PRODUCT_NAME + ".exe", BuildTarget.StandaloneWindows, BuildOptions.Development | BuildOptions.ConnectWithProfiler | BuildOptions.AllowDebugging);
            //生成windwos版本文件
            File.WriteAllText(PathConfig.PROJECT_WIN + "/WinVersion.txt", version);
            //删除Xlua代码
            Generator.ClearAll();
        }        
        //-----------------------------资源命名-----------------------------------------------
        //字体命名
        static void SetAllFontAssetBundle()
        {
            if (!Directory.Exists(PathConfig.FONT))
            {
                Debug.Log("Font 目录不存在");
                return;
            }
            
            string[] lookFor = new string[] { GetRelativePath(PathConfig.PROJECT, PathConfig.FONT) };
            foreach (var guid in AssetDatabase.FindAssets("t:Font", lookFor))
            {
                var fontPath = AssetDatabase.GUIDToAssetPath(guid);
                var assetImporter = AssetImporter.GetAtPath(fontPath) as AssetImporter;
                var fontName = Path.GetDirectoryName(GetRelativePath(PathConfig.FRAMEWORK, fontPath));
                Debug.Log(fontName);
                assetImporter.assetBundleName = Path.ChangeExtension(fontName, "ab");
                assetImporter.SaveAndReimport();
            }
            Debug.Log("设置Font AssetBundle 完成");
        }     
        //预设命名
        static void SetAllPrefabAssetBundle()
        {
            if (!Directory.Exists(PathConfig.PREFAB))
            {
                Debug.Log("Prefabs 目录不存在");
                return;
            }
            string[] lookFor = new string[] { GetRelativePath(PathConfig.PROJECT, PathConfig.PREFAB) };
            foreach (var guid in AssetDatabase.FindAssets("t:prefab", lookFor))
            {
                var prefabPath = AssetDatabase.GUIDToAssetPath(guid);
                var assetImporter = AssetImporter.GetAtPath(prefabPath);
                var relativePath = GetRelativePath(PathConfig.FRAMEWORK, prefabPath);
                Debug.Log(relativePath);
                assetImporter.assetBundleName = Path.ChangeExtension(relativePath, "ab");
                assetImporter.SaveAndReimport();
            }
            Debug.Log("设置Prefab AssetBundle完成");
        }
        //图片命名
        static void SetAllImageAssetBundle()
        {
            if (!Directory.Exists(PathConfig.IMAGE))
            {
                Debug.Log("Images 目录不存在");
                return;
            }
            string[] lookFor = new string[] { GetRelativePath(PathConfig.PROJECT, PathConfig.IMAGE) };
            foreach (var guid in AssetDatabase.FindAssets("t:texture2D", lookFor))
            {
                //从GUID获取
                var imagePath = AssetDatabase.GUIDToAssetPath(guid);
                var textureImporter = TextureImporter.GetAtPath(imagePath) as TextureImporter;
                var atlasName = Path.GetDirectoryName(GetRelativePath(PathConfig.FRAMEWORK, imagePath));
                Debug.Log(atlasName);
                // 这里保证 Packing Tag 与 AssetBundle Name 保持一致，便于理解
                // AssetBundle Name 会被自动改为小写，Packing Tag 在这里手动处理下
                textureImporter.spritePackingTag = atlasName.ToLowerInvariant();
                textureImporter.assetBundleName = atlasName + ".ab";
                textureImporter.SaveAndReimport();
            }
            Debug.Log("设置Image AssetBundle完成");
        }
        //场景命名
        static void SetAllSceneAssetBundle()
        {
            if (!Directory.Exists(PathConfig.SCENE))
            {
                Debug.Log("Scene 目录不存在");
                return;
            }
            string[] lookFor = new string[] { GetRelativePath(PathConfig.PROJECT, PathConfig.SCENE) };
            foreach (var guid in AssetDatabase.FindAssets("t:scene", lookFor))
            {
                var scenePath = AssetDatabase.GUIDToAssetPath(guid);
                var assetImporter = AssetImporter.GetAtPath(scenePath);
                var relativePath = GetRelativePath(PathConfig.FRAMEWORK, scenePath);
                Debug.Log(relativePath);
                assetImporter.assetBundleName = Path.ChangeExtension(relativePath, "ab");
                assetImporter.SaveAndReimport();
            }
            Debug.Log("设置Scene AssetBundle完成");
        }
        //材质命名
        static void SetAllMaterialAssetBundle()
        {
            if (!Directory.Exists(PathConfig.MATERIAL))
            {
                Debug.Log("Material目录不存在");
                return;
            }
            
            string[] lookFor = new string[] { GetRelativePath(PathConfig.PROJECT, PathConfig.MATERIAL) };
            foreach (var guid in AssetDatabase.FindAssets("t:material", lookFor))
            {
                var materialPath = AssetDatabase.GUIDToAssetPath(guid);
                var assetImporter = AssetImporter.GetAtPath(materialPath);
                var relativePath = GetRelativePath(PathConfig.FRAMEWORK, materialPath);
                Debug.Log(relativePath);
                assetImporter.assetBundleName = Path.ChangeExtension(relativePath, "ab");
                assetImporter.SaveAndReimport();
            }
            Debug.Log("设置materialPath AssetBundle完成");
        }
        //lua命名
        static void SetAllLuaAssetBundle()
        {
            if (!Directory.Exists(PathConfig.LUATXT))
            {
                Debug.Log("Lua目录不存在");
                return;
            }
            
            string[] lookFor = new string[] { GetRelativePath(PathConfig.PROJECT, PathConfig.LUATXT) };
            Debug.Log(AssetDatabase.FindAssets("t:TextAsset", lookFor));
            foreach (var guid in AssetDatabase.FindAssets("t:TextAsset", lookFor))
            {
                var luaPath = AssetDatabase.GUIDToAssetPath(guid);
                var assetImporter = AssetImporter.GetAtPath(luaPath);
                //要单独一个.lua打成一个ab用的
                //var relativePath = GetRelativePath(PathConfig.SCRIPTS, luaPath).Replace("_","/");
                //assetImporter.assetBundleName = Path.ChangeExtension(relativePath, "ab");
                assetImporter.assetBundleName = Path.ChangeExtension("lua/luatxt", "ab");
                assetImporter.SaveAndReimport();
            }
            Debug.Log("设置Lua AssetBundle完成");

        }
        //动画资源命名
        static void SetAllAnimationAssetBundle()
        {
            if (!Directory.Exists(PathConfig.ANIMATION))
            {
                Debug.Log("Animation 目录不存在");
                return;
            }
            string[] lookFor = new string[] { GetRelativePath(PathConfig.PROJECT, PathConfig.ANIMATION) };
            foreach (var guid in AssetDatabase.FindAssets("t:ScriptableObject", lookFor))
            {
                var animationPath = AssetDatabase.GUIDToAssetPath(guid);
                var animationImporter = AssetImporter.GetAtPath(animationPath);
                var atlasName = Path.GetDirectoryName(GetRelativePath(PathConfig.FRAMEWORK, animationPath));
                Debug.Log(atlasName);
                animationImporter.assetBundleName = atlasName + ".ab";
                animationImporter.SaveAndReimport();
            }
            Debug.Log("Animation AssetBundle完成");
        }
        //pbcfile命名
        public static void SetAllPbcAssetbundle()
        {
            if (!Directory.Exists(PathConfig.PBCFILE))
            {
                Debug.Log("PBCFILE 目录不存在");
                return;
            }
            string[] lookFor = new string[] { GetRelativePath(PathConfig.PROJECT, PathConfig.PBCFILE) };
            foreach (var guid in AssetDatabase.FindAssets("t:TextAsset", lookFor))
            {
                var pbcfilePath = AssetDatabase.GUIDToAssetPath(guid);
                var assetImporter = AssetImporter.GetAtPath(pbcfilePath);
                var relativePath = GetRelativePath(PathConfig.FRAMEWORK, pbcfilePath);
                Debug.Log(relativePath);
                assetImporter.assetBundleName = Path.ChangeExtension(relativePath, "ab");
                assetImporter.SaveAndReimport();
            }
            Debug.Log("设置PbcFile AssetBundle完成");
        }
        //-----------------------------------------------------------------------------------
        //清理AB和Resources文件
        static void CleanABCache()
        {
            if (Directory.Exists(PathConfig.AB))
            {
                Directory.Delete(PathConfig.AB, true);
            }
            Directory.CreateDirectory(PathConfig.AB);
            if (Directory.Exists(PathConfig.RESOURCES))
            {
                Directory.Delete(PathConfig.RESOURCES, true);
            }
            Directory.CreateDirectory(PathConfig.RESOURCES);
            Debug.Log("清理完成。");
        }
        //外接SDK需要：导出安卓工程
        static void makeAndroidStudioProject()
        {
            string from, to;
            //AndroidManifest.xml
            from = PathConfig.PROJECT_ANDROID_P + "/AndroidManifest.xml";
            to = PathConfig.PROJECT_ANDROIDSTUDIO + "/app/src/main/AndroidManifest.xml";
            Tools.CopyFile(from, to);
            //libs arm
            from = PathConfig.PROJECT_ANDROID_P + "/libs/armeabi-v7a/";
            to = PathConfig.PROJECT_ANDROIDSTUDIO + "/app/src/main/jniLibs/armeabi-v7a/";
            Tools.CopyFolder(from,to);
            //libs x86
            from = PathConfig.PROJECT_ANDROID_P + "/libs/x86/";
            to = PathConfig.PROJECT_ANDROIDSTUDIO + "/app/src/main/jniLibs/x86/";
            Tools.CopyFolder(from, to);
            //libs unity jar
            from = PathConfig.PROJECT_ANDROID_P + "/libs/unity-classes.jar";
            to = PathConfig.PROJECT_ANDROIDSTUDIO + "/app/libs/unity-classes.jar";
            Tools.CopyFile(from, to);
            //res
            from = PathConfig.PROJECT_ANDROID_P + "/res/";
            to = PathConfig.PROJECT_ANDROIDSTUDIO + "/app/src/main/res/";
            Tools.CopyFolder(from, to);
            //src
            from = PathConfig.PROJECT_ANDROID_P + "/src/";
            to = PathConfig.PROJECT_ANDROIDSTUDIO + "/app/src/main/java/";
            Tools.CopyFolder(from, to);
            //assets
            from = PathConfig.PROJECT_ANDROID_P + "/assets/";
            to = PathConfig.PROJECT_ANDROIDSTUDIO + "/app/src/main/assets/";
            Tools.CopyFolder(from, to);
        } 
        //设置安卓版本号
        static void setAndroidVersion(string version)
        {
            PlayerSettings.bundleVersion = version;
            PlayerSettings.Android.bundleVersionCode = GetVersionCode(version);
        }
        //设置IOS版本号
        static void setIosVersion(string version)
        {
            PlayerSettings.bundleVersion = version;
        }
        //版本号和Code转换用来比较
        static public int GetVersionCode(string version)
        {
            const int verLen = 3;
            int ret = 0;
            int[] codenum = new int[verLen] { 10000000, 10000, 1 };
            string[] versplit = version.Split('.');
            if (versplit.Length != verLen)
            {
                return -1;
            }
            for (int i = 0; i < verLen; i++)
            {
                ret += System.Convert.ToInt32(versplit[i]) * codenum[i];
            }
            return ret;
        }
        //获取相对路径
        static public string GetRelativePath(string root, string fullPath)
        {
            root = Path.GetFullPath(root).Replace('\\','/'); 
            fullPath = Path.GetFullPath(fullPath).Replace('\\', '/');
            return fullPath.Substring(root.Length + 1);
        }
    }
}
