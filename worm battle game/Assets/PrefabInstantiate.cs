using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabInstantiate : MonoBehaviour
{
    public GameObject prefab;
    public float minX;
    public float maxX;
    // Start is called before the first frame update
    void Start()
    {
        Vector2 pos = new Vector2(Random.Range(minX, maxX), transform.position.y);
        Instantiate(prefab, pos, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
