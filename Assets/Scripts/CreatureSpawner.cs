using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using LH.GOAP;
using UnityEngine;
using UnityEngine.Serialization;

public class CreatureSpawner : MonoBehaviour
{
    // Grab our prefab
    public GameObject creaturePrefab;
    // Number of creatures to spawn
    public float spawnTime = 10f;
    public int maxSpawn = 100;
    
    // Start is called before the first frame update
    void Start() {
        // Call the SpawnPatient method for the first time
        Invoke(nameof(SpawnCreature), spawnTime);
    }
    
    private void SpawnCreature() {
        //  only ever spawn a maximum of 10 entities per type
        if (GWorld.Instance.GetQueue(creaturePrefab.GetComponent<Entity>().type.ToString()).queue.Count < maxSpawn) {
            Instantiate(creaturePrefab, this.transform.position, Quaternion.identity);

            Resource resource = creaturePrefab.GetComponent<Entity>().type;
            Debug.Log($"{resource} level: " + GameWorld.Instance.CalculateAverageLevel(resource)); 
        }
        
        // Call the SpawnPatient repeatedly
        Invoke(nameof(SpawnCreature), spawnTime);
    }
}
