using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPanel : BasePanel
{
    protected override void OnClick(string btnName)
    {
        switch(btnName) 
        {
            case "BtnAgain":
                ScenesMgr.GetInstance().LoadScene("Game", () => {
                    UIManager.GetInstance().HidePanel("WinPanel");
                    
                });
                break;
        
        }
    }
}
