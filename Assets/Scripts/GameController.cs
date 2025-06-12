using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private SaveSystem save_system;
    private InventorySystem inventory_system;
    private ZombieSpawner zombie_spawner;

    private bool game_started = false;


    [Header("General")]
    [SerializeField] private int global_score;
    
    [Space(20)]
    [Header("UI")]
    [SerializeField] private CanvasGroupFade blackscreen_fade;
    [SerializeField] private CanvasGroupFade start_screen_fade;
    [SerializeField] private CanvasGroupFade restart_screen_fade;
    [SerializeField] private Text score_text;

    [Space(20)]
    [Header("Saving")]
    [SerializeField] private bool is_autosave_enabled = true;
    [SerializeField] private float autosave_time = 3f;
    private float autosave_timer;

    private void Start()
    {
        save_system = FindFirstObjectByType<SaveSystem>();
        inventory_system = FindFirstObjectByType<InventorySystem>();
        zombie_spawner = FindFirstObjectByType<ZombieSpawner>();

        blackscreen_fade.StartFadeOut(0.25f);

        autosave_timer = autosave_time;

        SaveData save = save_system.LoadProgress();

        if(save != null)
        {
            inventory_system.LoadInventory(save.inventory);

            global_score = save.score;

            score_text.text = "Global Score: " + global_score;
        }
    }

    private void Update()
    {
        if (is_autosave_enabled)
        {
            autosave_timer -= Time.deltaTime;

            if(autosave_timer <= 0)
            {
                save_system.SaveProgress(global_score, inventory_system.Inventory);
                autosave_timer = autosave_time;
            }
        }

        if (Input.GetKeyDown(KeyCode.F7))
        {
            save_system.CleanSave();
            RestartLevel();
        }
    }

    public void StartGame()
    {
        if (game_started)
            return;

        game_started = true;

        zombie_spawner.EnabeleSpawn(true);

        start_screen_fade.StartFadeOut(1.25f);
    }


    public void EndGame()
    {
        zombie_spawner.EnabeleSpawn(false);

        blackscreen_fade.StartFadeIn(0.25f);

        StartCoroutine(ShowRestartScreen());
    }

    IEnumerator ShowRestartScreen() 
    {
        yield return new WaitForSeconds(3f);

        restart_screen_fade.StartFadeIn(0.25f);
    }


    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

    public int GlobalScore { get { return global_score; } set { global_score = value; score_text.text = "Global Score: " + global_score; } }
}
