using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    private List<ZombieController> life_zombie_list = new List<ZombieController>();

    [Header("General")]
    [SerializeField] private bool spawn_enabled = false;
    [SerializeField] private List<Transform> spawn_points;

    [Space(20)]
    [Header("UI")]
    [SerializeField] private Transform health_bar_holder;

    [Space(20)]
    [Header("Prefabs")]
    [SerializeField] private GameObject zombie_prefab;
    [SerializeField] private GameObject health_bar_prefab;


    private void Start()
    {
        if (spawn_points.Count < 1)
        {
            Debug.LogWarning("No Spawn Points For Zombie!");
            this.enabled = false;
        }
    }
    private void Update()
    {
        if (spawn_enabled & life_zombie_list.Count < 3)
            SpawnZombie();
    }


    private bool IsSpawnPointAllreayUsed(Transform spawn_point)
    {
        foreach (Transform point in last_used_spawn_points)
            if (spawn_point == point)
                return true;

        return false;
    }

    List<Transform> last_used_spawn_points = new List<Transform>();
    private void SpawnZombie()
    {
        Transform spawn_point = spawn_points[Random.Range(0, spawn_points.Count - 1)];

        while(IsSpawnPointAllreayUsed(spawn_point))
            spawn_point = spawn_points[Random.Range(0, spawn_points.Count - 1)];


        Transform zombie = Instantiate(zombie_prefab, new Vector3(1000, 1000, 0),Quaternion.Euler(Vector3.zero)).transform;

        zombie.transform.position = spawn_point.position;

        HealthBar health_bar = Instantiate(health_bar_prefab, new Vector3(1000, 1000, 0), Quaternion.Euler(Vector3.zero)).GetComponent<HealthBar>();

        health_bar.transform.SetParent(health_bar_holder, true);

        zombie.GetComponent<ZombieController>().SetHealthBar(health_bar);

        life_zombie_list.Add(zombie.GetComponent<ZombieController>());

        
        last_used_spawn_points.Add(spawn_point);

        if (last_used_spawn_points.Count > 3)
            last_used_spawn_points.RemoveAt(0);
    }

    public void RemoveZombieFormList(ZombieController zombie) { life_zombie_list.Remove(zombie); }

    public void EnabeleSpawn(bool enable) { spawn_enabled = enable; }
}
