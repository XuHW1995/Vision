using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliderControl : MonoBehaviour
{
    public Scrollbar m_Scrollbar;
    public ScrollRect m_ScrollRect;
    public Transform ToggleGroup;

    private float mTargetValue;

    private bool mNeedMove = false;

    private const float MOVE_SPEED = 2F;

    private const float SMOOTH_TIME = 0.2F;

    private float mMoveSpeed = 0f;

    private static int currentindex = 0;

    public void OnPointerDown()
    {
        mNeedMove = false;
    }

    public void OnPointerUp()
    {
        // 判断当前位于哪个区间，设置自动滑动至的位置
        if (m_Scrollbar.value <= 0.125f)
        {
            mTargetValue = 0;
            currentindex = 0;
            SetToggleOnByIndex(0);
            
        }
        else if (m_Scrollbar.value <= 0.375f)
        {
            mTargetValue = 0.25f;
            currentindex = 1;
            SetToggleOnByIndex(1);
        }
        else if (m_Scrollbar.value <= 0.625f)
        {
            mTargetValue = 0.5f;
            currentindex = 2;
            SetToggleOnByIndex(2);
        }
        else if (m_Scrollbar.value <= 0.875f)
        {
            mTargetValue = 0.75f;
            currentindex = 3;
            SetToggleOnByIndex(3);
        }
        else
        {
            mTargetValue = 1f;
            currentindex = 4;
            SetToggleOnByIndex(4);
        }

        mNeedMove = true;
        mMoveSpeed = 0;
    }

    public void OnClickLeftButton()
    {
        if (currentindex > 0 )
        {
            currentindex--;
        }
        SetToggleOnByIndex(currentindex);
    }

    public void OnClickRightButton()
    {
        if (currentindex < 4)
        {
            currentindex++;
        }
        SetToggleOnByIndex(currentindex);
    }

    public void OnButtonClick(int value)
    {
        if(ToggleGroup.GetChild(value-1).GetComponent<Toggle>().isOn == true)
        {
            switch (value)
            {
                case 1:
                    mTargetValue = 0;
                    currentindex = 0;
                    //Debug.Log(value);
                    break;
                case 2:
                    mTargetValue = 0.25f;
                    currentindex = 1;
                    //Debug.Log(value);
                    break;
                case 3:
                    mTargetValue = 0.5f;
                    currentindex = 2;
                    //Debug.Log(value);
                    break;
                case 4:
                    mTargetValue = 0.75f;
                    currentindex = 3;
                    //Debug.Log(value);
                    break;
                case 5:
                    mTargetValue = 1f;
                    currentindex = 4;
                    //Debug.Log(value);
                    break;
                default:
                    Debug.LogError("!!!!!");
                    break;
            }
            mNeedMove = true;
        }        
    }

    public void SetToggleOnByIndex(int index)
    {
        ToggleGroup.GetChild(index).GetComponent<Toggle>().isOn = true;
    }

    void Update()
    {
        if (mNeedMove)
        {
            if (Mathf.Abs(m_Scrollbar.value - mTargetValue) < 0.01f)
            {
                m_Scrollbar.value = mTargetValue;
                mNeedMove = false;
                return;
            }
            m_Scrollbar.value = Mathf.SmoothDamp(m_Scrollbar.value, mTargetValue, ref mMoveSpeed, SMOOTH_TIME);
        }
    }
}