using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[XLua.LuaCallCSharp]
public class ShowFPS : MonoBehaviour
{
    float fps1 = 0;
    float frameCount1 = 0;
    float frameTime1 = 0;


    float fps5 = 0;
    float frameCount5 = 0;
    float frameTime5 = 0;
    GUIStyle gs = new GUIStyle();

    delegate void calcFPS(ref float fps, ref float fc, ref float ft, float refreshTime);
    calcFPS calcfps;
    // Use this for initialization
    void Start()
    {
        gs.fontSize = 30;
        gs.normal.textColor = Color.red;

        calcfps = (ref float fps, ref float fc, ref float ft, float refreshTime) =>
        {
            fc++;
            ft += Time.deltaTime;
            if (ft > refreshTime)
            {
                fps = fc / ft;
                ft = 0;
                fc = 0;
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        calcfps(ref fps1, ref frameCount1, ref frameTime1, 0.2f);
        calcfps(ref fps5, ref frameCount5, ref frameTime5, 3.0f);
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 200, 100), string.Format("FPS: {0:0.00} / {1:0.00}", fps1, fps5), gs);
    }
}
