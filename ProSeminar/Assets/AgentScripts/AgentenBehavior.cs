using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class AgentenBehavior : MonoBehaviour
{
    public float gewicht= 1.0f;
    public List<GameObject> target;
    protected Agent agent;
    public Vector3 dest;
    // Start is called before the first frame update
    public virtual void Start()
    {
        
        agent = gameObject.GetComponent<Agent>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(target.Count<=1){
            agent.Bewegungsvektor(GetBewegung(), gewicht);
        }else{
            agent.Bewegungsvektor(combineDirectionsMittel(GetBewegungsList()), gewicht);
        }
        
        
        
    }

    public virtual Bewegung GetBewegung(){
        return new Bewegung();
    }
    public virtual List<Bewegung> GetBewegungsList(){
        return new List<Bewegung>();
    }

    public Bewegung combineDirectionsMittel(List<Bewegung> bewegungsListe){
        Bewegung bewOutput = new Bewegung();
        foreach (Bewegung bew in bewegungsListe)
        {
            bewOutput.direction += bew.direction;
        }
        bewOutput.direction /= bewegungsListe.Count;
        return bewOutput; 
    }
}
