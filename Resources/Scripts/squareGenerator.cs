using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//if we are using UI, we need to add this to the top of our code
using UnityEngine.UI;

//this is a CLASS
class Box
{
    //with 3 ATTRIBUTES
    public GameObject boxObject;
    public float scale;
    public string name;

    public string toString()
    {
        return "My Box is called" + this.name;
    }

}

public class squareGenerator : MonoBehaviour
{

    // Start is called before the first frame update
    GameObject myBox,cross;

    Text debugTextObject;
    

    //instead of using the gameobject, I could use the class
    Box aBox;

    List<Box> allMyBoxes;
    
    //TASK 1: Modify the coroutine so only ONE box is generate at one point, first the horizontal ones are generated,
    //THEN the vertical ones are generated until the full cross is ready, after which the coroutine stops.

    //TASK 2: Modify this code so that the cross moves and its center cross is always at the edge of the screen.  Once it touches
    //all four edges, it needs to go back to the center of the screen after which the coroutine stops

    //TASK 3: Create a chessboard at one box at a time which covers the whole screen.   


    //SECOND THING TO HAPPEN
    IEnumerator createBoxes()
    {
        allMyBoxes = new List<Box>();
        int counter = 0;
        for (float coord = -4.5f; coord <= 4.5f; coord++)
        {
            //horizontal part of the square
            GameObject horizontalsquare = createSquare(coord, 0);
            horizontalsquare.name = "HSquare:" + coord + "," + 0;
            //apply random colors to the horizontal boxes
            SpriteRenderer boxRenderer = horizontalsquare.GetComponent<SpriteRenderer>();
            boxRenderer.color = Random.ColorHSV();
            boxRenderer.sortingOrder = -1;
            horizontalsquare.transform.SetParent(cross.transform);
            Box horizontalBox = new Box();
            horizontalBox.boxObject = horizontalsquare;
            horizontalBox.name = "HSquare:" + coord + "," + 0;
            allMyBoxes.Add(horizontalBox);

            debugTextObject.text += horizontalBox.toString() + "\n";

            //--------STOP----------
            yield return new WaitForSeconds(0.1f);
            //WAIT FOR 0.1 of a second
        }
        for (float coord = -4.5f; coord <= 4.5f; coord++) { 
            //vertical part of the square
            GameObject verticalsquare = createSquare(0, coord);
            verticalsquare.name = "VSquare:" + 0 + "," + coord;
            verticalsquare.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
            verticalsquare.transform.SetParent(cross.transform);

            Box verticalBox = new Box();
            verticalBox.boxObject = verticalsquare;
            verticalBox.name = "VSquare:" + coord + "," + 0;
            allMyBoxes.Add(verticalBox);

            debugTextObject.text += verticalBox.toString() + "\n";
            //-----------STOP------------
            yield return new WaitForSeconds(0.1f);
            //WAIT for 0.1 of a second
        }
        //to start the second coroutine
        StartCoroutine(moveCross());
        yield return null;
    }

    IEnumerator chessBoard()
    {
        bool alternatey = false;
        for (float ycoord = -4.5f; ycoord <= 4.5f; ycoord++)
        {
            //for each row
            for (float xcoord = -4.5f; xcoord <= 4.5f; xcoord++)
            {
                GameObject sq = createSquare(xcoord, ycoord);
                if (alternatey)
                {
                    if ((Mathf.Floor(xcoord) % 2 == 0) )
                    {
                        sq.GetComponent<SpriteRenderer>().color = Color.black;
                    }
                    else
                    {
                        sq.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                }
                else
                {
                    if (Mathf.Floor(xcoord) % 2 == 0) 
                    {
                        sq.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                    else
                    {
                        sq.GetComponent<SpriteRenderer>().color = Color.black;
                    }
                }
                
                yield return new WaitForSeconds(0.1f);
            }
            alternatey = !alternatey;
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

    IEnumerator changeBoxColor()
    {
        cross.transform.position = new Vector3(0f, 0f);
        foreach (Box b in allMyBoxes)
        {
            b.boxObject.GetComponent<SpriteRenderer>().color = Color.green;
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

    //THIRD THING TO HAPPEN
    IEnumerator moveCross()
    {
        for (float coord = -4.5f; coord <= 4.5f; coord++)
        {
            //leftmost position
            cross.transform.position = new Vector3(coord, 0f);
           //-----------------STOP--------------
            yield return new WaitForSeconds(0.1f);
            //WAIT for 0.1 of a second
        }
        //
        StartCoroutine(animation2());
        //finish
        yield return null;
    }

    //FOURTH THING TO HAPPEN
    IEnumerator animation2()
    {
        StartCoroutine(changeBoxColor());
        yield return null;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            //get the current value of whether or not the text is showing
            bool debugenabled = GameObject.Find("debugText").GetComponent<Text>().enabled;
            //flip its value
            debugenabled = !debugenabled;
            //show or hide the text accordingly
            GameObject.Find("debugText").GetComponent<Text>().enabled = debugenabled;
        }    
    }

    


    //this method happens ONCE, FIRST THING TO HAPPEN
    void Start()
    {
        debugTextObject = GameObject.Find("debugText").GetComponent<Text>();
     
        //go to the Resources folder, then go to the Prefabs subfolder
        //and find the GameObject Square
        myBox = Resources.Load<GameObject>("Prefabs/Square");
        

        cross = new GameObject("Cross");
        cross.transform.position = new Vector3(0f, 0f);
        //cross.AddComponent<crossController>();

        //   StartCoroutine(createBoxes());
        StartCoroutine(chessBoard());
       
            
      
     
    }

    GameObject createSquare(float xpos,float ypos)
    {
       return Instantiate(myBox, new Vector3(xpos, ypos), Quaternion.identity);
    }

}
