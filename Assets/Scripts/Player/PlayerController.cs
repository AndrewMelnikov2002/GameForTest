using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameController game_controller;
    private Animator anim;
    private FixedJoystick joystick;
    private Rigidbody2D rb;


    private bool is_player_dead = false;

    [Header("Main")]
    [SerializeField] [Range(0,100)] private float player_health = 100;
    [SerializeField] private HealthBar health_bar;
    [Space(10)]
    [SerializeField] private float move_speed;
    [Space(10)]
    [SerializeField] private SpriteRenderer player_sprite;
    private Vector3 move_direction = Vector3.zero;


    [Space(20)]
    [Header("Effects")]
    [SerializeField] private ParticleSystem death_effect;


    [Space(20)]
    [Header("Gun")]
    [SerializeField] private float shoot_cooldown = 0.5f;
    [Space(10)]
    [SerializeField] private SpriteRenderer gun_sprite;
    [SerializeField] private Transform gun_holder;
    [Space(10)]
    [SerializeField] private ParticleSystem shoot_effect;
    [Space(10)]
    [SerializeField] private GameObject bullet_prefab;
    private float shoot_cooldown_timer = 0;
    private float gun_angle = 0;



    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        game_controller = FindFirstObjectByType<GameController>();
        joystick = FindFirstObjectByType<FixedJoystick>();
        rb = GetComponent<Rigidbody2D>();

        if (joystick == null)
            Debug.LogError("Not Found FixedJoystik!");

        FindFirstObjectByType<CameraTracking>().SetTarget(transform);

        if (health_bar != null)
        {
            health_bar.SetTarget(transform);
            health_bar.SetAmount(1);
        }
        else
            Debug.LogWarning("No player health bar!");
    }


    private void Update()
    {
        if (!is_player_dead)
        {
            if (joystick.Direction != Vector2.zero)
            {
                gun_angle = Vector2.SignedAngle(Vector2.right, joystick.Direction);
                move_direction = joystick.Direction;

                transform.position += move_direction * Time.deltaTime * move_speed;
            }

            anim.SetBool("is_walk", joystick.Direction != Vector2.zero);

            anim.SetFloat("walk_speed", joystick.Direction.magnitude);

            player_sprite.flipX = move_direction.x < 0;

            Quaternion gun_rotation = Quaternion.Lerp(gun_holder.rotation, Quaternion.Euler(0, 0, gun_angle), Time.deltaTime * 4f);

            gun_holder.rotation = gun_rotation;

            gun_sprite.flipY = player_sprite.flipX;

            if (Input.GetKeyDown(KeyCode.Space))
                Shoot();

            if (shoot_cooldown_timer > 0)
                shoot_cooldown_timer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude > 0)
            rb.velocity += -rb.velocity * Time.deltaTime * 5f;
    }

    public void Shoot()
    {
        if (shoot_cooldown_timer > 0)
            return;

        shoot_effect.Play();

        Transform bullet = Instantiate(bullet_prefab).transform;

        bullet.transform.position = shoot_effect.transform.position;

        bullet.rotation = gun_holder.rotation;

        shoot_cooldown_timer = shoot_cooldown;
    }

    public void GetDamage(float damage, Vector3 direction)
    {
        if (player_health > damage)
        {
            player_health -= damage;

            if (health_bar != null)
                health_bar.SetAmount(player_health / 100);

            anim.SetTrigger("damage");

            rb.AddForce(direction * 7.5f, ForceMode2D.Impulse);
        }
        else
        {
            player_health = 0;
            if (health_bar != null)
                health_bar.gameObject.SetActive(false);


            Death();
        }
    }

    public void HealPlayer(float heal)
    {
        player_health += Mathf.Abs(heal);
        player_health = Mathf.Clamp(player_health, 0, 100);

        if (health_bar != null)
            health_bar.SetAmount(player_health / 100);

        anim.SetTrigger("heal");
    }

    private void Death()
    {
        is_player_dead = true;

        player_sprite.enabled = false;
        gun_sprite.enabled = false;

        death_effect.Play();

        game_controller.EndGame();
    }

    public bool IsDead { get { return is_player_dead; } }
}

