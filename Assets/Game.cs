using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; 
using TMPro; 

/*
Controls when rounds should start and what enemies are spawning in 
*/

public class Game : MonoBehaviour
{

    [SerializeField] private GameObject player; 
    [SerializeField] private GameObject[] enemies; 
    private Transform[] spawnPoints; 
    [SerializeField] private Transform[] platforms; 
    private Hashtable currentEnemies; 

    private int roundNumber = 1; 
    private int numberOfEnemies = 10; 
    private int numSpawned = 0; 
    private bool spawn = true; 

    [SerializeField] private GameObject[] itemDisplay; 
    private Hashtable itemReference; 
    private string currentKey = ""; 

    [SerializeField] private GameObject[] health; 

    [SerializeField] private GameObject retryButton; 

    [SerializeField] private CinemachineVirtualCamera zoomOut; 

    private int combo = 0; 

    [SerializeField] private TMP_Text roundText; 


    // Start is called before the first frame update
    void Start()
    {
        currentEnemies = new Hashtable(); 
        spawnPoints = new Transform[platforms.Length]; 
        for(int i = 0; i < platforms.Length; i++) {
            spawnPoints[i] = platforms[i].GetChild(0).transform; 
        }
        itemReference = new Hashtable(); 
        InitialzeItemReference(); 
        player.transform.position = transform.position; 
        retryButton.SetActive(false); 
    }

    // Update is called once per frame
    void Update()
    {

        //roundText.text = "" + roundNumber; 

        if(!player.GetComponent<Movement>().Alive()) {
            EndGame(); 
        }

        // slowly spawns enemies in when the next round starts 
        if(numSpawned != numberOfEnemies && spawn) {
            StartCoroutine(SpawnEnemy()); 
        }
        else if(currentEnemies.Count == 0) {
            numSpawned = 0; 
            numberOfEnemies *= 2; 
            roundNumber++; 
        }

        if(Input.GetKeyDown(KeyCode.Tab)) {
            zoomOut.Priority *= -1; 
        }

        if(combo == 3) {
            combo = 0; 
            player.GetComponent<Movement>().AddHealth(); 
        }
    }

    // initializes the HashTable with all the item icon references 
    // iterates through the list of all items in 5s 
    // makes an array of 5 for those images 
    // adds the 5 to hash table with the name of the item as the reference 
    public void InitialzeItemReference() {
        GameObject[] display; 
        for(int i = 0; i < itemDisplay.Length; i += 5) {
            display = new GameObject[5]; 
            for(int b = 0; b < 5; b++) {
                display[b] = itemDisplay[i + b]; 
                display[b].SetActive(false); 
            }
            string itemName = itemDisplay[i].name; 
            string key = itemName.Substring(0, itemName.IndexOf(" ")); 
            print(key); 
            itemReference.Add(key, display); 
        } 
    }

    // spawns 1 enemy while giving a cooldown between the spawning of the next enemy 
    IEnumerator SpawnEnemy() {
        int rand = Random.Range(0, enemies.Length);
        int randSpawn = Random.Range(0, spawnPoints.Length);
        GameObject enemy = Instantiate(enemies[rand], spawnPoints[randSpawn].position, spawnPoints[randSpawn].rotation);
        enemy.GetComponent<Enemy>().SetId(numSpawned); 
        currentEnemies.Add(enemy.GetComponent<Enemy>().GetId(), enemy); 
        //print(currentEnemies.Count); 
        numSpawned++; 
        spawn = false; 
        yield return new WaitForSeconds(1); 
        spawn = true; 
    }

    // removes given enemy from list of current enemies 
    public void RemoveEnemy(int id) {
        currentEnemies.Remove(id); 
        combo++; 
        StartCoroutine(Combo(combo));
    }

    private IEnumerator Combo(int current) {
        yield return new WaitForSeconds(3); 
        if(combo == current) { // if player didn't kill another enemy within 3 seconds of killing this one reset combo 
            combo = 0; 
        }
    }

    // Displays given item in the game's UI 
    public void DisplayItem(string key) {
        if(!currentKey.Equals("")) {
            // set the current display to false
            SetArray((GameObject[]) itemReference[currentKey], false); 
        }
        currentKey = key; 
        print(key); 
        GameObject[] arr = (GameObject[]) itemReference[key]; 
        // set the current array to true 
        SetArray(arr, true); 
    }

    // as the player throws the item disable one of the images for that item
    // to represent the remaining ammo the player has for given item 
    public void Throw(string key, int index) {
        GameObject[] arr = (GameObject[]) itemReference[key];
        arr[index].SetActive(false); 
    }

    // either displays every item in the array or disables every item in the array 
    void SetArray(GameObject[] arr, bool active) {
        foreach(GameObject item in arr) {
            item.SetActive(active); 
        }
    }

    // Adjust the player ui based on the given input
    public void PlayerHealth(int index, bool active) {
        health[index].SetActive(active); 
    }

    // Screen for when the game ends 
    void EndGame() {
        retryButton.SetActive(true); 
    }

    // checks if the first given position is ahead of the second one
    // returns 1 for a "greater" position, -1 for a lesser positon 
    public int ComparePositions(Vector3 pos1, Vector3 pos2) {
        if(pos1.x > pos2.x) {
            return 1; 
        }
        else 
            return -1; 
    }

    public void Reset() {
        player.GetComponent<Movement>().Reset(); 
        retryButton.SetActive(false); 
        SetArray(health, true); 
        if(currentKey != "")
            SetArray((GameObject[]) itemReference[currentKey], false); 
        foreach(DictionaryEntry entry in currentEnemies) {
            GameObject enemy = (GameObject) entry.Value; 
            Destroy(enemy); 
        }
        currentEnemies = new Hashtable(); 
        roundNumber = 1; 
        numberOfEnemies = 10; 
        player.transform.position = transform.position; 
        spawn = true; 
    }

}
