using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : UIMenuSingleton
{
    public bool allowed = true;

    // Use this for initialization
    override protected void Start()
    {
        base.Start();
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();
        CancelInput();
    }

    public void Spawn()
    {
        if (!allowed) return;
        this.gameObject.SetActive(true);
        primaryComponent.Activate();
        currentComponent = primaryComponent;
        MainCharacterController.Instance.Disable();
    }

    public void Despawn()
    {
        this.gameObject.SetActive(false);
        MainCharacterController.Instance.Enable();
    }

    private void CancelInput()
    {
        if (Input.GetButtonDown("Cancel/Run"))
        {
            this.Despawn();
        }
    }
}
