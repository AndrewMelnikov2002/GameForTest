using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer visual_sprite;


    [Header("General")]
    [SerializeField] private bool is_ai_enabled = true;
    [SerializeField] private float move_speed = 1;


    [SerializeField] private float zombie_health = 100;
    [Space(10)]
    [SerializeField] private HealthBar health_bar;
    [Space(10)]
    [SerializeField] private ParticleSystem death_effect;

    private PlayerController player;

    [Header("Loot")]
    [SerializeField] private List<ItemConfig> loot;




    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        visual_sprite = GetComponentInChildren<SpriteRenderer>();

        if (health_bar != null)
        {
            health_bar.SetAmount(1);
            health_bar.SetTarget(transform);
        }

        death_effect.Play();
    }

    private void Update()
    {
        if (rb.velocity.magnitude > 0)
            rb.velocity += -rb.velocity * Time.deltaTime * 5f;

        if (player != null & is_ai_enabled)
        {
            anim.SetBool("is_walk", true);
            transform.position += (player.transform.position - transform.position).normalized * Time.deltaTime * move_speed;

            visual_sprite.flipX = player.transform.position.x < transform.position.x;
        }
        else
            anim.SetBool("is_walk", false);

    }

    private void Death()
    {
        visual_sprite.enabled = false;

        is_ai_enabled = false;    

        death_effect.Play();

        if (loot.Count > 0)
        {
            Transform droped_item = Instantiate(loot[Random.Range(0, loot.Count - 1)].WorldObject).transform;

            droped_item.position = transform.position;
        }

        FindFirstObjectByType<PlayerController>().AddScore();

        StartCoroutine(DestroyZombie());
    }

    private IEnumerator DestroyZombie()
    {
        yield return new WaitForSeconds(0.25f);

        transform.position = new Vector3(1000, 1000, 0);

        yield return new WaitForSeconds(4f);

        FindFirstObjectByType<ZombieSpawner>().RemoveZombie(this);

        Destroy(health_bar.gameObject);

        Destroy(gameObject);
    }

    public void GetDamage(float damage, Vector3 direction)
    {
        if (zombie_health > damage)
        {
            zombie_health -= damage;

            if (health_bar != null)
                health_bar.SetAmount(zombie_health / 100);

            anim.SetTrigger("damage");

            rb.AddForce(direction * 7.5f, ForceMode2D.Impulse);
        }
        else
        {
            zombie_health = 0;

            if (health_bar != null)
                health_bar.gameObject.SetActive(false);


            Death();
        }
    }

    public void SetHealthBar(HealthBar health_bar)
    {
        this.health_bar = health_bar;

        this.health_bar.SetTarget(transform);
        this.health_bar.SetAmount(1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            player = collision.GetComponent<PlayerController>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            player = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player" & player != null)
        {
            player.GetDamage(25f, collision.transform.position - transform.position);
            if (player.IsDead)
                is_ai_enabled = false;
        }
    }
}

