using UnityEngine;

public class WorldItem : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private ItemConfig config;
    [SerializeField] int count;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            FindFirstObjectByType<InventorySystem>().AddItem(config, count);
            Destroy(gameObject);
        }
    }
}
