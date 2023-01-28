using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodMeter : MonoBehaviour
{
    [SerializeField] GameObject player;
    private Vector3 offset = new Vector2(0, 2);
    public int bloodValue;

    // Start is called before the first frame update
    void Start()
    {
        //bloodValue = 10;
        GetComponent<Slider>().value = bloodValue;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = (player.transform.position + offset);
    }

    public void RecoverBlood(int bloodToRecover)
    {
        bloodValue += bloodToRecover;
        if (bloodValue > 10)
        {
            bloodValue = 10;
        }
        GetComponent<Slider>().value = bloodValue;
    }

    public void SubtractBlood(int bloodToSubtract)
    {
        bloodValue -= bloodToSubtract;
        if (bloodValue < 0)
        {
            bloodValue = 0;
        }
        GetComponent<Slider>().value = bloodValue;
    }
}
