using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class tireOffRoad : MonoBehaviour
{
    public Text FrontLeft;
    public Text FrontRight;
    public Text BackLeft;
    public Text BackRight;

    // Start is called before the first frame update
    void Start()
    {
        FrontLeft.text = "FL";
        FrontRight.text = "FR";
        BackLeft.text = "BL";
        BackRight.text = "BR";
        FrontLeft.color = Color.green;
        FrontRight.color = Color.green;
        BackLeft.color = Color.green;
        BackRight.color = Color.green;
    }

    public void changeText(string tire, bool on)
    {
        Debug.Log(string.Format("tire {0}", on));
        if (tire.ToLower() == "front left")
        {
            changeColor(FrontLeft, on);
        }
        else if (tire.ToLower() == "front right")
        {
            changeColor(FrontRight, on);
        }
        else if (tire.ToLower() == "back left")
        {
            changeColor(BackLeft, on);
        }
        else if (tire.ToLower() == "back right")
        {
            changeColor(BackRight, on);
        }
    }

    void changeColor(Text tire, bool on)
    {
        if (on == false)
        {
            tire.color = Color.green;
        }
        else
        {
            tire.color = Color.red;
        }
    }
}
