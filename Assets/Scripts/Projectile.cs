using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Transform target;
    public float speed;
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        RotateTowardsTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        MoveTowardsTarget();

        if (Vector2.Distance(transform.position, target.position) < 0.2f)
        {
            DealDamage();
            Destroy(gameObject);
        }
    }

    void RotateTowardsTarget()
    {
        if (target != null)
        {
            Vector2 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void MoveTowardsTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    void DealDamage()
    {
        // Deal damage to the target
        if (target.GetComponent<Enemy>() != null)
        {
            target.GetComponent<Enemy>().health -= damage;
            Debug.Log("Dealt " + damage + " damage to the enemy!");
        }
    }
}
