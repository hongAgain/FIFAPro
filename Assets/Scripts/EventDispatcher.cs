using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class EventDispatcher : MonoBehaviour
{
    public string DoubleClickPram = "";
    public string ClickPram = "";
    public System.Object UserData = null;

    public UIBaseWindowLua Receiver = null;

    void Start()
    {

    }

    void OnClick()
    {
//        if (Receiver != null)
//        {
//            Receiver.OnClick(gameObject, ClickPram, UserData);
//        }
    }

    void OnDoubleClick()
    {
//        if(Receiver != null)
//        {
//            Receiver.OnDoubleClick(gameObject, ClickPram, DoubleClickPram);
//        }
    }

}

