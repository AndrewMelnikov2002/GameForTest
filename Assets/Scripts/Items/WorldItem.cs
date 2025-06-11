using UnityEngine;

public class WorldItem : MonoBehaviour
{
    [SerializeField] private ItemConfig config;
    [SerializeField] int count;
    //[SerializeField] AudioClip pick_up_sound;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.GetComponent<InventorySystem>().AddItem(config, count);

            //FindFirstObjectByType<PlayerController>().GetComponent<AudioSource>().PlayOneShot(pick_up_sound);

            Destroy(gameObject);
        }
    }
}
