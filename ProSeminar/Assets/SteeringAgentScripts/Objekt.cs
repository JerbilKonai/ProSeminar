using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objekt : MonoBehaviour
{
    public bool isTarget=false;
    private bool secondActivation = false;
    

    // Start is called before the first frame update
    void Start()
    {
        Master.masterListen.alleObjekte.Add(this);
        Master.masterListen.activeGoal = Master.masterListen.activeGoal || isTarget;
        if(isTarget){
            gameObject.GetComponent<SpriteRenderer>().color=Color.green;
        }else{
            gameObject.GetComponent<SpriteRenderer>().color=Color.red;
        } 
    }

    void OnDisable()
    {
        bool updateValue =false;
        for(int i =0; i< Master.masterListen.alleObjekte.Count; i++){
            if(Master.masterListen.alleObjekte[i].isTarget){
                updateValue =true;
                break;
            }
        }
        Master.masterListen.activeGoal = updateValue;
        
    }

    
    
    
    private void OnEnable() {
        if(secondActivation){
            Master.masterListen.alleObjekte.Add(this);
            Master.masterListen.activeGoal = Master.masterListen.activeGoal || isTarget;
            
        }
        secondActivation = true;
    }

    
    
    
}
