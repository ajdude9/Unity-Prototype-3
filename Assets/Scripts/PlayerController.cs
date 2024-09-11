using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private float jumpForce = 20;
    private float gravityModifier = 5;
    private bool isGrounded = true;
    private bool jumpReset = false;
    public bool gameOver = false;
    private Animator playerAnim;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public AudioClip jumpSound;
    public AudioClip deathSound;
    private AudioSource playerAudio;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        Physics.gravity *= gravityModifier;
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space) && isGrounded && !gameOver)
        {
            playerRb.velocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpForce = 20;
            isGrounded = false;
            jumpReset = false;
            playerAnim.SetTrigger("Jump_trig");
            playerAudio.PlayOneShot(jumpSound, 1.0f);
            dirtParticle.Stop();
        }
        if(transform.position.y > 0.5 && !jumpReset)
        {
            jumpReset = true;
            Debug.Log("Jumped");
        }
        if(transform.position.y < 0.5 && jumpReset)
        {
            isGrounded = true;
            jumpReset = false;
            
            Debug.Log("Reset");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpForce = 20;
            dirtParticle.Play();
        }
        if(collision.gameObject.CompareTag("gameOverTrigger"))
        {
            gameOver = true;
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            explosionParticle.Play();
             playerAudio.PlayOneShot(deathSound, 1.0f);
            dirtParticle.Stop();
        }
    }
    
}
