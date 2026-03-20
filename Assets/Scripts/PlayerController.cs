using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float jumpForce;
    public float gravityModifier;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;

    public AudioClip jumpSfx;
    public AudioClip crashSfx;

    private Rigidbody rb;
    private InputAction jumpAction;
    private InputAction dash;
    private bool isOnGround = true;
    public bool IsDashing;
    private int hp = 3;

    private Animator playerAnim;
    private AudioSource playerAudio;

    public bool gameOver = false;
    private int jumpCount;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Physics.gravity *= gravityModifier;

        jumpAction = InputSystem.actions.FindAction("Jump");
        dash = InputSystem.actions.FindAction("Sprint");

        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (jumpCount >= 2)
        {
            isOnGround = false;
        }

        if (jumpAction.triggered && isOnGround && !gameOver)
        {
            rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
            playerAnim.SetTrigger("Jump_trig");
            dirtParticle.Stop();
            playerAudio.PlayOneShot(jumpSfx);
            jumpCount++;
        }

        if (dash.IsPressed())
        {
            IsDashing = true;
        }
        else
        {
            IsDashing = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            jumpCount = 0;
            dirtParticle.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            hp--;
            Debug.Log("hit! hp remaining: " +hp);
            Instantiate(explosionParticle, transform.position, Quaternion.identity);
            playerAudio.PlayOneShot(crashSfx);
            Destroy(collision.gameObject);
        }

        if (hp <= 0)
        {
            gameOver = true;
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            Instantiate(explosionParticle, transform.position, Quaternion.identity);
            dirtParticle.Stop();
            playerAudio.PlayOneShot(crashSfx);
        }
    }

}