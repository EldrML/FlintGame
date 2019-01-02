using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGenericMenu : UIMenuSingleton
{
    override protected void Start()
    {
        base.Start();
        primaryComponent.Activate();
        currentComponent = primaryComponent;
    }

    override protected void Update()
    {
        base.Update();
    }
}
