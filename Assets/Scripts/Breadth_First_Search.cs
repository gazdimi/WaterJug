using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Breadth_First_Search : MonoBehaviour
{
    public GameObject retry_canvas;

    private static GameObject info;
    private static GameObject SmallBottle;
    private static GameObject BigBottle;

    private static List<Tuple<string, string>> movement = new List<Tuple<string, string>>();                                //current_volume_1, current_volume_2
    private static int retry = 0;
    private static int frames = 0;
    private static int state = 1;

    void Update()
    {
        frames++;
        if (frames % 60 == 0)
        {
            if (movement.Count != 0)
            {
                info.transform.GetChild(2).GetComponent<Text>().text = "State: " + state.ToString();

                string current_volume_1 = movement[0].Item1;
                string current_volume_2 = movement[0].Item2;

                SmallBottle.transform.GetChild(2).GetChild(1).GetComponent<Text>().text = current_volume_1;
                BigBottle.transform.GetChild(2).GetChild(1).GetComponent<Text>().text = current_volume_2;
                retry++;
                state++;
                movement.RemoveAt(0);
            }
            else if (movement.Count == 0 && retry != 0)
            { //solution done
                state = 0;
				retry = 0;
                movement.Clear();
				if(state==0 && movement.Count==0 && retry==0){
					retry_canvas.SetActive(true);
				}
            }
            frames = 0;
        }
    }

    public static State Search(GameObject _smallBottle, GameObject _bigBottle, int capacity_1, int capacity_2, int target, GameObject _info) //capacity_1 --> smaller, capacity_2 --> larger
    {
        SmallBottle = _smallBottle;
        BigBottle = _bigBottle;
        info = _info;

        SmallBottle.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = capacity_1.ToString();
        BigBottle.transform.Find("Canvas").Find("TotalCapacity").GetComponent<Text>().text = capacity_2.ToString();
        info.transform.GetChild(1).GetComponent<Text>().text = "Target: " + target.ToString();

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
        for (int i = 0; i < path.Count; i++) {
            State state = path[path.Count - i - 1];
            movement.Add(new Tuple<string, string>(state.current_volume_1.ToString(), state.current_volume_2.ToString()));
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
