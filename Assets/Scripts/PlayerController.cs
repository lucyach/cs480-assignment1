using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;


public class PlayerController : MonoBehaviour
{

    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    public float speed;
    public float jumpForce = 10f;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    
    // Double jump variables
    private bool isGrounded;
    private int jumpsRemaining;
    private int maxJumps = 2;


    void Start() {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);
        jumpsRemaining = maxJumps;
    }

    void OnMove (InputValue movementValue) {
       Vector2 movementVector = movementValue.Get<Vector2>();
       movementX = movementVector.x;
       movementY = movementVector.y;
   }

   void OnJump(InputValue jumpValue) {
       if (jumpValue.isPressed && jumpsRemaining > 0) {
           rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
           jumpsRemaining--;
       }
   }

   void FixedUpdate() {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
   }

   void OnTriggerEnter (Collider other) 
   {
       if (other.gameObject.CompareTag("PickUp")) 
       {
           other.gameObject.SetActive(false);
           count++;
           SetCountText();
       }
   } 

   void SetCountText() {
       countText.text = "Count: " + count.ToString();
       if (count >= 6) 
       {
           winTextObject.SetActive(true);
       }
   }

   void OnCollisionEnter(Collision collision) {
       // Check if we're touching the ground (anything below the ball)
       if (collision.contacts.Length > 0) {
           Vector3 contactNormal = collision.contacts[0].normal;
           if (contactNormal.y > 0.7f) { // If the surface is mostly horizontal (ground)
               isGrounded = true;
               jumpsRemaining = maxJumps; // Reset double jump when touching ground
           }
       }
   }

   void OnCollisionExit(Collision collision) {
       // When leaving the ground, we're no longer grounded
       isGrounded = false;
   }
}
