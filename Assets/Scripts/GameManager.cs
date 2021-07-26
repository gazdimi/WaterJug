using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Numerics;


public class GameManager : MonoBehaviour
{
    public static GameManager game;
    public InputField JugA;
    public InputField JugB;
    public InputField Target;

    public GameObject BigBottle;
    public GameObject SmallBottle;

    public GameObject menu;
    public Text error_message;

    //gets called before application starts
    private void Awake()
    {
        if (game == null)
        {
            game = this;
        }
        else if (game != this)
        {
            Destroy(this);
        }
    }

    //OnClick method
    public void BS_Start()
    {
        Debug.Log("Breadth first search started...");
        try
        {
            Int32.TryParse(JugA.text, out int jug_A);
            Int32.TryParse(JugB.text, out int jug_B);
            Int32.TryParse(Target.text, out int target);
            Empty();
            if (jug_A <= 0 || jug_B <= 0 || target <= 0)
            {
                error_message.text = "Please give only numbers, greater than 0...";
            }
            else if (jug_A == jug_B)
            {
                error_message.text = "Water jugs cannot have equal capacity...";
            }
            else if (target > jug_A && target > jug_B)
            {
                error_message.text = "Target number cannot be greater than the jug capacities.";
            }
            else if (target % BigInteger.GreatestCommonDivisor(jug_A, jug_B) != 0)
            {
                error_message.text = "Τhere is no possible solution for the given numbers.";
            }
            else {
                int smaller = Math.Min(jug_A, jug_B);
                int larger = Math.Max(jug_A, jug_B);
                menu.SetActive(false);
                Breadth_First_Search.State solution = Breadth_First_Search.Search(SmallBottle, BigBottle, smaller, larger, target);
                Breadth_First_Search.PrintSolution(solution);
            }
        }
        catch (ArgumentException)
        {
            error_message.text = "Error type of input, only integers are allowed. Please try again!";
        }
    }

    public void Empty() {
        JugA.text = "";
        JugB.text = "";
        Target.text = "";
    }
}
