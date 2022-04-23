using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Avoid : AgentenBehavior
{
    public override Bewegung GetBewegung()
    {
        Bewegung bew = new Bewegung();
        if(!target.Any()){
            bew.direction = new Vector3(0,0,0);
            return bew;
        }
        
        bew.direction = target[0].transform.position - transform.position;
        bew.direction.Normalize();
        bew.direction = bew.direction * agent.maxSpeed * (-1.0f);
        return bew;
    }

    public override List<Bewegung> GetBewegungsList()
    {
        List<Bewegung> bewList = new List<Bewegung>();
        if(!target.Any()){
            Bewegung bew = new Bewegung();
            bew.direction = new Vector3(0,0,0);
            bewList.Add(bew);
            return bewList;
        }else{
            foreach (GameObject tar in target)
            {
                Bewegung bewEintrag =  new Bewegung();
                bewEintrag.direction = tar.transform.position - transform.position;
                bewEintrag.direction.Normalize();
                bewEintrag.direction = bewEintrag.direction * agent.maxSpeed * (-1.0f);
                bewList.Add(bewEintrag);
            }
            return bewList;
        }
        
    }
}
