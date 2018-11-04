using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    private Rigidbody2D rb;
    private int previousLookPosition;
    private Animator animator;
    public int lookPosition;
    public float joystickDeadZone = 0.05f;
    public float walkSpeed = 1.0f;
    public float runSpeed = 2.0f;


    void Start()
    {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.animator = this.GetComponent<Animator>();
        this.lookPosition = Cardinal.SOUTH;
        this.previousLookPosition = Cardinal.SOUTH;
    }

    void Update()
    {
        // This implements Golden Sun's 8 cardinal point clamping even with analog joysticks
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        if (Mathf.Abs(x) > this.joystickDeadZone)
        {
            x = x > 0 ? 1.0f : -1.0f;
        }
        if (Mathf.Abs(y) > this.joystickDeadZone)
        {
            y = y > 0 ? 1.0f : -1.0f;
        }

        // Make sure the magnitude of the speed vector never exceeds the walkSpeed.
        // This is done because of diagonal speeds being higher that straight ones.
        Vector2 speed = Vector2.ClampMagnitude(new Vector2(x, y), this.walkSpeed);

        // Check if player is running
        bool run = Input.GetAxis("Fire1") > 0.0f;
        if (run)
        {
            speed = speed * this.runSpeed;
        }

        // Animation triggering
        this.animator.SetFloat("speedMagnitude", speed.magnitude);
        if (speed.magnitude > 0)
        {
            this.previousLookPosition = this.lookPosition;
            this.lookPosition = Cardinal.calculateFacingPositionForSpeed(speed);
            if (this.previousLookPosition != this.lookPosition)
            {
                // There was a cardinal point location. Change animation
                this.animator.SetInteger("lookPosition", this.lookPosition);
                this.animator.SetTrigger("changeCardinal");
            }
        }

        // Final player movement
        this.rb.MovePosition(this.rb.position + speed * Time.deltaTime);
    }
}
