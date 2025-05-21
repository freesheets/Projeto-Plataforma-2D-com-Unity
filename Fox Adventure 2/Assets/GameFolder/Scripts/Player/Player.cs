using UnityEngine; // Importa as funcionalidades principais do Unity (MonoBehaviour, Rigidbody2D, etc.)

public class Player : MonoBehaviour // Define a classe Player como um componente Unity (herda de MonoBehaviour)
{
    private Rigidbody2D rb2d; // Referência ao Rigidbody2D do personagem (para aplicar física)

    public Animator playerAnim;
    [SerializeField] private LayerMask groundMask; // Colisão com o chão

    private float moveInput; // Armazena a entrada horizontal do jogador (ex: A/D ou setas)
    public float moveSpeed; // Velocidade de movimento (ajustável no Inspector)

    public float jumpForce; // Força do pulo (não está sendo usado ainda)

    [SerializeField] private bool OnGround;

    private bool WasOnGround;

    private bool isJump;

    private Collider2D[] colliders_1, colliders_2;

    private float groundCheckRadius = 0.036f;  // Tamanho do circulo no pé do player
    public Transform[] groundCheck; // posição do objeto que vai fazer a checagem da colisão com o chão


    // Start é chamado uma vez no início do jogo
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>(); // Obtém o Rigidbody2D anexado ao GameObject
    }

    // Update é chamado uma vez por frame (ideal para ler inputs)
    void Update()
    {
        InputSystem(); // Chama método responsável por capturar entradas do jogador
        CheckGround(); // Chama método responsável por 
        Animations();
    }

    // FixedUpdate é chamado a cada intervalo fixo de tempo (ideal para física)
    private void FixedUpdate()
    {
        Move(); // Aplica movimento baseado na entrada
    }

    // Método que aplica o movimento horizontal
    private void Move()
    {
        // Move o personagem na horizontal mantendo a velocidade vertical (pulo, gravidade etc.)
        rb2d.linearVelocity = new Vector2(moveInput * moveSpeed, rb2d.linearVelocity.y);
        // OBS: Aqui deveria ser "velocity", não "linearVelocity" — isso causará erro
    }

    // Método que lê a entrada do jogador
    private void InputSystem()
    {
        moveInput = Input.GetAxisRaw("Horizontal"); // Lê entrada horizontal (esquerda/direita)

        // Se o jogador está apertando esquerda ou direita
        if (moveInput != 0f)
        {
            // Inverte a escala horizontal para virar o sprite na direção do movimento
            transform.localScale = new Vector3(moveInput, 1f, 1f);
        }

        if (Input.GetKeyDown(KeyCode.Space) && OnGround)
        {
            Jump();
        }
    }

    void CheckGround()
    {
        colliders_1 = Physics2D.OverlapCircleAll(groundCheck[0].position, groundCheckRadius, groundMask);
        colliders_2 = Physics2D.OverlapCircleAll(groundCheck[1].position, groundCheckRadius, groundMask);

        if (colliders_1.Length > 0 || colliders_2.Length > 0)
        {
            OnGround = true;

        }
        else
        {
            OnGround = false;
        }
    }

    private void Jump()
    {
        rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, jumpForce);
    }

    private void Animations()
    {
        playerAnim.SetFloat("SpeedX", Mathf.Abs(moveInput));
        playerAnim.SetFloat("SpeedY", rb2d.linearVelocity.y);
        playerAnim.SetBool("OnGround", OnGround);
        
    }

}
