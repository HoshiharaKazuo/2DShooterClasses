using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

public class PlayerController : MonoBehaviour
{
    // headers servem para criar cabeçlhos no editor - útil pra organizar variáveis e referências
    [Header("Velocidade e força de pulo")]
    public float playerSpeed;
    public float playerJumpForce;

    [Header("Tamanho e distancia da caixa de check ground")]
    public Vector2 boxSize;
    public float castDistance;
    public float boxOffsetX;


    [Header("Referências de Componentes")]
    public Rigidbody2D playerRb; 
    public LayerMask groundLayer;
    public Animator animator; 
    public Transform bulletPoint; 
    public GameObject bulletPrefab;
    public SpriteRenderer spriteRenderer;

    private bool isGrounded = true;
    private bool idDead = false;
    private bool canMove = true;
    

    // Update é chamado uma vez por frame
    void Update()
    {
        if(canMove)
            MovePlayer();

        if (CheckIsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            // checa se o player esta tocando o chão e se ele apertou a tecla Espaço
            // se sim, adiciona força no eixo Y no rigidbody do player, o que o impulsiona para cima
            playerRb.AddForceY(playerJumpForce);
        }

        if(Input.GetKeyDown(KeyCode.F))
            animator.SetTrigger("shoot");
    }

    private void MovePlayer()
    {
        // Input.GetAxis returna um valor entre 1 e -1, de acordo com as teclas pressionadas - Setas ou A e D
        float moveX = Input.GetAxisRaw("Horizontal");

        if (moveX != 0) // se moveX for um valor diferente de 0, velocidade do player no eixo X é calculada
        {
            Vector2 velocity = new Vector2(moveX * playerSpeed, playerRb.linearVelocityY);
            playerRb.linearVelocity = velocity;
            transform.localScale = new Vector3(moveX, 1, 1);

            animator.SetBool("isWalking", true);
        }
        else // se moveX for 0, velocidade do player no eixo X é zerada
        {
            playerRb.linearVelocity = new Vector2(0, playerRb.linearVelocityY);
            animator.SetBool("isWalking", false);
        }
    }

    public void CreateBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletPoint.position, Quaternion.identity); // instancia a prefab da bala, na posição do transform do bullet point
        bullet.GetComponent<Bullet>().SetDirection(transform.localScale.x);
    }

    private bool CheckIsGrounded()
    {
        Vector2 origin = (Vector2)transform.position + new Vector2(boxOffsetX, 0);

        // origem da caixa
        // tamanho da caixa - box size;
        // ângulo da projeção - 0
        // direção da projeção - no caso, usamos tranform.up negativo, o que direciona a projeção para baixo
        // distancia da projeção a partir do meio do player
        // qual layer deve ser detectado


        if (Physics2D.BoxCast(origin, boxSize, 0, -transform.up, castDistance, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    
    private void OnDrawGizmos()
    {
        // Desenha linhas no editor, aqui, replicaremos nosso boxcast
        Vector2 origin = (Vector2)transform.position + new Vector2(boxOffsetX, 0);
        Gizmos.DrawWireCube(origin - (Vector2)transform.up * castDistance, boxSize);
    }

    public void PlayerDeath()
    {
        canMove = false;
        playerRb.linearVelocityX = 0;
        animator.SetTrigger("death");
    }

    public void ResetGame()
    {
        // scene manager recarrega a cena atual
        // .name para que o load scene carregua a cena baseada em seu nome - uma string
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
