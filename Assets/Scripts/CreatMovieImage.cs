using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.IO;
//using UnityEditor;

public class CreatMovieImage : MonoBehaviour {

    public VideoPlayer vp;
    public Image showImage;

    private List<VideoStruct> allVideoData;
    private int snippTime = 20;

    //是否同时存时间
    private bool isSaveTime = false;
    //存图的过程中是否实时显示
    private bool isShow = false;

    void Start () {
        allVideoData = MySearch.SearchByInputText("");

        Debug.Log("视频总数：" + allVideoData.Count);
        StartCoroutine(Snipping());
    }

    IEnumerator Snipping()
    {
        for (int i = 4561; i <= allVideoData.Count; i++)
        {
            Debug.Log(i + "  正在截图.....");
            vp.url = allVideoData[i].url;
            vp.Prepare();

            //等待视频跳到截图时间
            while (vp.time < snippTime - 1)
            {
                vp.time = snippTime;
                yield return new WaitForSeconds(1);//延迟1S，时间设定需要时间 
                yield return null;
            }

            StartCoroutine(CreatImage(vp.texture));
        }       
    }

    IEnumerator CreatImage(Texture image)
    {
        Texture2D videoFrameTexture = new Texture2D(image.width, image.height);
        UnityEngine.RenderTexture.active = (RenderTexture)image;
        Rect temprect = new Rect(0, 0, image.width, image.height);
        videoFrameTexture.ReadPixels(temprect, 0, 0);
        videoFrameTexture.Apply();

        if (isShow)
        {
            Sprite tempsprite = Sprite.Create(videoFrameTexture, temprect, new Vector2(0, 0));
            showImage.sprite = tempsprite;
        } 

        StartCoroutine(SaveTexture(videoFrameTexture, vp.url));
        yield return null;
    }

    IEnumerator SaveTexture(Texture2D source, string videourl)
    {
        Texture2D result = new Texture2D(400, 200, TextureFormat.RGB24, false);

        for (int i = 0; i < result.height; ++i)
        {
            for (int j = 0; j < result.width; ++j)
            {
                Color newColor = source.GetPixelBilinear((float)j / (float)result.width, (float)i / (float)result.height);
                result.SetPixel(j, i, newColor);
            }
        }
        result.Apply();
        
        string savePath = ReadAndSaveVideoInfo.GetImageSavePathByUrl(videourl);
        if (!Directory.Exists(Path.GetDirectoryName(savePath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
        }
        
        File.WriteAllBytes(savePath, result.EncodeToJPG(30));
        Resources.UnloadUnusedAssets();

        if (isSaveTime)
        {
            Debug.Log(videourl + "视频时长" + (int)System.Math.Ceiling(vp.frameCount / vp.frameRate));
            ReadAndSaveVideoInfo.SaveVideoTime(videourl, (int)System.Math.Ceiling(vp.frameCount / vp.frameRate));
        }

        //AssetDatabase.Refresh();
        yield return null;
        //Debug.Log("SaveSuccess!!!  Save Path is : " + savePath);
    }
}
