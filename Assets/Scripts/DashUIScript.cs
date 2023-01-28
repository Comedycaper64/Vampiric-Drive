using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashUIScript : MonoBehaviour
{
    [SerializeField] GameObject player;
    private Vector3 offset = new Vector2(0, -2);
    public int dashNumber;

    // Start is called before the first frame update
    void Start()
    {
        dashNumber = 1;   
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = (player.transform.position + offset);
        GetComponent<Text>().text = "Dashes: " + player.GetComponent<PlayerController>().dashNumber;
    }
}
