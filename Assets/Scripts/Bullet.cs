using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed;
    public float bulletTimer;
    public int bulletDamage;
    public float direction;
    public Rigidbody2D bulletRb;
    public bool isEnemyBullet; // aqui, como usamos o mesmo script para balas do inimigo e do player, vamos usar um identificador para saber quem pertence

    void Start()
    {
        bulletRb.linearVelocityX = bulletSpeed * direction;

        // Para chamar IEnumerators, devemos chamar uma corotina - uma fun��o que pode suspender uma a��o por determinado tempo
        StartCoroutine(BulletLifetime());
    }

    public void SetDirection(float dir)
    {
        direction = dir;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isEnemyBullet) // se a bala for do inimigo...
        {
            if (collision.gameObject.tag == "Player") // ...e a colis��o for com o player, causa a morte do player
            {
                collision.GetComponent<PlayerController>().PlayerDeath();
                Destroy(gameObject);
            }
        }
        else // se n�o...
        {
            if (collision.gameObject.tag == "Enemy") // ... e a colis��o for com um inimigo, causa dano ao inimigo
            {
                collision.GetComponent<Enemy>().DamageEnemy(bulletDamage);
                Destroy(gameObject);
            }
        }
    }

    
    private IEnumerator BulletLifetime()
    {
        // Usamos IEnumerator para fazer com que a bala se destrua ap�s um tempo
        yield return new WaitForSeconds(bulletTimer);

        Destroy(gameObject);
    }
}
