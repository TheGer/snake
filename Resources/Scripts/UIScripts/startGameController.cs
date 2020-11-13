using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class startGameController : MonoBehaviour
{
    GameObject startButton;

    // Start is called before the first frame update
    void Start()
    {
        startButton = Instantiate(Resources.Load<GameObject>("Prefabs/ButtonPrefab"), new Vector3(0f, 0f), Quaternion.identity);

        startButton.GetComponentInChildren<Text>().text = "Start Snake!";

        startButton.GetComponentInChildren<Button>().onClick.AddListener(
                () =>
                {
                    Camera.main.GetComponent<snakeGenerator>().enabled = true;
                    Camera.main.GetComponent<foodGenerator>().enabled = true;
                    startButton.SetActive(false);
                });

     
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
