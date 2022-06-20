using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fliehe : SteeringBehaviour
{
    // Start is called before the first frame update
    
    public override void Start()
    {
        base.Start();
    }
    public override List<Vector3> GetVektorenList(){
        List<Objekt> obj = Master.masterListen.alleObjekte; 
        List<Vector3> output = new List<Vector3>();

        foreach (Objekt item in obj)
        {
            
            if(!item.isTarget){
                Vector3 richtung = new Vector3();
                richtung = transform.position-item.gameObject.transform.position ;
                if(richtung.magnitude >maxDistanz || richtung.magnitude < minDistanz){
                    continue;
                }
                output.Add(-richtung);
                simpleVector+=richtung;
                simpleVector.Normalize();
            }
            
            
        }
        return output;
    }

    public override void generateContextMap(List<Vector3> input){
        foreach (Vector3 item in input)
        {
            float strength = 1 - (item.magnitude- minDistanz) / (maxDistanz-minDistanz);
            float magnitude = item.magnitude;
            Vector3 vect =  item / item.magnitude * strength;
            
            for(int i =0; i < gameObject.GetComponent<SteeringAgent>().sensoren.Count; i++){
                Vector3 sensor = gameObject.GetComponent<SteeringAgent>().sensoren[i];
                float winkel = winkelBerechnung(sensor, vect);
                if(winkel>= sensorSensitivität*Mathf.PI/180){
                    continue;
                }
                float endwert = winkelNorm(winkel)*distNorm(magnitude);//oder magnitude von vor vect?
                endwert *= endwert; 
                contextMap[i] = Mathf.Min(contextMap[i], -endwert);
            }
        }

        
    }

    
    


}
