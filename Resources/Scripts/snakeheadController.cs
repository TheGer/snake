using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snakeheadController : MonoBehaviour
{
    snakeGenerator mysnakegenerator;
    foodGenerator myfoodgenerator;

    private void Start()
    {
        mysnakegenerator = Camera.main.GetComponent<snakeGenerator>();
        myfoodgenerator = Camera.main.GetComponent<foodGenerator>();
    }

    void checkBounds()
    {
        if ((transform.position.x < -(Camera.main.orthographicSize-1)) || (transform.position.x > (Camera.main.orthographicSize - 1)))
        {
            transform.position = new Vector3(-transform.position.x,transform.position.y);
        }

        if ((transform.position.y < -(Camera.main.orthographicSize - 1)) || (transform.position.y > (Camera.main.orthographicSize - 1)))
        {
            transform.position = new Vector3(transform.position.x, -transform.position.y);
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position -= new Vector3(1f,0);
            checkBounds();
            myfoodgenerator.eatFood(this.transform.position);
            Debug.Log(mysnakegenerator.hitTail(this.transform.position, mysnakegenerator.snakelength));

        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1f, 0);
            checkBounds();
            myfoodgenerator.eatFood(this.transform.position);
            Debug.Log(mysnakegenerator.hitTail(this.transform.position, mysnakegenerator.snakelength));
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.position += new Vector3(0, 1f);
            checkBounds();
            myfoodgenerator.eatFood(this.transform.position);
            Debug.Log(mysnakegenerator.hitTail(this.transform.position, mysnakegenerator.snakelength));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.position -= new Vector3(0, 1f);
            checkBounds();
            myfoodgenerator.eatFood(this.transform.position);
            Debug.Log(mysnakegenerator.hitTail(this.transform.position, mysnakegenerator.snakelength));
        }

        //Debug.Log(mysnakegenerator.hitTail(this.transform.position, mysnakegenerator.snakelength)); 
    }
}
