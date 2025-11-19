using UnityEngine;

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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); // ← obtenemos el Animator
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
}