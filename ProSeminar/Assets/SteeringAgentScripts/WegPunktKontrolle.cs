using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WegPunktKontrolle : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject nextWaypoint = null;
    public bool loop = false;

    private SteeringAgent agent;

    private LineRenderer lr;
    private Transform[] points;
    



    void Awake()
    {
        lr=GetComponent<LineRenderer>();
    
        agent = FindObjectOfType<SteeringAgent>();
        SetUpLine();
    }

    // Update is called once per frame
    public void SetUpLine()
    {
        lr.positionCount = 2;
        points = new Transform[2];
        this.points[0]= agent.transform;
        this.points[1] = this.transform;
    }

    private void Update() {
        for(int i=0; i<points.Length; i++){
            lr.SetPosition(i, points[i].position);  
        }
    }


    void OnDisable()
    {
        
        if(nextWaypoint != null){
            nextWaypoint.gameObject.SetActive(true);
            if(!loop){
                Destroy(gameObject);
            }
        }
        
    }
}
