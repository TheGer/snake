using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snakeheadController : MonoBehaviour
{
    snakeGenerator mysnakegenerator;

    private void Start()
    {
        mysnakegenerator = Camera.main.GetComponent<snakeGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position -= new Vector3(1f,0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1f, 0);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.position += new Vector3(0, 1f);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.position -= new Vector3(0, 1f);
        }

        //Debug.Log(mysnakegenerator.hitTail(this.transform.position, mysnakegenerator.snakelength)); 
    }
}
