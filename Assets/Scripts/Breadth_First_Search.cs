using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Breadth_First_Search : MonoBehaviour
{
    private static GameObject SmallBottle;
    private static GameObject BigBottle;

    public static State Search(GameObject _smallBottle, GameObject _bigBottle, int capacity_1, int capacity_2, int target)
    {
        SmallBottle = _smallBottle;
        BigBottle = _bigBottle;

        SmallBottle.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = capacity_1.ToString();
        BigBottle.transform.Find("Canvas").Find("TotalCapacity").GetComponent<Text>().text = capacity_2.ToString();

        /*SmallBottle.transform.GetChild(2).GetChild(1).GetComponent<Text>().text = "8"; //testing
        Debug.Log("SMALLER -->" + smaller); //testing
        Debug.Log("LARGER -->" + larger); //testing*/

        State initial_state = new State();
        initial_state.capacity_1 = capacity_1;
        initial_state.current_volume_1 = 0;
        initial_state.capacity_2 = capacity_2;
        initial_state.current_volume_2 = 0;
        initial_state.target = target;

        if (initial_state.ProblemSolved())
        {
            return initial_state;
        }
        Stack<State> search_frontier = new Stack<State>();
        List<State> closed_set = new List<State>();

        search_frontier.Push(initial_state);
        while (search_frontier.Count != 0)
        {
            State current_state = search_frontier.Pop();                                                    //removes & returns the object at the top of the Stack
            if (current_state.ProblemSolved())
            {
                return current_state;
            }
            closed_set.Add(current_state);
            List<State> children = SequentialStates(current_state);
            foreach (State child in children)
            {
                if (!(closed_set.Contains(child)) || !(search_frontier.Contains(child)))
                {
                    search_frontier.Push(child);
                }
            }
        }
        return null;
    }

    public static List<State> SequentialStates(State current_state) {
        List<State> children = new List<State>();
        State new_state;

        if (current_state.current_volume_1 == 0)                                                                    //jug_1 is empty
        {
            current_state.current_volume_1 = current_state.capacity_1;                                              //then fill it
        }
        else if (current_state.current_volume_2 == current_state.capacity_2)
        {
            current_state.current_volume_2 = 0;
        } 
        else if (!(current_state.current_volume_1 == 0)) 
        {
            int target_volume = Mathf.Min(current_state.capacity_2, (current_state.current_volume_2 + current_state.current_volume_1));
            int temp_volume = Mathf.Max(0, current_state.current_volume_2 + current_state.current_volume_1 - current_state.capacity_2);
            current_state.current_volume_1 = temp_volume;
            current_state.current_volume_2 = target_volume;
        }

        new_state = new State();
        new_state.capacity_1 = current_state.capacity_1;
        new_state.current_volume_1 = current_state.current_volume_1;
        new_state.capacity_2 = current_state.capacity_2;
        new_state.current_volume_2 = current_state.current_volume_2;
        new_state.target = current_state.target;

        if (new_state.Accepted()) {
            new_state.parent = current_state;
            children.Add(new_state);
        }
        return children;
    }

    public static void PrintSolution(State solution) {
        List<State> path = new List<State> { solution };
        State parent = solution.parent;
        while (parent != null)
        {
            path.Add(parent);
            parent = parent.parent;
        }
        Debug.Log("Jug 1 current volume of capacity " + solution.parent.capacity_1 + " | Jug 2 current volume of capacity " + solution.parent.capacity_2);
        Debug.Log("__________________________________________________________________________\n");
        for (int i = 0; i < path.Count; i++) {
            State state = path[path.Count - i - 1];
            Debug.Log(string.Concat(System.Linq.Enumerable.Repeat("     ", 4)) + state.current_volume_1 + string.Concat(System.Linq.Enumerable.Repeat("     ", 2)) + "   |   " + string.Concat(System.Linq.Enumerable.Repeat("     ", 3)) + state.current_volume_2);
            Debug.Log("----------------------------------------------------------------------");
        }
    }

    public class State {
        public int capacity_1;
        public int current_volume_1;
        public int capacity_2;
        public int current_volume_2;
        public int target;
        public State parent = null;

        public State() { }

        public bool ProblemSolved()                                                                             //check if solution has been reached
        {
            if (current_volume_1 + current_volume_2 == target)                                                  
            {
                return true;
            }
            return false;
        }

        public bool Accepted()                                                                                  //check if new state is valid
        {
            return (current_volume_1 <= capacity_1 && current_volume_2 <= capacity_2);
        }
    }
}
