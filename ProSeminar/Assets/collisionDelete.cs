using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionDelete : MonoBehaviour
{
    // Start is called before the first frame update
    void OnCollisionEnter2D(Collision2D other)
    {
        SteeringAgent agentTest = other.gameObject.GetComponent<SteeringAgent>();
        if(agentTest != null){
            Master.masterListen.alleObjekte.Remove(gameObject.GetComponent<Objekt>());
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }

}
