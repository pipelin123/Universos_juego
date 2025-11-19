using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float jumpForce = 20f;

    [Header("Suelo")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.1f;

    [Header("Coleccionables")]
    public int nucleo = 0;
    public int gota = 0;
    public int totalItemsRequired = 13;

    [Header("UI")]
    public TextMeshProUGUI textNucleo;
    public TextMeshProUGUI textGota;
    public TextMeshProUGUI textScore;
    
    [Header("Panels")]
    public GameObject GameOverPanel;
    public GameObject VictoryPanel;

    [Header("Especial")]
    public GameObject swordObject;  
    public bool hasSword = false;
    public bool hasKey = false;
    public bool hasSpike = false;

    private Rigidbody2D rb;
    private Animator anim;
    private float horizontalInput;
    private bool isGrounded;
    private bool isFacingRight = true;
    private int score = 0;
    public GameObject messageFindSword;
    public GameObject messageGotSword;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        UpdateTextNucleo();
        UpdateTextGota();
        UpdateTextScore();

        swordObject.SetActive(false); // La espada no aparece al inicio
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        UpdateAnimations();
        FlipSprite();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // --- Núcleo ---
        if (other.CompareTag("nucleo"))
        {
            nucleo++;
            moveSpeed += 1.5f;
            score += 100;

            UpdateTextNucleo();
            UpdateTextScore();

            Destroy(other.gameObject);
            CheckIfAllCollected();
        }

        // --- Gota ---
        if (other.CompareTag("gota"))
        {
            gota++;
            jumpForce += 2f;
            score += 200;

            UpdateTextGota();
            UpdateTextScore();

            Destroy(other.gameObject);
            CheckIfAllCollected();
        }

        // --- Espada ---
        if (other.CompareTag("Sword"))
        {
            hasSword = true;
            Debug.Log("Espada obtenida.");
            Destroy(other.gameObject);

            if (messageFindSword != null)
                messageFindSword.SetActive(false);

            if (messageGotSword != null)
                messageGotSword.SetActive(true);
        }   

        // --- Estatua (Key) ---
        if (other.CompareTag("Key"))
        {
            if (hasSword)
            {
                hasKey = true;
                Destroy(other.gameObject);
                Debug.Log("Has activado la estatua.");
            }
            else
            {
                Debug.Log("Necesitas la espada para activar la estatua.");
            }
        }

        // --- Spike ---
        if (other.CompareTag("Spike"))
        {
            hasSpike = true;
            GameOverPanel.SetActive(true);
            Time.timeScale = 0f;
        }

        // --- Condición final ---
        if (nucleo + gota >= totalItemsRequired && hasKey && !hasSpike)
        {
            VictoryPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    void UpdateAnimations()
    {
        anim.SetFloat("Speed", Mathf.Abs(horizontalInput));
        anim.SetBool("IsJumping", !isGrounded);
    }

    void FlipSprite()
    {
        if (horizontalInput < 0 && isFacingRight)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            isFacingRight = false;
        }
        else if (horizontalInput > 0 && !isFacingRight)
        {
            transform.localScale = new Vector3(1, 1, 1);
            isFacingRight = true;
        }
    }

    void UpdateTextNucleo()  => textNucleo.text = nucleo + "/7";
    void UpdateTextGota()    => textGota.text = gota + "/6";
    void UpdateTextScore()   => textScore.text = score + "";

    void CheckIfAllCollected()
    {
        if (nucleo + gota >= totalItemsRequired)
        {
            Debug.Log("Se han recogido todos los coleccionables. Aparece la espada.");
            swordObject.SetActive(true);

            // Mostrar mensaje de buscar la espada
            if (messageFindSword != null)
                messageFindSword.SetActive(true);
        }
    }
}
