using UnityEngine;

[CreateAssetMenu(fileName = "ItemConfig", menuName = "ScriptableObjects/ItemConfig", order = 1)]
public class ItemConfig : ScriptableObject
{
    [SerializeField] private string id;

    [SerializeField] private string in_game_name;

    [SerializeField] private Sprite icon;

    [SerializeField] private bool is_stackble;


    [SerializeField] private GameObject world_prefab;

    public GameObject WorldObject { get { return world_prefab; } }

    public string Id { get { return id; } }

    public string InGameName { get { return in_game_name; } }

    public Sprite Icon { get { return icon; } }

    public bool isStackble { get { return is_stackble; } }
}
