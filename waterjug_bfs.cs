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
	
	State initial_state = new State();
	initial_state = State(smaller,0,larger,0);
	if (initial_state.problem_solved()){
		return initial_state;
	}
	Stack<State> search_frontier = new Stack<State>();
	List<State> closed_set = new List<State>();
	search_frontier.Push(initial_state);
	while(search_frontier.Count != 0){
		 State current_state = search_frontier.Pop();
		 if (current_state.ProblemSolved()) {
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

	public class State{
		public State parent;
		
		public State() {}
		
		public bool problem_solved(){
			if (_smallBottle + _bigBottle ==target{
				return true;
				) else{
					return false;
				}
			}
		}
	
		public accepted(){
			return current_volume_1 <= smaller && current_volume_2 <= larger;
		}	
	}
	
	public static List<State> SequentialStates(State current_state) {
		List<State> children = new List<State>();
        State new_state;
		if(current_state.current_volume_1 ==0){
			current_state.current_volume_1 = current_state.smaller;
		} else if (current_state.current_volume_2 == current_state.larger){
			current_state.current_volume_2=0;
		} else if(!(current_state.current_volume_1==0)){
			int target_volume = min(current_state.larger,current_state.current_volume_2 + current_state.current_volume_1);
			int temp_volume= max(0,current_state.current_volume_2+current_state.current_volume_1-current_state.larger);
			current_state.current_volume_1 = temp_volume;
			current_state.current_volume_2 = target_volume;
		}
		new_state = State(current_state.smaller, current_state.current_volume_1,current_state.larger,current_state.current_volume_2,current_state.target);
		if(new_state.accepted()){
			new_state.parent = current_state;
			children.append(new_state);
		}
		return children;
				
	}

