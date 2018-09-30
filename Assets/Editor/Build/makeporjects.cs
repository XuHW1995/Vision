using UnityEditor;
using UnityEngine;
using System.IO;
using CSObjectWrapEditor;

namespace EditorTools
{
    //·���滮
    public class PathConfig
    {
        public static string[] MAINSCENE = { "Assets/Res/Scenes/Main.unity" };
        public static string PROJECT = Application.dataPath + "/..";
        public static string FRAMEWORK = Application.dataPath + "/Res";
        public static string SCRIPTS = Application.dataPath + "/Scripts";
        //Assetbundle���·��
        public static string AB = Application.streamingAssetsPath;
        //��Դ·��
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
        //��Ϊ��xml���ݴ��·��
        static public string BEHAVIORTREE = Application.dataPath + "/Scripts/CSharp/AllPlugins/behaviac/Exported/";
        //ResourcesĿ¼
        static public string RESOURCES = Application.dataPath + "/Resources";
        //���·��
        public static string PROJECT_IOS_Xcode = Application.dataPath + "/../ExportProject/IOS/Xcodeproject";
        public static string PROJECT_ANDROID = Application.dataPath + "/../ExportProject/Android/";
        public static string PROJECT_ANDROID_P = Application.dataPath + "/../ExportProject/Android/" + PlayerSettings.productName;
        public static string PROJECT_ANDROIDSTUDIO = Application.dataPath + "/../ExportProject/Android/proj.android-studio";
        public static string PROJECT_WIN = Application.dataPath + "/../ExportProject/Windows/";
        public static string PRODUCT_NAME = PlayerSettings.productName;
    }
    //��Դ���
    public class makeproj
    {
        //-------------------------��ƽ̨���-------------------------------------------------
        //��׿APK
        public static void BuildAndroidProject(string version, bool bCleanCache )
        {
            //����Xlua����
            Generator.GenAll();
            if (Directory.Exists(PathConfig.PROJECT_ANDROID_P))
            {
                Directory.Delete(PathConfig.PROJECT_ANDROID_P, true);
            }
            if (bCleanCache)
                CleanABCache();
            //.luaת��Ϊ.txt
            Tools.LuatoTxt();
            //������Ϊ��xml�ļ�
            Tools.CopyXmlFolder();
            //ˢ�½���
            AssetDatabase.Refresh();
            //Ϊ��Դ���ǩ
            SetAllImageAssetBundle();
            SetAllPrefabAssetBundle();
            SetAllSceneAssetBundle();
            SetAllMaterialAssetBundle();
            SetAllFontAssetBundle();
            SetAllLuaAssetBundle();
            SetAllAnimationAssetBundle();
            SetAllPbcAssetbundle();
            //���ð汾��
            setAndroidVersion(version);
            
            //���Assetbundle
            BuildPipeline.BuildAssetBundles(PathConfig.AB, BuildAssetBundleOptions.None, BuildTarget.Android);           
            //���APK 
            BuildPipeline.BuildPlayer(PathConfig.MAINSCENE, PathConfig.PROJECT_ANDROID + PlayerSettings.productName + ".apk", BuildTarget.Android, BuildOptions.None);
            //ɾ��Xlua����
            Generator.ClearAll();
        }
        //��IOS��Xcode����
         public static void BuildIOSProject(string version, bool bCleanCache )
        {
            //����Xlua����
            Generator.GenAll();
            if (Directory.Exists(PathConfig.PROJECT_IOS_Xcode))
            {
                Directory.Delete(PathConfig.PROJECT_IOS_Xcode,true);
            }
            if (bCleanCache)
                CleanABCache();
            //.luaת��Ϊ.txt
            Tools.LuatoTxt();
            //������Ϊ��xml�ļ�
            Tools.CopyXmlFolder();
            //ˢ�½���
            AssetDatabase.Refresh();
            //Ϊ��Դ���ǩ
            SetAllImageAssetBundle();
            SetAllPrefabAssetBundle();
            SetAllSceneAssetBundle();
            SetAllMaterialAssetBundle();
            SetAllFontAssetBundle();
            SetAllLuaAssetBundle();
            SetAllAnimationAssetBundle();
            SetAllPbcAssetbundle();
            //���Assetbundle
            BuildPipeline.BuildAssetBundles(PathConfig.AB, BuildAssetBundleOptions.None, BuildTarget.iOS);
            //���Xcode����
            BuildPipeline.BuildPlayer(PathConfig.MAINSCENE, PathConfig.PROJECT_IOS_Xcode, BuildTarget.iOS, BuildOptions.None);
            //ɾ��Xlua����
            Generator.ClearAll();
        }
        //��Windows����exe
         public static void BuildWINProject(string version, bool bCleanCache )
        {
            //����Xlua����
            Generator.GenAll();
            if (Directory.Exists(PathConfig.PROJECT_WIN))
            {
              Directory.Delete(PathConfig.PROJECT_WIN, true);
            }
            //����AB�ļ�
            if (bCleanCache)
                CleanABCache();
            //.luaת��Ϊ.txt
            Tools.LuatoTxt();
            //������Ϊ��xml�ļ�
            Tools.CopyXmlFolder();
            //ˢ�½���
            AssetDatabase.Refresh();           
            //Ϊ��Դ���ǩ
            SetAllImageAssetBundle();
            SetAllPrefabAssetBundle();
            SetAllSceneAssetBundle();
            SetAllMaterialAssetBundle();
            SetAllFontAssetBundle();
            SetAllLuaAssetBundle();
            SetAllAnimationAssetBundle();
            SetAllPbcAssetbundle();
            //�������б�ǵ���Դ���Assetbundle
            BuildPipeline.BuildAssetBundles(PathConfig.AB, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
            //����Rmworld.exe
            BuildPipeline.BuildPlayer( PathConfig.MAINSCENE  , PathConfig.PROJECT_WIN +  PathConfig.PRODUCT_NAME + ".exe", BuildTarget.StandaloneWindows, BuildOptions.Development | BuildOptions.ConnectWithProfiler | BuildOptions.AllowDebugging);
            //����windwos�汾�ļ�
            File.WriteAllText(PathConfig.PROJECT_WIN + "/WinVersion.txt", version);
            //ɾ��Xlua����
            Generator.ClearAll();
        }        
        //-----------------------------��Դ����-----------------------------------------------
        //��������
        static void SetAllFontAssetBundle()
        {
            if (!Directory.Exists(PathConfig.FONT))
            {
                Debug.Log("Font Ŀ¼������");
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
            Debug.Log("����Font AssetBundle ���");
        }     
        //Ԥ������
        static void SetAllPrefabAssetBundle()
        {
            if (!Directory.Exists(PathConfig.PREFAB))
            {
                Debug.Log("Prefabs Ŀ¼������");
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
            Debug.Log("����Prefab AssetBundle���");
        }
        //ͼƬ����
        static void SetAllImageAssetBundle()
        {
            if (!Directory.Exists(PathConfig.IMAGE))
            {
                Debug.Log("Images Ŀ¼������");
                return;
            }
            string[] lookFor = new string[] { GetRelativePath(PathConfig.PROJECT, PathConfig.IMAGE) };
            foreach (var guid in AssetDatabase.FindAssets("t:texture2D", lookFor))
            {
                //��GUID��ȡ
                var imagePath = AssetDatabase.GUIDToAssetPath(guid);
                var textureImporter = TextureImporter.GetAtPath(imagePath) as TextureImporter;
                var atlasName = Path.GetDirectoryName(GetRelativePath(PathConfig.FRAMEWORK, imagePath));
                Debug.Log(atlasName);
                // ���ﱣ֤ Packing Tag �� AssetBundle Name ����һ�£��������
                // AssetBundle Name �ᱻ�Զ���ΪСд��Packing Tag �������ֶ�������
                textureImporter.spritePackingTag = atlasName.ToLowerInvariant();
                textureImporter.assetBundleName = atlasName + ".ab";
                textureImporter.SaveAndReimport();
            }
            Debug.Log("����Image AssetBundle���");
        }
        //��������
        static void SetAllSceneAssetBundle()
        {
            if (!Directory.Exists(PathConfig.SCENE))
            {
                Debug.Log("Scene Ŀ¼������");
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
            Debug.Log("����Scene AssetBundle���");
        }
        //��������
        static void SetAllMaterialAssetBundle()
        {
            if (!Directory.Exists(PathConfig.MATERIAL))
            {
                Debug.Log("MaterialĿ¼������");
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
            Debug.Log("����materialPath AssetBundle���");
        }
        //lua����
        static void SetAllLuaAssetBundle()
        {
            if (!Directory.Exists(PathConfig.LUATXT))
            {
                Debug.Log("LuaĿ¼������");
                return;
            }
            
            string[] lookFor = new string[] { GetRelativePath(PathConfig.PROJECT, PathConfig.LUATXT) };
            Debug.Log(AssetDatabase.FindAssets("t:TextAsset", lookFor));
            foreach (var guid in AssetDatabase.FindAssets("t:TextAsset", lookFor))
            {
                var luaPath = AssetDatabase.GUIDToAssetPath(guid);
                var assetImporter = AssetImporter.GetAtPath(luaPath);
                //Ҫ����һ��.lua���һ��ab�õ�
                //var relativePath = GetRelativePath(PathConfig.SCRIPTS, luaPath).Replace("_","/");
                //assetImporter.assetBundleName = Path.ChangeExtension(relativePath, "ab");
                assetImporter.assetBundleName = Path.ChangeExtension("lua/luatxt", "ab");
                assetImporter.SaveAndReimport();
            }
            Debug.Log("����Lua AssetBundle���");

        }
        //������Դ����
        static void SetAllAnimationAssetBundle()
        {
            if (!Directory.Exists(PathConfig.ANIMATION))
            {
                Debug.Log("Animation Ŀ¼������");
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
            Debug.Log("Animation AssetBundle���");
        }
        //pbcfile����
        public static void SetAllPbcAssetbundle()
        {
            if (!Directory.Exists(PathConfig.PBCFILE))
            {
                Debug.Log("PBCFILE Ŀ¼������");
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
            Debug.Log("����PbcFile AssetBundle���");
        }
        //-----------------------------------------------------------------------------------
        //����AB��Resources�ļ�
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
            Debug.Log("������ɡ�");
        }
        //���SDK��Ҫ��������׿����
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
        //���ð�׿�汾��
        static void setAndroidVersion(string version)
        {
            PlayerSettings.bundleVersion = version;
            PlayerSettings.Android.bundleVersionCode = GetVersionCode(version);
        }
        //����IOS�汾��
        static void setIosVersion(string version)
        {
            PlayerSettings.bundleVersion = version;
        }
        //�汾�ź�Codeת�������Ƚ�
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
        //��ȡ���·��
        static public string GetRelativePath(string root, string fullPath)
        {
            root = Path.GetFullPath(root).Replace('\\','/'); 
            fullPath = Path.GetFullPath(fullPath).Replace('\\', '/');
            return fullPath.Substring(root.Length + 1);
        }
    }
}
