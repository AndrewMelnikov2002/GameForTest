using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private FixedJoystick joystick;
    private SaveSystem save_system;

    [Header("Main")]
    [SerializeField] [Range(0,100)] private float player_health = 100;
    [SerializeField] private HealthBar health_bar;
    private bool is_player_dead = false;
    private Rigidbody2D rb;

    [SerializeField] private int global_score = 0;

    [Space(20)]
    [Header("Visual")]
    [SerializeField] private SpriteRenderer player_sprite;
    [SerializeField] private ParticleSystem death_effect;



    [Space(20)]
    [Header("Movement")]
    [SerializeField] private float move_speed;


    [Space(20)]
    [Header("Gun")]
    [SerializeField] private SpriteRenderer gun_sprite;
    [SerializeField] private Transform gun_holder;
    [Space(10)]
    [SerializeField] private ParticleSystem shoot_effect;
    [Space(10)]
    [SerializeField] private GameObject bullet_prefab;


    //[Space(20)]
    //[Header("Sounds")]
    //[SerializeField] private AudioClip shoot_sound;

    //[SerializeField] private AudioClip damage_sound;

    //[SerializeField] private AudioClip death_sound;

    //private AudioSource audio_source;

    [Space(20)]
    [Header("Other")]
    [SerializeField] private Blackscreen black_screen;
    [SerializeField] private RestartScreen restart_screen;

    [SerializeField] private Text score_text;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();

        joystick = FindFirstObjectByType<FixedJoystick>();

        save_system = GetComponent<SaveSystem>();

        //audio_source = GetComponent<AudioSource>();

        if (joystick == null)
            Debug.LogError("Not Found FixedJoystik!");

        FindFirstObjectByType<CameraTracking>().SetTarget(transform);
        rb = GetComponent<Rigidbody2D>();

        if (health_bar != null)
        {
            health_bar.SetTarget(transform);
            health_bar.SetAmount(1);
        }
        else
            Debug.LogWarning("No player health bar!");

        black_screen = FindFirstObjectByType<Blackscreen>();

        SaveData save = save_system.LoadProgress();

        if(save != null)
        {
            global_score = save.score;

            score_text.text = "Global Score: " + global_score;

            FindFirstObjectByType<InventorySystem>().LoadInventory(save.inventory);
        }
    }

    float save_timer;

    float gun_angle = 0;
    Vector3 move_direction = Vector3.zero;
    private void Update()
    {
        if (rb.velocity.magnitude > 0)
            rb.velocity += -rb.velocity * Time.deltaTime * 5f;

        if (!is_player_dead)
        {
            if (joystick.Direction != Vector2.zero)
            {
                gun_angle = Vector2.SignedAngle(Vector2.right, joystick.Direction);
                move_direction = joystick.Direction;

                transform.position += move_direction * Time.deltaTime * move_speed;
            }

            if (save_timer > 0)
                save_timer -= Time.deltaTime;
            else
            {
                save_timer = 3f;

                save_system.SaveProgress(global_score,FindFirstObjectByType<InventorySystem>().Inventory);
            }


            anim.SetBool("is_walk", joystick.Direction != Vector2.zero);

            anim.SetFloat("walk_speed", joystick.Direction.magnitude);

            player_sprite.flipX = move_direction.x < 0;

            Quaternion gun_rotation = Quaternion.Lerp(gun_holder.rotation, Quaternion.Euler(0, 0, gun_angle), Time.deltaTime * 4f);

            gun_holder.rotation = gun_rotation;

            gun_sprite.flipY = player_sprite.flipX;

            if (Input.GetKeyDown(KeyCode.Space))
                Shoot();

            if (Input.GetKeyDown(KeyCode.F7))
                save_system.CleanSave();

            if (shoot_cooldown > 0)
                shoot_cooldown -= Time.deltaTime;
        }
    }


    private float shoot_cooldown = 0;
    public void Shoot()
    {
        if (shoot_cooldown > 0)
            return;

        shoot_effect.Play();

        Transform bullet = Instantiate(bullet_prefab).transform;

        bullet.transform.position = shoot_effect.transform.position;

        bullet.rotation = gun_holder.rotation;

        shoot_cooldown = 0.5f;

        //audio_source.PlayOneShot(shoot_sound);
    }


    private void Death()
    {
        is_player_dead = true;

        player_sprite.enabled = false;
        gun_sprite.enabled = false;

        death_effect.Play();

        black_screen.gameObject.SetActive(true);
        black_screen.FadeIn();

        StartCoroutine(ShowRestartScreen());
    }

    public void GetDamage(float damage, Vector3 direction)
    {


        if(player_health > damage)
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

    public void Restart()
    {
        Debug.Log("Restarting...");
        SceneManager.LoadScene(0);
    }

    IEnumerator ShowRestartScreen()
    {
        yield return new WaitForSeconds(3f);

        restart_screen.gameObject.SetActive(true);
        restart_screen.SetAlpha(0);
    }

    public bool IsDead { get { return is_player_dead; } }

    public void AddScore() { global_score += 1; score_text.text = "Global Score: " + global_score; }


}

[System.Serializable]
public class SaveData 
{
    public int score;

    public List<InventoryItem> inventory;
}

