using UnityEngine;

public class BorderScaling : MonoBehaviour
{
    [Tooltip("0 Center, 1 Top, 2 Right, 3 Bottom, 4 Left")]
    [Range(0,4)]
    public int positioning;
    private float camX;
    private float camY;
    Camera cam;
    private BoxCollider2D border;

    [MinAttribute(0.1f)]
    public float thickness=1;

    public bool outerBorder=false;

    // Start is called before the first frame update
    void Start()
    {   if(positioning != 0){
            border  = this.GetComponent<BoxCollider2D>();
        }
        /*
        Screen ist nur für UI-Elemente gut, für Objekte in der Szene ist für uns die feste Kamera besser.
        */
        //float screenWidth = Screen.width/2;
        //float screenHeight = Screen.height/2;
        //Debug.Log(screenWidth);
        //Debug.Log(screenHeight);
        cam = Camera.main;
        Vector3 umrechner=cam.ScreenToWorldPoint(cam.transform.position); //punkt links unten in der ecke der Kamera/ nur negative Werte
        //umrechnung um auch bei verschiebung der Kamera die Werte der unteren linken Ecke zu bekommen und nicht zu große/kleine Grenzen zu ziehen.
        camX = umrechner.x-cam.transform.position.x;
        camY = umrechner.y-cam.transform.position.y;
        if(positioning == 0){
            transform.position = new Vector3(cam.transform.position.x,cam.transform.position.y,0);
        }else {
            if(positioning == 1){
                if(outerBorder){
                    transform.position = new Vector3(0, -camY+thickness, 0);//setze das tragende Objekt auf die entsprechende Kamerakante
                    border.offset =  new Vector2(0, thickness/2);//verschiebe den Mittelpunkt des Colliders ein Stück weiter nach außen
                    border.size = new Vector2((-camX+thickness*2)*2, thickness);// setze den Collider auf gegebene Dimension, sodass dieser mit Collidern an anliegenden Kanten sich in den Ecken überschneidet
                }else{
                    transform.position = new Vector3(0, -camY, 0);//setze das tragende Objekt auf die entsprechende Kamerakante
                    border.offset =  new Vector2(0, thickness/2);//verschiebe den Mittelpunkt des Colliders ein Stück weiter nach außen
                    border.size = new Vector2((-camX+thickness)*2, thickness);// setze den Collider auf gegebene Dimension, sodass dieser mit Collidern an anliegenden Kanten sich in den Ecken überschneidet
                }
              }else if(positioning == 2){
                  if(outerBorder){
                    transform.position = new Vector3(-camX+thickness,0, 0);//setze das tragende Objekt auf die entsprechende Kamerakante
                    border.offset =  new Vector2( thickness/2,0);//verschiebe den Mittelpunkt des Colliders ein Stück weiter nach außen
                    border.size = new Vector2( thickness, (-camY+thickness*2)*2);// setze den Collider auf gegebene Dimension, sodass dieser mit Collidern an anliegenden Kanten sich in den Ecken überschneidet
                }else{
                    transform.position = new Vector3(-camX, 0, 0);
                    border.offset =  new Vector2(thickness/2,0);
                    border.size = new Vector2(thickness, (-camY+thickness) * 2);
                }
            }else if(positioning == 3){
                if(outerBorder){
                    transform.position = new Vector3(0, camY-thickness, 0);//setze das tragende Objekt auf die entsprechende Kamerakante
                    border.offset =  new Vector2( 0,-thickness/2);//verschiebe den Mittelpunkt des Colliders ein Stück weiter nach außen
                    border.size = new Vector2(  (-camX+thickness*2)*2, thickness);// setze den Collider auf gegebene Dimension, sodass dieser mit Collidern an anliegenden Kanten sich in den Ecken überschneidet
                }else{
                    transform.position = new Vector3(0, camY, 0);
                    border.offset =  new Vector2(0,-thickness/2);
                    border.size = new Vector2((-camX+thickness)*2, thickness);
                }

            }else if(positioning == 4){
                if(outerBorder){
                    transform.position = new Vector3(camX-thickness,0, 0);//setze das tragende Objekt auf die entsprechende Kamerakante
                    border.offset =  new Vector2( -thickness/2,0);//verschiebe den Mittelpunkt des Colliders ein Stück weiter nach außen
                    border.size = new Vector2( thickness, (-camY+thickness*2)*2);// setze den Collider auf gegebene Dimension, sodass dieser mit Collidern an anliegenden Kanten sich in den Ecken überschneidet
                }else{
                    transform.position = new Vector3(camX, 0, 0);
                    border.offset =  new Vector2(-thickness/2, 0);
                    border.size = new Vector2(thickness,(-camY+thickness) * 2);
                }
            }
            border.enabled = true; // BoxCollider anfangs deaktiviert, um zu verhindern dass sie mit etwas kollidieren bevor sie an die kanten gesetzt werden konnten.
        }

        

    }


}

