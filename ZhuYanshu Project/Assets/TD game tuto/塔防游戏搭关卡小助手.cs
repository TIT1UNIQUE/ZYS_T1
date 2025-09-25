using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 塔防游戏搭关卡小助手 : MonoBehaviour
{
    public float 格子边长;
    public float 格子原点;
    //脚本的作用，我想做一个塔防游戏关卡搭建的辅助程序，把预先放置好的多个cube，
    //整理好位置，使其中的每个手动摆放的cube，
    //都刚好位于以格子原点为原点，以格子边长为边长的格子中

    public Transform 所有方块的父对象;
    public string 理想的方块名 = "障碍物";

    void Start()
    {
        重命名();
    }

    void 重命名()
    {
        for (int i = 0; i < 所有方块的父对象.childCount; i++)
        {

            var c = 所有方块的父对象.GetChild(i);
            c.name = 理想的方块名;

            var pos = c.transform.position;
            var x = pos.x + 0.5f;
            var z = pos.z + 0.5f;
            x = Mathf.Round(x) - 0.5f; z = Mathf.Round(z) - 0.5f;
            c.transform.position = new Vector3(x, 0, z);

            // var x = Mathf.Round(pos.x);
            // var z = Mathf.Round(pos.z);
            //  其中一个障碍物.transform.position = new Vector3(x, 0, z);
        }
    }
}
