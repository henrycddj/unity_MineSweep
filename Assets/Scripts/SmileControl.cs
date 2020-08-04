using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SmileControl : MonoBehaviour,IPointerClickHandler
{
    public Control control;
    // Start is called before the first frame update
    void Start()
    {
        control = GetComponentInParent<Control>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //初始化游戏
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("called smile");
            control.gameInitial();
        }
    }
}
