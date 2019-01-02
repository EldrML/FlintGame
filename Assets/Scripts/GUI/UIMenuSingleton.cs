using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuSingleton : MonoSingleton<UIMenuSingleton>
{
    protected override UIMenuSingleton GetSingletonInstance { get { return this; } }

    public UIComponent primaryComponent;
    protected UIComponent currentComponent;
    private bool _XaxisInUse;
    private bool _YaxisInUse;

    virtual protected void Start()
    {

    }

    virtual protected void Update()
    {
        ManageInput();
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

    void TriggerAcceptAction()
    {
        currentComponent.AcceptAction();
    }

    protected void ManageInput()
    {
        // TODO: Change input system to button. This creates lag (gravity and so on)
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.7f)
        {
            if (!_XaxisInUse)
            {
                if (Input.GetAxis("Horizontal") > 0)
                {
                    this.GoToUIElement(this.currentComponent.elementAtEast);
                }
                else if (Input.GetAxis("Horizontal") < 0)
                {
                    this.GoToUIElement(this.currentComponent.elementAtWest);
                }
                _XaxisInUse = true;
            }
        }
        else
        {
            _XaxisInUse = false;
        }

        if (Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.7f)
        {
            if (!_YaxisInUse)
            {
                if (Input.GetAxis("Vertical") > 0)
                {
                    this.GoToUIElement(this.currentComponent.elementAtNorth);
                }
                else if (Input.GetAxis("Vertical") < 0)
                {
                    this.GoToUIElement(this.currentComponent.elementAtSouth);
                }
                _YaxisInUse = true;
            }
        }
        else
        {
            _YaxisInUse = false;

        }

        if (Input.GetButtonDown("Accept/Use"))
        {
            TriggerAcceptAction();
        }
    }
}
