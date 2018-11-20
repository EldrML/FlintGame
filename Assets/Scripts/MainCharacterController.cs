using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterController : MonoSingleton<MainCharacterController>
{

    protected override MainCharacterController GetSingletonInstance { get { return this; } }

    [SerializeField]
    private float _joystickDeadZone = 0.05f;
    [SerializeField]
    private float _walkSpeed = 1.0f;
    [SerializeField]
    private float _runSpeed = 2.0f;

    private Cardinal.Direction _lookPosition;
    private Rigidbody2D _rb;
    private Animator _animator;

    private BoxCollider2D _collider;

    void Start()
    {
        _rb = this.GetComponent<Rigidbody2D>();
        _animator = this.GetComponent<Animator>();
        _lookPosition = Cardinal.Direction.South;
        _collider = this.GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        // This implements Golden Sun's 8 cardinal point clamping even with analog joysticks
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        if (Mathf.Abs(x) > _joystickDeadZone)
        {
            x = x > 0 ? 1.0f : -1.0f;
        }
        if (Mathf.Abs(y) > _joystickDeadZone)
        {
            y = y > 0 ? 1.0f : -1.0f;
        }

        // Make sure the magnitude of the speed vector never exceeds the walkSpeed.
        // This is done because of diagonal speeds being higher that straight ones.
        Vector2 speed = Vector2.ClampMagnitude(new Vector2(x, y), this._walkSpeed);

        // Check if player is running
        bool run = Input.GetAxis("Cancel/Run") > 0.0f;
        if (run)
        {
            speed = speed * this._runSpeed;
        }

        // Decides if interact or open menu
        InteractionControl();

        // This if here seems redundant but helps to ensure no animation is being
        // applied when this.Disable() is called.
        if (this.enabled)
        {
            // Animation triggering
            _animator.SetFloat("speedMagnitude", speed.magnitude);
            if (speed.magnitude > 0)
            {
                if (_lookPosition != Cardinal.CalculateFacingPositionForSpeed(speed))
                {
                    _lookPosition = Cardinal.CalculateFacingPositionForSpeed(speed);
                    // There was a cardinal point location. Change animation
                    this._animator.SetInteger("lookPosition", (int)_lookPosition);
                    this._animator.SetTrigger("changeCardinal");
                }
            }
        }

        // Final player movement
        _rb.MovePosition(this._rb.position + speed * Time.deltaTime);
    }

    private void InteractionControl()
    {
        if (Input.GetButtonDown("Accept/Use"))
        {
            bool interactionSuccessful = MakeInteraction();
            if (!interactionSuccessful)
            {
                UIMainMenu.Instance.Spawn();
            }
        }
    }

    private bool MakeInteraction()
    {
        // Returns true if interacted, false if not an interactable
        Vector2 center = _collider.bounds.center;
        Vector2 direction = Cardinal.vectorForDirection(_lookPosition);
        float size = _collider.size.x;

        // Disables own collider to prevent colliding on itself
        _collider.enabled = false;
        RaycastHit2D hit = Physics2D.Raycast(center, direction, size);
        _collider.enabled = true;

        if (hit.collider != null)
        {
            Interactable interactable = hit.transform.GetComponent<Interactable>();
            if (interactable == null)
            {
                // Can't interact with this.
                return false;
            }
            else
            {
                // Interaction successful.
                interactable.Interact(this.gameObject);
                return true;
            };
        }
        return false;
    }

    public void Disable()
    {
        this.enabled = false;

        // Force idle animation
        _animator.SetFloat("speedMagnitude", 0);
        this._animator.SetTrigger("changeCardinal");
    }

    public void Enable()
    {
        this.enabled = true;
    }
}