using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
   
    public float minX;
    public float maxX;
   
    // Start is called before the first frame update
    void Start()
    {
            Vector2 randomPosition = new Vector2(Random.Range(minX, maxX),transform.position.y);
           GameObject myPlayer = (GameObject)PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
