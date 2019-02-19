using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float maxSpawnRadius = 15;
    public int maxEnemies = 10;

    public int enemiesKilled;

    public GameObject enemy;
    public GameObject tpsPlayer;
    public GameObject fpsPlayer;

    bool titan;
    bool bCamera = false;
    public bool inGame = false;
    public float _playerHealth { get; set; }

    [SerializeField]
    private GameObject gameUIelements;
    [SerializeField]
    private GameObject cameraMenu, elements, fpsCamera, startButton, healthText, victoryMenu;

    public UnityEngine.UI.Text winLoseText;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {

    }

    public void Init()
    {
        //
        inGame = true;
        cameraMenu.SetActive(false);
        elements.SetActive(false);
        startButton.SetActive(false);
        gameUIelements.SetActive(true);

        fpsCamera.SetActive(true);
        _playerHealth = 100;
        SpawnEnemies();

        Cursor.visible = false;

        Camera.main.GetComponent<FPSCamera>().enabled = true;

        Time.timeScale = 1;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            fpsPlayer.SetActive(bCamera);
            tpsPlayer.SetActive(!bCamera);
            bCamera = !bCamera;
        }

        healthText.GetComponent<UnityEngine.UI.Text>().text = _playerHealth.ToString();

        if (enemiesKilled > maxEnemies / 2 && !titan)
        {
            //Spawn Titan
            GameObject gotitan = Instantiate(enemy, new Vector3(Random.Range(-20,-25),0, Random.Range(20,25)), Quaternion.identity);
            gotitan.GetComponent<EnemyBehaviour>().SetEnemy(3);
            titan = true;
        }
    }

    void SpawnEnemies()
    {

        for (int i = 0; i < maxEnemies; i++)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        float correctPosToSpawnX = Random.Range(-maxSpawnRadius, maxSpawnRadius);
        float correctPosToSpawnY = Random.Range(-maxSpawnRadius, maxSpawnRadius);


        while (correctPosToSpawnX < 5 && correctPosToSpawnX > -5)
        {
            correctPosToSpawnX = Random.Range(-maxSpawnRadius, maxSpawnRadius);
        }
        while (correctPosToSpawnY < 5 && correctPosToSpawnY > -5)
        {
            correctPosToSpawnY = Random.Range(-maxSpawnRadius, maxSpawnRadius);
        }

        int enemyType = Random.Range(0,2);

        GameObject go = Instantiate(enemy);
        Vector3 pos = new Vector3(correctPosToSpawnX, go.transform.localScale.y / 2, correctPosToSpawnY);
        go.transform.position = pos;

        go.GetComponent<EnemyBehaviour>().SetEnemy(enemyType);
    }

    public void NewGame()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject go in temp)
        {
            Destroy(go);
        }

        fpsPlayer.SetActive(false);
        tpsPlayer.SetActive(false);
        cameraMenu.SetActive(true);
        elements.SetActive(true);
        inGame = false;
        startButton.SetActive(true);
   
    }

    public void VictoryLose(bool value)
    {

        if(value)winLoseText.text = "You WIN!";
        if(!value)winLoseText.text = "You LOSE!";

        GameObject[] temp = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject go in temp)
        {
            Destroy(go);
        }

        //Hide gameUI and enable pause menu with victory
        victoryMenu.SetActive(true);
        
        fpsPlayer.SetActive(false);
        tpsPlayer.SetActive(false);
        cameraMenu.SetActive(true);
        elements.SetActive(true);
        gameUIelements.SetActive(false);
        Cursor.visible = true;
        inGame = false;
    }
}
