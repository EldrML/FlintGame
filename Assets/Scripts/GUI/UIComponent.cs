using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIComponent : MonoBehaviour
{
    public UIComponent elementAtSouth;
    public UIComponent elementAtEast;
    public UIComponent elementAtNorth;
    public UIComponent elementAtWest;

    private bool active;

    private Animator animator;

    void Awake()
    {
        InitializeComponent();
    }

    public virtual void InitializeComponent()
    {
        this.animator = this.GetComponent<Animator>();
    }

    public void Activate()
    {
        this.active = true;
        this.transform.SetAsLastSibling();
        this.UpdateAnimator();
    }

    public void Deactivate()
    {
        this.active = false;
        this.UpdateAnimator();
    }

    public void UpdateAnimator()
    {
        this.animator.SetBool("active", this.active);
    }

    public virtual void AcceptAction()
    {
        Debug.Log("UIComponent default accept action");
    }
}
