using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int enemyLife;
    public float shootingDelay;
    public Transform playerTransform;
    public Animator animator;
    public GameObject bulletPrefab;
    public Transform bulletPoint;

    private void Start()
    {
        StartCoroutine(EnemyShoot());
    }

    private void Update()
    {
        FacePlayer();
    }

    private void FacePlayer()
    {
        if (playerTransform.position.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    public void DamageEnemy(int damage)
    {
        enemyLife -= damage;

        if(enemyLife <= 0 )
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator EnemyShoot()
    {
        yield return new WaitForSeconds(shootingDelay);

        animator.SetTrigger("shoot");

        StartCoroutine(EnemyShoot());
    }

    public void CreateBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletPoint.position, Quaternion.identity); // instancia a prefab da bala, na posição do transform do bullet point
        bullet.GetComponent<Bullet>().SetDirection(transform.localScale.x);
    }
}
