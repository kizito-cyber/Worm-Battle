using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public int readyToSHoot = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Shoot()
    {
      
        StartCoroutine(StartCooldown());
       
    }
    IEnumerator StartCooldown()
    {

        Wormy.isFire = true;
        yield return new WaitForSeconds(readyToSHoot);
        Wormy.isFire = false;
    }
}
