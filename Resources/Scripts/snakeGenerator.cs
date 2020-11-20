using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class positionRecord

{


    //the place where I've been
    Vector3 position;
    //at which point was I there?
    int positionOrder;

    //positionRecord{positionorder = 1, position = new Vector3(1f,1f);}

    //positionRecord2{positionorder = 1, position = new Vector3(1.5f,1f);}

    //positionRecord == positionRecord2

    //List<positionRecord>() myList

    //positionRecord1,positionRecord2

    //myList.Contains(positionRecord2); == true



    

    GameObject breadcrumbBox;

    public void changeColor()
    {
        this.BreadcrumbBox.GetComponent<SpriteRenderer>().color = Color.black;
    }


//==
//1.Equals(1) = true

    //this method exists in every object in C sharp


    public override bool Equals(System.Object obj)
    {
        if (obj == null)
            return false;
        positionRecord p = obj as positionRecord;
        if ((System.Object)p == null)
            return false;
        return position == p.position;
    }


    public bool Equals(positionRecord o)
    {
        if (o == null)
            return false;

        
            //the distance between any food spawned
            return Vector3.Distance(this.position,o.position) < 2f;
       
       
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }




    public Vector3 Position { get => position; set => position = value; }
    public int PositionOrder { get => positionOrder; set => positionOrder = value; }
    public GameObject BreadcrumbBox { get => breadcrumbBox; set => breadcrumbBox = value; }
}

public class snakeGenerator : MonoBehaviour
{

    public int snakelength;

    foodGenerator fgen;
    snakeheadController snakeController;


    int pastpositionslimit = 100;

    GameObject playerBox,breadcrumbBox,pathParent,timerUI;

    List<positionRecord> pastPositions;

    int positionorder = 0;

    bool firstrun = true;


    Color snakeColor;



    IEnumerator waitToGenerateFood()
    {
        while (true)
        {
            if (!firstrun)
            {
                StartCoroutine(fgen.generateFood());
                break;
            }

            yield return null;

        }
        yield return null;
    }


    // Start is called before the first frame update
    void Start()
    {

        snakeColor = Color.green;

        playerBox = Instantiate(Resources.Load<GameObject>("Prefabs/Square"), new Vector3(0f, 0f), Quaternion.identity);

        timerUI = Instantiate(Resources.Load<GameObject>("Prefabs/Timer"), new Vector3(0f, 0f), Quaternion.identity);

        //the default value for the timer is started
        timerUI.GetComponentInChildren<timerManager>().timerStarted = true;

       


        pathParent = new GameObject();

        pathParent.transform.position = new Vector3(0f, 0f);

        pathParent.name = "Path Parent";

        
        breadcrumbBox = Resources.Load <GameObject>("Prefabs/Square");

        playerBox.GetComponent<SpriteRenderer>().color = Color.black;

        //move the box with the arrow keys
        playerBox.AddComponent<snakeheadController>();

        playerBox.name = "Black player box";

        pastPositions = new List<positionRecord>();

        fgen = Camera.main.GetComponent<foodGenerator>();

        StartCoroutine(waitToGenerateFood());

        drawTail(snakelength);
       
    }

    //TASK 1: create a coroutine based on this code that when the X key is pressed, the box is going to go through
    //all its past positions, until it gets to the beginning. The speed should be one position every second
    IEnumerator reverseMoves()
    {
        //1. check that we have more than 5 moves, otherwise don't run
        if (pastPositions.Count < 5)
        {
            yield return null;
        }


        //2. reverse the list of moves, to get the moves I want to go back to
        List<positionRecord> reversedPositions = new List<positionRecord>();

        reversedPositions = pastPositions;

        reversedPositions.Reverse();


        //3. iterate through the moves, waiting for one second every move
        foreach(positionRecord p in reversedPositions)
        {
            playerBox.transform.position = p.Position;

            yield return new WaitForSeconds(1f);
        }


        yield return null;
    }


    //TASK 2: We want to show or hide the trail we have just created.  What would be a good way of doing this?




    //TASK 3: We want to stop our box from going out of the camera.  
    //What would be an optimized way of making sure this doesn't happen
    //Mathf.Clamp is a very interesting function

    //TASK 4: Only generate the red breadcrumbs if you REALLY need them


    //TASK 5: Create a coroutine to move the black box all the way around the screen and back to the center of the screen once a full
    //circuit has been completed.  This should be trigged by pressing the space bar. 

    //TASK 5b: Make sure that all the boxes generated are children of an object called PathParent

    //TASK 6: Implement a button once you click it the snake appears and the timer starts.

    //TASK 7: Implement a coroutine that generates up to 6 blocks every random seconds between 6 and 10 at positions which are
    //rounded off to the nearest decimal.  Make sure that food cannot spawn on top of a past spawned food. 



    IEnumerator Task5()
    {
        //this takes me to the edge of the screen
        float xpos = 0f;
        while(xpos < 10f)
        {
            Debug.Log(xpos);
            playerBox.transform.position += new Vector3(1f, 0f);
            xpos++;
            savePosition();
            yield return new WaitForSeconds(0.1f);
        }

        
        yield return null;
    }
    

    // Update is called once per frame

