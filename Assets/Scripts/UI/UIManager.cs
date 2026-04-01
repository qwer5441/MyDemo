using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager 
{
    private static UIManager instance=new UIManager();
    public static UIManager Instance => instance;

    //存储显示的面板
    private Dictionary<string,BasePanel>panelDic=new Dictionary<string,BasePanel>();

    //用于设置为面板的父对象
    private Transform canvasTrans;

    //自动创建画布
    private UIManager()
    {
        //实例canvas
        GameObject canvas = GameObject.Instantiate(Resources.Load<GameObject>("UI/Canvas"));
        //保存canvas起来
        canvasTrans = canvas.transform;
        //过场景不删
        GameObject.DontDestroyOnLoad(canvas);
    }

    //显示面板
    public T ShowPanel<T>() where T : BasePanel
    {
        //获取泛型T的类型（类名）
        //提前定义好 面板名字 和 类名一样
        string panelName = typeof(T).Name;

        //字典中是否已经显示这个面板
        if (panelDic.ContainsKey(panelName))
        {
            return panelDic[panelName] as T;
        }

        //还没有显示 就根据面板名字 动态创建 面板的预设体
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("UI/" + panelName));

        //设置父对象
        panelObj.transform.SetParent(canvasTrans, false);

        //获取面板上自带的显示代码
        T panel = panelObj.GetComponent<T>();

        //添加到字典中 以备之后的显示等操作
        panelDic.Add(panelName, panel);

        //调用自己的显示逻辑
        panel.ShowMe();

        return panel;
    }

    //隐藏面板
    public void HidePanel<T>(bool isFade=true)where T : BasePanel
    {
        //根据泛型得面板名字
        string panelName=typeof(T).Name;
        if (panelDic.ContainsKey(panelName))
        {
            if (isFade)
            {
                //面板淡出后再删除
                panelDic[panelName].HideMe(() => //传入淡出后要做的事
                {
           
                    //删除对象
                    GameObject.Destroy(panelDic[panelName].gameObject);
                    //删除字典里的面板脚本
                    panelDic.Remove(panelName);
                });
            }
            else
            {
                //直接删除
                GameObject.Destroy(panelDic[panelName].gameObject);
                panelDic.Remove(panelName);
            }

        }
    }

    //得到面板
    public T GetPanel<T>() where T : BasePanel
    {
        string panelName= typeof(T).Name;
        if (panelDic.ContainsKey(panelName))
        {
            return panelDic[panelName] as T;
        } 
        return null;
    }
}
