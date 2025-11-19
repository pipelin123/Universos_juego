using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float moveSpeed = 5f;    
    public float jumpForce = 20f;   

    [Header("Configuración de Suelo")]
    public Transform groundCheck;         
    public LayerMask groundLayer;         
    public float groundCheckRadius = 0.1f;

    // Componentes y variables privadas
    private Rigidbody2D rb;
    private Animator anim;
    private float horizontalInput;
    private bool isGrounded;
    private bool isFacingRight = true;
    public int nucleo = 0;
    public int gota = 0;
    public int score = 0;
    public bool hasKey = false;
    public bool hasSpike = false;
    public TextMeshProUGUI textNucleo;
    public TextMeshProUGUI textGota;
    public TextMeshProUGUI textScore;
    public GameObject GameOverPanel;
    public GameObject VictoryPanel;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); // ← obtenemos el Animator
        UpdateTextNucleo();
        UpdateTextGota();
        UpdateTextScore();
    }

    void Update()
    {
        // 1. Input Horizontal
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // 2. Saltar
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // 3. Animaciones
        UpdateAnimations();

        // 4. Voltear sprite
        FlipSprite();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Print("prueba");
        if (other.CompareTag("nucleo"))
        {
            nucleo = nucleo + 1;
            UpdateTextNucleo();

            Destroy(other.gameObject);
            Debug.Log("Collected!!!");
            Debug.Log("Score: " + nucleo);
            score = score + 100;
            UpdateTextScore();

        }
        if (other.CompareTag("gota"))
        {
            gota = gota + 1;
            UpdateTextGota();

            Destroy(other.gameObject);
            Debug.Log("Collected!!!");
            jumpForce += 2f;
            Debug.Log("Score: " + gota);
            score = score + 200;
            UpdateTextScore();

        }
        if (other.CompareTag("Key"))
        {
            hasKey = true;
            Debug.Log("has recolectado la llave!");
            Destroy(other.gameObject);
            
        }
        if (other.CompareTag("Spike"))
        {
            hasSpike = true;
            Debug.Log("has muerto!");
            GameOverPanel.SetActive(true);
            Time.timeScale = 0f;
        }
        
        
        //codicion de victoria
        if (nucleo + gota >= 30 || hasKey && !hasSpike)  //solo en un booleano si se pregunta sin nada es verdadero, y con un signo de admiracion al principio es falso
        {
            Debug.Log("Has ganado. Tienes suficientes puntos, la llave y no has tocado los pinchos");
            VictoryPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    void FixedUpdate()
    {
        // 5. Comprobar si está en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 6. Movimiento horizontal
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    // ACTUALIZAR ANIMACIONES
    void UpdateAnimations()
    {
        // Speed será el valor absoluto de horizontalInput
        anim.SetFloat("Speed", Mathf.Abs(horizontalInput));

        // isJumping será verdadero si NO estamos tocando el suelo
        anim.SetBool("IsJumping", !isGrounded);
    }

    // VOLTEAR EL PERSONAJE
    void FlipSprite()
    {
        if (horizontalInput < 0 && isFacingRight)
        {
            transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
            isFacingRight = false;
        }
        else if (horizontalInput > 0 && !isFacingRight)
        {
            transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
            isFacingRight = true;
        }
    }

    // DIBUJAR groundCheck EN EDITOR
    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
    void UpdateTextNucleo()
    {
        textNucleo.text = nucleo +"/7";
    }

    void UpdateTextGota()
    {
        textGota.text = gota +"/6";
    }

    void UpdateTextScore()
    {
        textScore.text = score +"";
    }
}