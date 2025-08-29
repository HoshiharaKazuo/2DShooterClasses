using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Stats de velocidade e pulo do player")]
    public float playerSpeed;
    public float playerJumpForce;

    [Header("Tamanho e distancia da caixa de check ground")]
    public Vector2 boxSize;
    public float castDistance;

    [Header("Referências de Componentes")]
    public Rigidbody2D playerRb;
    public LayerMask groundLayer;

    private bool isGrounded = true;
    
    // Start é chamado uma vez antes da primeira execução do Update após a criação do MonoBehaviour
    void Start()
    {

    }

    // Update é chamado uma vez por frame
    void Update()
    {
        MovePlayer();

        if (CheckIsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            JumpPlayer();
        }
    }

    private void MovePlayer()
    {
        // Input.GetAxis returna um valor entre 1 e -1, de acordo com as teclas pressionadas - Setas ou A e D
        float moveX = Input.GetAxisRaw("Horizontal");

        if (moveX != 0) // se moveX for um valor diferente de 0, velocidade do player no eixo X é calculada
        {
            Vector2 velocity = new Vector2(moveX * playerSpeed, playerRb.linearVelocityY);
            playerRb.linearVelocity = velocity;
        }
        else // se moveX for 0, velocidade do player no eixo X é zerada
        {
            playerRb.linearVelocity = new Vector2(0, playerRb.linearVelocityY);
        }
    }

    private void JumpPlayer()
    {
        playerRb.AddForceY(playerJumpForce);
    }

    // função para checar se o player está tocando o chão
    private bool CheckIsGrounded()
    {
        // origem do caixa - no caso, o meio do objeto player - transform.position;
        // tamanho da caixa - box size;
        // ângulo da projeção - 0
        // direção da projeção - no caso, usamos tranform.up negativo, o que direciona a projeção para baixo
        // distancia da projeção a partir do meio do player
        // qual layer deve ser detectado
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Desenha linhas no editor, aqui, replicaremos nosso boxcast
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position-transform.up * castDistance, boxSize);
    }
}
