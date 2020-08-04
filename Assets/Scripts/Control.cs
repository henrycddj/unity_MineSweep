using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Control : MonoBehaviour
{
    //格子状态数组 0表示格子初始状态,1表示空格子,2表示活跃地雷
    //3表示排除的地雷,4表示被点击的地雷
    enum StatusEnum { initialCube = 0, emptyCube = 1, activeMine = 2, disableMine = 3, burstMine = 4 };
    private GameObject[] objs;
    public CubeControl[] Cubes; //格子数组
    public SmileControl Smile;   //笑脸对象
    public int cubeCount;      //格子数量
    public int rowNum = 8;
    public int colNum = 15;
    public int mineCount=20;   //地雷数量
    private int flagNum;
    private int LEFT_CLICK = 1;  //左键标识
    private int RIGHT_CLICK = 2; //右键标识
    
    // Update is called once per frame
    void Update()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        gameInitial();
        
    }

    //游戏初始化
    public void gameInitial()
    {
        objs = GameObject.FindGameObjectsWithTag("Cube");
        Cubes = new CubeControl[objs.Length];
        for(int i=0;i<objs.Length;++i)
        {
            Cubes[i] = GameObject.Find("Cube "+"("+i+")").GetComponent<CubeControl>();
        }
        cubeCount = Cubes.Length;
        for (int i = 0; i < Cubes.Length; ++i)
        {
            Cubes[i].Initial(i / colNum, i % colNum, OnClick);
        }
        randomMine();
        for (int i = 0; i < Cubes.Length; ++i)
        {
            if (Cubes[i].isBomb == true)
                Cubes[i].statusMine = (int)StatusEnum.activeMine;
        }
        flagNum = mineCount;
    }

    //地雷随机到图中
    private void randomMine()
    {
        for (int i = 0; i < mineCount; ++i)
            Cubes[i].isBomb = true;
        for(int i = 0; i < mineCount; ++i)
        {
            bool temp;
            int tempint=UnityEngine.Random.Range(0,Cubes.Length);
            temp = Cubes[tempint].isBomb;
            Cubes[tempint].isBomb = Cubes[i].isBomb;
            Cubes[i].isBomb = temp;
        }
    }
    

    //鼠标点击了 一个cube
    public void OnClick(CubeControl temp)
    {
        Debug.Log("此类型:" + temp.statusMine.ToString());
        //左键
        if (temp.clickType==LEFT_CLICK)
        {
            //若是初始状态
            if (temp.statusMine == (int)StatusEnum.initialCube)
            {
                    expand(temp.row, temp.col);
            }
            else if (temp.statusMine == (int)StatusEnum.activeMine)
            {
                temp.statusMine = (int)StatusEnum.burstMine;
                gameOver();
            }
        }
        //标旗
        if(temp.clickType==RIGHT_CLICK)
        {
            if (temp.statusMine == (int)StatusEnum.initialCube || temp.statusMine ==(int)StatusEnum.activeMine)
            {
                //bool flagstatus;
                //flagstatus = temp.flagMine.activeInHierarchy;
                //temp.flagMine.SetActive(!flagstatus);
                if (temp.flagMine.activeInHierarchy == true)
                    temp.flagMine.SetActive(false);
                else
                    temp.flagMine.SetActive(true);

                //格子状态切换
                if(temp.flagMine.activeInHierarchy == true)
                {
                    if(temp.statusMine==(int)StatusEnum.activeMine)
                    {
                        temp.statusMine = (int)StatusEnum.disableMine;
                    }
                }
                else
                {
                    if (temp.statusMine == (int)StatusEnum.disableMine)
                    {
                        temp.statusMine = (int)StatusEnum.activeMine;
                    }
                }
            }
        }
    }
    //游戏结束地雷全部标识出来
    private void gameOver()
    {
        for(int i=0;i<Cubes.Length;++i)
        {
            CubeControl temp = Cubes[i];
            if (temp.statusMine == (int)StatusEnum.activeMine)
            {
                temp.activeMine.SetActive(true);
            }
            else if (temp.statusMine == (int)StatusEnum.disableMine)
            {
                temp.disableMine.SetActive(true);
                temp.flagMine.SetActive(false);
            }
            else if(temp.statusMine == (int)StatusEnum.burstMine)
            {
                temp.burstMine.SetActive(true);
            }
            else if (temp.statusMine == (int)StatusEnum.initialCube)
            {
                expand(temp.row, temp.col);
                //temp.tileMine.SetActive(false);
                //temp.textMine.GetComponent<Text>().text = "" + neighborCount(temp.row,temp.col);
            }
        }
    }

    private void expand(int row,int col)
    {
        if (row>=0&&row<rowNum&&col>=0&&col<colNum)
        {
            CubeControl temp = Cubes[row * colNum + col];
            if(temp.isBomb==false&&temp.statusMine==(int)StatusEnum.initialCube)
            {
                temp.statusMine = (int)StatusEnum.emptyCube;
                int num = neighborCount(row, col);
                if (num != 0)
                {
                    //if(temp.tileMine.activeInHierarchy==true)
                    temp.tileMine.SetActive(false);
                    //if(temp.textMine.activeInHierarchy==false)
                    temp.textMine.SetActive(true);
                    temp.textMine.GetComponent<Text>().text = "" + num;

                }
                else
                {
                    //if (temp.tileMine.activeInHierarchy == true)
                        temp.tileMine.SetActive(false);
                    //if (temp.textMine.activeInHierarchy == false)
                        temp.textMine.SetActive(true);
                    temp.textMine.GetComponent<Text>().text = "" + 0;
                        expand(row - 1, col - 1);
                        expand(row - 1, col);
                        expand(row - 1, col + 1);
                        expand(row, col - 1);
                        expand(row, col + 1);
                        expand(row + 1, col - 1);
                        expand(row + 1, col);
                        expand(row + 1, col + 1);
                }
            }
        }
    }

    private int neighborCount(int row,int col)
    {
        int count = 0;
        //左上角
        if(col>=1&&row>=1)
        {
            if (Cubes[(row - 1) * colNum + col - 1].isBomb == true)
                ++count;
        }
        //正上方
        if(row>=1)
        {
            if (Cubes[(row - 1) * colNum + col].isBomb == true)
                ++count;
        }
        //右上方
        if(row>=1&&col+1<colNum)
        {
            if (Cubes[(row - 1) * colNum + col + 1].isBomb == true)
                ++count;
        }
        //左方
        if(col>=1)
        {
            if (Cubes[row* colNum + col - 1].isBomb == true)
                ++count;
        }
        //右方
        if(col+1<colNum)
        {
            if (Cubes[row* colNum + col + 1].isBomb == true)
                ++count;
        }
        //左下方
        if(row+1<rowNum&&col>=1)
        {
            if (Cubes[(row+1) * colNum + col - 1].isBomb == true)
                ++count;
        }
        //正下方
        if(row+1<rowNum)
        {
            if (Cubes[(row+1) * colNum + col].isBomb == true)
                ++count;
        }
        //右下方
        if(row+1<rowNum&&col+1<colNum)
        {
            if (Cubes[(row+1) * colNum + col + 1].isBomb == true)
                ++count;
        }

        return count;
    }
}
