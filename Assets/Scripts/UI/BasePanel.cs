using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePanel : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private float alphaSpeed = 10;
    public bool isShow = false;
    private UnityAction hideCallBack=null;
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        canvasGroup =this.GetComponent<CanvasGroup>();
        if(canvasGroup == null)
        {
            canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
        }
    }
    protected virtual void Start()
    {
        Init();
    }

    public abstract void Init();

    //ÏÔÊ¾Ãæ°å
    public virtual void ShowMe()
    {
        canvasGroup.alpha = 0;
        isShow = true;
    }
    //Òþ²ØÃæ°å
    public virtual void HideMe(UnityAction callBack)
    {
        canvasGroup.alpha = 1;
        isShow = false;

        hideCallBack = callBack;
    }

    // Update is called once per frame
    void Update()
    {
        if (isShow && canvasGroup.alpha != 1)
        {
            canvasGroup.alpha += alphaSpeed*Time.deltaTime;
            if (canvasGroup.alpha >= 1)
                canvasGroup.alpha = 1;
        }
        else if (!isShow && canvasGroup.alpha != 0)
        {
            canvasGroup.alpha -= alphaSpeed*Time.deltaTime;
            if(canvasGroup.alpha <= 0)
            {
                canvasGroup.alpha = 0;
                hideCallBack?.Invoke();
            }
        }
    }
}
