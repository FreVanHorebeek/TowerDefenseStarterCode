using UnityEngine;
using static Enums;

public class Tower : MonoBehaviour
{
    public float attackRange = 2f;
    public float attackRate = 1f;
    public int attackDamage = 1;
    public float attackSize = 1f;
    public GameObject bulletPrefab;
    public TowerType type;

    private float lastAttackTime;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    void Update()
    {
        if (Time.time - lastAttackTime >= 1f / attackRate)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    void Attack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                GameObject enemy = collider.gameObject;
                Shoot(enemy);
                break; // Only shoot one enemy per attack
            }
        }
    }

    void Shoot(GameObject enemy)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Projectile projectile = bullet.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.damage = attackDamage;
            projectile.target = enemy.transform;
            bullet.transform.localScale = new Vector3(attackSize, attackSize, 1f);
        }
    }
}
