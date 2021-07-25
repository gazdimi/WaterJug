using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Breadth_First_Search : MonoBehaviour
{
    private static GameObject SmallBottle;
    private static GameObject BigBottle;

    public static State Search(GameObject _smallBottle, GameObject _bigBottle, int smaller, int larger, int target) 
    {
        SmallBottle = _smallBottle;
        BigBottle = _bigBottle;

        SmallBottle.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = smaller.ToString();
        BigBottle.transform.Find("Canvas").Find("TotalCapacity").GetComponent<Text>().text = larger.ToString();

        SmallBottle.transform.GetChild(2).GetChild(1).GetComponent<Text>().text = "8"; //testing

        Debug.Log("SMALLER -->" + smaller); //testing
        Debug.Log("LARGER -->" + larger); //testing
        return null;
    }

    public class State { 
    
    }
}
