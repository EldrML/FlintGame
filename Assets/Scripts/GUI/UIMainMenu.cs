using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoSingleton<UIMainMenu>
{
    protected override UIMainMenu GetSingletonInstance { get { return this; } }

    public UIComponent primaryComponent;
    private UIComponent currentComponent;
    private Button uiButton;

    public bool allowed = true;

    // Use this for initialization
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ManageInput();
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

    void GoToUIElement(UIComponent nextComponent)
    {
        if (nextComponent != null)
        {
            this.currentComponent.Deactivate();
            nextComponent.Activate();
            this.currentComponent = nextComponent;
        }
    }

    public void DebugSomething()
    {
        // TODO: This is for testing. delete me
        Debug.Log("Button pressed");
    }

    void TriggerAcceptAction()
    {
        currentComponent.AcceptAction();
    }

    private void ManageInput()
    {
        // TODO: Change input system to button. This creates lag (gravity and so on)
        if (Input.GetButtonDown("Horizontal"))
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                this.GoToUIElement(this.currentComponent.elementAtEast);
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                this.GoToUIElement(this.currentComponent.elementAtWest);
            }
        }

        if (Input.GetButtonDown("Vertical"))
        {
            if (Input.GetAxis("Vertical") > 0)
            {
                this.GoToUIElement(this.currentComponent.elementAtNorth);
            }
            else if (Input.GetAxis("Vertical") < 0)
            {
                this.GoToUIElement(this.currentComponent.elementAtSouth);
            }
        }

        if (Input.GetButtonDown("Accept/Use"))
        {
            TriggerAcceptAction();
        }

        if (Input.GetButtonDown("Cancel/Run"))
        {
            this.Despawn();
        }
    }
}
