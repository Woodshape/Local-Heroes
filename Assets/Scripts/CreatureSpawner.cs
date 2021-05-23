using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CreatureSpawner : MonoBehaviour
{
    // Grab our prefab
    public GameObject creaturePrefab;
    // Number of creatures to spawn
    public float spawnTime = 10f;
    
    // Start is called before the first frame update
    void Start() {
        // Call the SpawnPatient method for the first time
        Invoke(nameof(SpawnCreature), spawnTime);
    }
    
    private void SpawnCreature() {
        Instantiate(creaturePrefab, this.transform.position, Quaternion.identity);
        
        // Call the SpawnPatient repeatedly
        Invoke(nameof(SpawnCreature), spawnTime);
    }
}
