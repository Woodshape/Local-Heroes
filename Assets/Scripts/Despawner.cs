using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Despawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        Invoke(nameof(Despawn), Random.Range(10f, 30f));
    }

    void Despawn() {
        Destroy(gameObject);
    }
}
