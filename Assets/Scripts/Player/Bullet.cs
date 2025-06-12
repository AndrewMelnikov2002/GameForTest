using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private float bullet_speed = 1f;
    [SerializeField] private float life_time;
    
    private float life_timer;

    private void Start() { life_timer = life_time; }

    void Update()
    {
        transform.position = transform.position + (transform.right * (bullet_speed * Time.deltaTime));

        life_timer -= Time.deltaTime;

        if (life_timer <= 0)
            Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Zombie" & !collision.isTrigger)
        {
            collision.GetComponent<ZombieController>().GetDamage(34f, collision.transform.position - transform.position);
            Destroy(gameObject);
        }
    }
}
