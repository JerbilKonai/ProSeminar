using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master : MonoBehaviour
{
    public static Master masterListen;
    public List<Objekt> alleObjekte = new List<Objekt>(); 
    public bool activeGoal;

    void Awake()
    {
        if(masterListen==null){
            DontDestroyOnLoad(gameObject);
            masterListen=this;
            
        }else if(masterListen!=this){
            masterListen.alleObjekte.Clear();
            Destroy(gameObject);
        }
    }
}
