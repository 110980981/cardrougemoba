using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalPanel : BasePanel
{
    [SerializeField]
    public int progress;
    private int preProgress;

    public Image star1;
    public Image star2;
    public Image star3;
    public Image star4;
    public Image star5;

    protected override void Awake()
    {
        progress = -1;
        preProgress = progress;
        base.Awake();
        EventCenter.GetInstance().AddEventListener("StartOrWin", () => { progress++; });
    }

    private void Update()
    {
        if (progress==1)
        {
            star1.color = Color.black;
            if(preProgress!=progress)
            {
                preProgress = progress;
            }
        }
        else if (progress==2)
        {
            star2.color = Color.black;
            if (preProgress != progress)
            {
                preProgress = progress;
            }

        }
        else if(progress==3)
        {
            star3.color = Color.black;
            if (preProgress != progress)
            {
                preProgress = progress;
            }
        }
        else if (progress==4)
        {
            star4.color = Color.black;
            if (preProgress != progress)
            {
                preProgress = progress;
            }
        }
        else if(progress==5)
        {
            star5.color = Color.black;
            if (preProgress != progress)
            {
                preProgress = progress;
                UIManager.GetInstance().ShowPanel<WinPanel>("WinPanel", E_UI_Layer.Top);
                UIManager.GetInstance().HidePanel("NormalPanel");
            }
        }
    }

    protected override void OnClick(string btnName)
    {
        switch(btnName)
        {
            case "BtnBack":
                Application.Quit();
                break;
        }
    }
}
