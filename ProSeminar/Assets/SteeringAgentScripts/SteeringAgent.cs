using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringAgent : MonoBehaviour
{
    // Start is called before the first frame update
    [Tooltip("0 no MCO\n1 Weighted\n2 Constraint\n3 Hybrid")]
    [Range(0, 3)]
    
    
    public int methodSwitch = 1;
    [Range(0,1)]
    public float gefahrenGewicht;
    [Range(0,1)]
    public float epsilonConstraint;
    public float speed = 3;
    public int sensorenAnzahl = 8;
    
    public int minWert =0;
    public int collisionCount =0;
    public List<Vector3> sensoren = new List<Vector3>();
    public List<SteeringBehaviour> verhalten = new List<SteeringBehaviour>();
    public List<float> contextMapGefahren= new List<float>();
    public List<float> contextMapInteressen= new List<float>();

    public Vector3 simpleNavigation = new Vector3();


    

    void Awake()
    {
        /*
        sensoren.Add(new Vector3(0,1,0)); //oben
        sensoren.Add(new Vector3(1,1,0)); //rechts oben
        sensoren.Add(new Vector3(1,0,0)); //usw.
        sensoren.Add(new Vector3(1,-1,0));
        
        sensoren.Add(new Vector3(0,-1,0));
        sensoren.Add(new Vector3(-1,-1,0));
        sensoren.Add(new Vector3(-1,0,0));
        sensoren.Add(new Vector3(-1,1,0));
        */
        setSensoren(sensorenAnzahl);
    }
    // Update is called once per frame

    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().color=Color.blue;
        for(int i=0; i< sensoren.Count; i++){
            contextMapGefahren.Add(0.0f);
            contextMapInteressen.Add(0.0f);
        }
        minWert = 0;

    }
    void Update()
    {
        if(Master.masterListen.activeGoal){

            
            for(int i=0; i< sensoren.Count; i++){
                contextMapGefahren[i] = 0.0f;
                contextMapInteressen[i] = 0.0f;
            }
            simpleNavigation=new Vector3();
            //Aggregation
            foreach (SteeringBehaviour verh in verhalten)
            {
                for(int i = 0; i<sensoren.Count; i++){
                    
                    if(verh.contextMap[i]>0){
                        contextMapInteressen[i] = Mathf.Max(contextMapInteressen[i], verh.contextMap[i]);
                    }else{
                        contextMapGefahren[i] = Mathf.Max(contextMapGefahren[i], -verh.contextMap[i]);
                    }
                                
                }
                simpleNavigation+=verh.simpleVector;
            }
            minWert=-1;
            switch(methodSwitch){
                case 1://weighting method
                    for(int i = 1; i<sensoren.Count; i++){
                        if(minWert==-1){
                            minWert=i;
                        }else{
                            if(weighting(i) < weighting(minWert)){
                                minWert=i;
                            }
                        }
                    }
                    break;
                case 2:
                    for(int i = 0; i<sensoren.Count; i++){
                        if(contextMapGefahren[i] <= epsilonConstraint){
                            if(minWert==-1|| -contextMapInteressen[i]<-contextMapInteressen[minWert]){
                                minWert=i;
                            }
                        }
                    }
                    if(minWert==-1){
                        minWert=0;
                        for(int i = 0; i<sensoren.Count; i++){
                            if(contextMapGefahren[i] <= contextMapGefahren[minWert] ||
                             contextMapGefahren[i] == contextMapGefahren[minWert] &&
                              -contextMapInteressen[i]<-contextMapInteressen[minWert]){
                                minWert= i;
                            }
                        }
                    }
                    break;
                case 3:
                    
                    for(int i = 1; i<sensoren.Count; i++){
                        if(contextMapGefahren[i]<=epsilonConstraint){
                            if(minWert==-1){
                                minWert=i;
                            }else{
                                if(weighting(i) < weighting(minWert)){
                                    minWert=i;
                                }
                            }
                        }
                       
                    }
                    if(minWert==-1){
                        minWert=0;
                        for(int i = 0; i<sensoren.Count; i++){
                            if(contextMapGefahren[i] <= contextMapGefahren[minWert] ||
                             contextMapGefahren[i] == contextMapGefahren[minWert] &&
                              -contextMapInteressen[i]<-contextMapInteressen[minWert]){
                                minWert= i;
                            }
                        }
                    }
                    
                    break;
                default:
                    break;
            }
            //Debug.Log(getLeft(minWert)-getRight(minWert));
            //Debug.Log(success);
            Vector3 steeringVector = new Vector3();
            float interpolationPoint =-1;
            if(minWert!=-1){
                
                interpolationPoint = (getValueMapInterest(minWert)-getRight(minWert)-getLeft(minWert))/(getLeft(minWert)-getRight(minWert));
            
                float leftVal= getLeft(minWert)*interpolationPoint+getValueMapInterest(minWert-1);
                float rightVal = getRight(minWert)*interpolationPoint+getValueMapInterest(minWert)-getRight(minWert);
                
                try
                {
                    if(leftVal==rightVal && Mathf.Abs(getLeft(minWert)-getRight(minWert))>0.01){
                       steeringVector=  interpolationPoint*sensoren[getIndexMap(minWert-1)].normalized+(1-interpolationPoint)*sensoren[minWert];
                        steeringVector *= speed*Time.deltaTime;
                    }else{
                        //Debug.Log(minWert +"\n"+interpolationPoint);
                        //Debug.Log(leftVal+", "+rightVal);
                        steeringVector=sensoren[getIndexMap(minWert)].normalized* speed*Time.deltaTime;
                    }
                
                }
                catch (System.Exception)
                {
                    //Debug.Log(e);
                }
            }
            
            if(methodSwitch<1){
                
                simpleNavigation=simpleNavigation.normalized*Time.deltaTime*speed;
                transform.position+=simpleNavigation;
            }else{
                transform.position+=steeringVector;
            }
            if(transform.position.magnitude>100){
                Master.masterListen.activeGoal=false;
                Debug.Log(interpolationPoint);
            }
            //transform.Translate(steeringVector, Space.World);




        }
    }

    private float weighting(int i){
        return gefahrenGewicht*contextMapGefahren[i]-(1-gefahrenGewicht)*contextMapInteressen[i];
    }
    

    private int getIndexMap(int i){
        if(i>= sensoren.Count){
            i-= sensoren.Count;
        }
        while(i<0){
            i+=sensoren.Count;
        }
        
        return i;
    }

    private float getValueMapInterest(int i){
        return contextMapInteressen[getIndexMap(i)];
    }

    private float getLeft(int i){
        return getValueMapInterest(i-1)-getValueMapInterest(i-2);
    }
    private float getRight(int i){
        return getValueMapInterest(i+1)-getValueMapInterest(i);
    }


    private void setSensoren(int n){
        sensoren.Clear();
        if(n==0){
            sensoren.Add(new Vector3(0,0,0));
        }
        float winkel = 2f * Mathf.PI/n;

        for(int i=0; i<n; i++){
            sensoren.Add(new Vector3(Mathf.Cos(winkel*i), Mathf.Sin(winkel*i), 0));
            //Debug.Log(Mathf.Cos(winkel*i)+", "+ Mathf.Sin(winkel*i));
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Objekt coll = other.gameObject.GetComponent<Objekt>();
        if(coll == null){
            Debug.Log("null");
        }else{
            if(!coll.isTarget){
                collisionCount++;
            }
        }
    }


}
