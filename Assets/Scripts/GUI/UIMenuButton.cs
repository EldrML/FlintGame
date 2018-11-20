using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuButton : UIComponent
{

    private Button uiButton;
    public override void InitializeComponent()
    {
        base.InitializeComponent();
        this.uiButton = this.GetComponent<Button>();
    }
    public override void AcceptAction()
    {
        this.uiButton.onClick.Invoke();
    }
}
