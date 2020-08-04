using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngineInternal;
using Unity.UIWidgets.foundation;

public class CubeControl : MonoBehaviour,IPointerClickHandler
{
    private int LEFT_CLICK = 1;
    private int RIGHT_CLICK = 2;
    //格子状态数组 0表示格子初始状态,1表示空格子,2表示活跃地雷
    //3表示排除的地雷,4表示被点击的地雷
    enum StatusEnum { initialCube = 0, emptyCube = 1, activeMine = 2, disableMine = 3, burstMine = 4 };
    public int statusMine;
    //五种旗帜
    public GameObject tileMine;  //初始框
    public GameObject activeMine; //未排除的地雷
    public GameObject disableMine;//排掉的地雷
    public GameObject burstMine;  //爆炸的地雷
    public GameObject flagMine;   //红旗
    public GameObject textMine;   //地雷数字
    public int row, col;
    public bool isBomb;
    private Action<CubeControl> OnClick;
    public int clickType=0;

    void Start()
    {
        
    }

    public void Initial(int x,int y,Action<CubeControl> click)
    {
        statusMine = (int)StatusEnum.initialCube;
        row = x;
        col = y;
        OnClick = click;
        isBomb = false;
        tileMine.SetActive(true);
        activeMine.SetActive(false);
        disableMine.SetActive(false);
        burstMine.SetActive(false);
        flagMine.SetActive(false);
        textMine.SetActive(false);
    }

    void Update()
    {

    }

    
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("左键点击:"+row+" , "+col);
            clickType = LEFT_CLICK;
        }
        else if(eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("右键点击:" + row + " , " + col);
            clickType = RIGHT_CLICK;
        }
        else
        {
            clickType = 0;
        }

        if (clickType == LEFT_CLICK || clickType == RIGHT_CLICK)
            OnClick(this);
        
    }
}
