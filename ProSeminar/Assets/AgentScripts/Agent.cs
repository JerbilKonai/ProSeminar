using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public float maxSpeed = 1.0f;
    public float minSpeed = 1.0f;
    public Vector3 Geschwindigkeit;
    protected Bewegung bewegung;


    private List<Vector3> sensoren = new List<Vector3>();
    private List<AgentenBehavior> verhaltensliste = new List<AgentenBehavior>();
    public float sensitivität = 45.0f;


    public GameObject[] objects;
    // Start is called before the first frame update
    void Start()
    {
        Geschwindigkeit= Vector3.zero;
        bewegung = new Bewegung();
        lookupObjects();
        sensoren.Add(new Vector3(0,1,0)); //oben
        sensoren.Add(new Vector3(1,1,0)); //rechts oben
        sensoren.Add(new Vector3(1,0,0)); //usw.
        sensoren.Add(new Vector3(1,-1,0));
        
        sensoren.Add(new Vector3(0,-1,0));
        sensoren.Add(new Vector3(-1,-1,0));
        sensoren.Add(new Vector3(-1,0,0));
        sensoren.Add(new Vector3(-1,1,0));
        foreach (AgentenBehavior item in gameObject.GetComponents<AgentenBehavior>())
        {
            verhaltensliste.Add(item);
        }
        Debug.Log(objects.Length);
        Debug.Log(verhaltensliste.Count);
    }

    void lookupObjects(){
        objects = GameObject.FindGameObjectsWithTag("Objekt");
    }


    public void Bewegungsvektor(Bewegung bew, float gewicht){
        this.bewegung.direction += (bew.direction * gewicht);
    }

    

    // Update is called once per frame
    void Update()
    {
        //sensorenCalc();
        Vector3 displacement = Geschwindigkeit * Time.deltaTime;
        this.transform.position += displacement;
        //transform.Translate(displacement, Space.World);
    }

    void LateUpdate()
    {
        Geschwindigkeit = bewegung.direction *Time.deltaTime;
        if(Geschwindigkeit.magnitude > maxSpeed){
            Geschwindigkeit.Normalize();
            Geschwindigkeit = Geschwindigkeit*maxSpeed;
        }
        if(bewegung.direction.magnitude == 0.0f){
            Geschwindigkeit = Vector3.zero;
        }else if(Geschwindigkeit.magnitude < minSpeed){
            Geschwindigkeit.Normalize();
            Geschwindigkeit = Geschwindigkeit*minSpeed;
        }
        bewegung = new Bewegung();
    }

    
    
}
