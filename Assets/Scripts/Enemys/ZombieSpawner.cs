using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> spawn_points;

    [SerializeField] private List<ZombieController> life_zombie_list;

    [SerializeField] private Transform health_bar_holder;

    [SerializeField] private GameObject zombie_prefab;
    [SerializeField] private GameObject health_bar_prefab;

    public void StartGame()
    {
        //for (int x = 0; x < 3; x++)
        //    SpawnZombie();
    }

    private void Update()
    {
        if (life_zombie_list.Count < 3)
            SpawnZombie();
    }


    Transform last_spawn_point = null;
    private void SpawnZombie()
    {
        Transform spawn_point = spawn_points[Random.Range(0, spawn_points.Count - 1)];

        while (spawn_point == last_spawn_point)
            spawn_point = spawn_points[Random.Range(0, spawn_points.Count - 1)];

        Transform zombie = Instantiate(zombie_prefab, new Vector3(1000, 1000, 0),Quaternion.Euler(Vector3.zero)).transform;

        zombie.transform.position = spawn_point.position;

        HealthBar health_bar = Instantiate(health_bar_prefab, new Vector3(1000, 1000, 0), Quaternion.Euler(Vector3.zero)).GetComponent<HealthBar>();

        health_bar.transform.SetParent(health_bar_holder, true);

        zombie.GetComponent<ZombieController>().SetHealthBar(health_bar);

        life_zombie_list.Add(zombie.GetComponent<ZombieController>());

        last_spawn_point = spawn_point;
    }

    public void RemoveZombie(ZombieController zombie) { life_zombie_list.Remove(zombie); }
}
