using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public List<float> contextMap = new List<float>();
    public float maxDistanz = 3;
    public float minDistanz = 0;
    public float sensorSensitivität=50.0f;
    public Vector3 simpleVector = new Vector3();

    
    public virtual void Start() {
        for(int i=0; i< gameObject.GetComponent<SteeringAgent>().sensoren.Count; i++){
            contextMap.Add(0.0f);
        }
        gameObject.GetComponent<SteeringAgent>().verhalten.Add(this);
    }
    public virtual List<Vector3> GetVektorenList(){
        List<Objekt> obj =  Master.masterListen.alleObjekte;
        return new List<Vector3>();
    }
    

    public virtual void generateContextMap(List<Vector3> input){

    }
    

    public float winkelBerechnung(Vector3 vectA, Vector3 vectB){
        return Mathf.Acos((vectA.x*vectB.x + vectA.y*vectB.y)/vectA.magnitude/vectB.magnitude);
    }

    public virtual float winkelNorm(float winkelIn){
        return 1-winkelIn/sensorSensitivität;
    }

    public virtual float distNorm(float distIn){
        return 1-distIn/maxDistanz;
    }



    void Update()
    {
        for(int i=0; i<contextMap.Count; i++){
            contextMap[i]=0.0f;
        }
        
        generateContextMap(GetVektorenList());
    }
}
