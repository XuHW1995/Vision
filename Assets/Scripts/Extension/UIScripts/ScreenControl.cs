using UnityEngine;

public class ScreenControl : MonoBehaviour
{
    void Start()
    {
        Screen.SetResolution(1366,768,false);
        Screen.fullScreen = false;
    } 
    void Update()
    {      
        //F10全屏 
        if (Input.GetKey(KeyCode.F10))
        {
            //获取设置当前屏幕分辩率  
            Resolution[] resolutions = Screen.resolutions;
            //设置当前分辨率  
            Screen.SetResolution(resolutions[resolutions.Length - 1].width, resolutions[resolutions.Length - 1].height, true);
            Screen.fullScreen = true;
        }
        //按ESC退出全屏  
        if (Input.GetKey(KeyCode.Escape))
        {
            Screen.SetResolution(1366, 768, false);
            Screen.fullScreen = false;  //退出全屏           
        }
    }
}