    bool boxExists(Vector3 positionToCheck)
    {
        //foreach position in the list

        foreach (positionRecord p in pastPositions)
        {
            
            if (p.Position == positionToCheck)
            {
                Debug.Log(p.Position + "Actually was a past position");
                if (p.BreadcrumbBox != null)
                {
                    Debug.Log(p.Position + "Actually has a red box already");
                    //this breaks the foreach so I don't need to keep checking
                    return true;
                }
            }
        }

        return false;
    }


    void savePosition()
    {
        positionRecord currentBoxPos = new positionRecord();

        currentBoxPos.Position = playerBox.transform.position;
        positionorder++;
        currentBoxPos.PositionOrder = positionorder;

        if (!boxExists(playerBox.transform.position))
        {
            currentBoxPos.BreadcrumbBox = Instantiate(breadcrumbBox, playerBox.transform.position, Quaternion.identity);

            currentBoxPos.BreadcrumbBox.transform.SetParent(pathParent.transform);

            currentBoxPos.BreadcrumbBox.name = positionorder.ToString();

            currentBoxPos.BreadcrumbBox.GetComponent<SpriteRenderer>().color = Color.red;

            currentBoxPos.BreadcrumbBox.GetComponent<SpriteRenderer>().sortingOrder = -1;
        }

        pastPositions.Add(currentBoxPos);
        Debug.Log("Have made this many moves: " + pastPositions.Count);
       
    }


    void cleanList()
    {
        for(int counter = pastPositions.Count - 1 ; counter > pastPositions.Count;counter--)
        {
            pastPositions[counter] = null;
        }
    }

    
    public void changeSnakeColor(int length,Color color)
    {
        int tailStartIndex = pastPositions.Count - 1;
        int tailEndIndex = tailStartIndex - length;
        
        snakeColor = color;

        for (int snakeblocks = tailStartIndex;snakeblocks>tailEndIndex;snakeblocks--)
        {
        
            pastPositions[snakeblocks].BreadcrumbBox.GetComponent<SpriteRenderer>().color = color;
        }
    }

    void drawTail(int length)
    {
        clearTail();

        if (pastPositions.Count>length)
        {
            //nope
            //I do have enough positions in the past positions list
            //the first block behind the player
            int tailStartIndex = pastPositions.Count - 1;
            int tailEndIndex = tailStartIndex - length;
          

            //if length = 4, this should give me the last 4 blocks
            for (int snakeblocks = tailStartIndex;snakeblocks>tailEndIndex;snakeblocks--)
            {
                //prints the past position and its order in the list
                //Debug.Log(pastPositions[snakeblocks].Position + " " + pastPositions[snakeblocks].PositionOrder);

                Debug.Log(snakeblocks);

                pastPositions[snakeblocks].BreadcrumbBox = Instantiate(breadcrumbBox, pastPositions[snakeblocks].Position, Quaternion.identity);
                pastPositions[snakeblocks].BreadcrumbBox.GetComponent<SpriteRenderer>().color = snakeColor;

            }

        } 

        if (firstrun)
        {
            
            //I don't have enough positions in the past positions list
            for(int count =length;count>0;count--)
            {
                positionRecord fakeBoxPos = new positionRecord();
                float ycoord = count * -1;
                fakeBoxPos.Position = new Vector3(0f, ycoord);
               // Debug.Log(new Vector3(0f, ycoord));
                //fakeBoxPos.BreadcrumbBox = Instantiate(breadcrumbBox, fakeBoxPos.Position, Quaternion.identity);
                //fakeBoxPos.BreadcrumbBox.GetComponent<SpriteRenderer>().color = Color.yellow;
                pastPositions.Add(fakeBoxPos);

                 


            }
            firstrun = false;
            drawTail(length);
            //Debug.Log("Not long enough yet");
        }

    }


    //if hit tail returns true, the snake has hit its tail
    public bool hitTail(Vector3 headPosition, int length)
    {
        int tailStartIndex = pastPositions.Count - 1;
        int tailEndIndex = tailStartIndex - length;

        //I am checking all the positions in the tail of the snake
        for (int snakeblocks = tailStartIndex; snakeblocks > tailEndIndex; snakeblocks--)
        {
            if ((headPosition == pastPositions[snakeblocks].Position) && (pastPositions[snakeblocks].BreadcrumbBox != null))
            {
              //  Debug.Log("Hit Tail");
                return true;
            }
        }


       return false;

    }



    void clearTail()
    {
        cleanList();
        foreach (positionRecord p in pastPositions)
        {
           // Debug.Log("Destroy tail" + pastPositions.Count);
            Destroy(p.BreadcrumbBox);
        }
    }


  


    void Update()
    {
        if (Input.anyKeyDown && !((Input.GetMouseButtonDown(0)
            || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))) && !Input.GetKeyDown(KeyCode.X) && !Input.GetKeyDown(KeyCode.Z) && !Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("a key was pressed "+Time.time);
          
            savePosition();

            //draw a tail of length 4
            drawTail(snakelength);



        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            StartCoroutine(reverseMoves());
        }


        if (Input.GetKeyDown(KeyCode.Z))
        {
            clearTail();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(snakeController.automoveCoroutine());
          //  StartCoroutine(Task5());
        }


    }
}
