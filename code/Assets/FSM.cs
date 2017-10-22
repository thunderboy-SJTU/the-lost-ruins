using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Transition
{
    NullTransition = 0, // Use this transition to represent a non-existing transition in your system
    SawPlayer,
    //LostPlayer,
	GG,
}


public enum StateID
{
    NullStateID = 0, // Use this ID to represent a non-existing State in your syste 
    ChasingPlayer,
    ROUNDS,
	DIE

}

public abstract class FSMState
{
    protected Dictionary<Transition, StateID> map = new Dictionary<Transition, StateID>();
    protected StateID stateID;
    public StateID ID { get { return stateID; } }

    public void AddTransition(Transition trans, StateID id)
    {
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("FSMState ERROR: NullTransition is not allowed for a real transition");
            return;
        }

        if (id == StateID.NullStateID)
        {
            Debug.LogError("FSMState ERROR: NullStateID is not allowed for a real ID");
            return;
        }
        if (map.ContainsKey(trans))
        {
            Debug.LogError("FSMState ERROR: State " + stateID.ToString() + " already has transition " + trans.ToString() +
                           "Impossible to assign to another state");
            return;
        }

        map.Add(trans, id);
    }

    public void DeleteTransition(Transition trans)
    {
        // Check for NullTransition
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("FSMState ERROR: NullTransition is not allowed");
            return;
        }

        if (map.ContainsKey(trans))
        {
            map.Remove(trans);
            return;
        }
        Debug.LogError("FSMState ERROR: Transition " + trans.ToString() + " passed to " + stateID.ToString() +
                       " was not on the state's transition list");
    }

    public StateID GetOutputState(Transition trans)
    {
        // Check if the map has this transition
        if (map.ContainsKey(trans))
        {
            return map[trans];
        }
        return StateID.NullStateID;
    }


    public virtual void DoBeforeEntering() { }


    public virtual void DoBeforeLeaving() { }


    public abstract void Reason(GameObject player, GameObject npc);

    public abstract void Act(GameObject player, GameObject npc);

} // class FSMState



public class FSMSystem
{
    private List<FSMState> states;


    private StateID currentStateID;
    public StateID CurrentStateID { get { return currentStateID; } }
    private FSMState currentState;
    public FSMState CurrentState { get { return currentState; } }

    public FSMSystem()
    {
        states = new List<FSMState>();
    }

    public void AddState(FSMState s)
    {

        if (s == null)
        {
            Debug.LogError("FSM ERROR: Null reference is not allowed");
        }



        if (states.Count == 0)
        {
            states.Add(s);
            currentState = s;
            currentStateID = s.ID;
            return;
        }

        foreach (FSMState state in states)
        {
            if (state.ID == s.ID)
            {
                Debug.LogError("FSM ERROR: Impossible to add state " + s.ID.ToString() +
                               " because state has already been added");
                return;
            }
        }
        states.Add(s);
    }


    public void DeleteState(StateID id)
    {

        if (id == StateID.NullStateID)
        {
            Debug.LogError("FSM ERROR: NullStateID is not allowed for a real state");
            return;
        }

        foreach (FSMState state in states)
        {
            if (state.ID == id)
            {
                states.Remove(state);
                return;
            }
        }
        Debug.LogError("FSM ERROR: Impossible to delete state " + id.ToString() +
                       ". It was not on the list of states");
    }


    public void PerformTransition(Transition trans)
    {

        if (trans == Transition.NullTransition)
        {
            Debug.LogError("FSM ERROR: NullTransition is not allowed for a real transition");
            return;
        }



        StateID id = currentState.GetOutputState(trans);
        if (id == StateID.NullStateID)
        {
            Debug.LogError("FSM ERROR: State " + currentStateID.ToString() + " does not have a target state " +
                           " for transition " + trans.ToString());
            return;
        }

        currentStateID = id;
        foreach (FSMState state in states)
        {
            if (state.ID == currentStateID)
            {
                currentState.DoBeforeLeaving();

                currentState = state;

                currentState.DoBeforeEntering();
                break;
            }
        }

    } // PerformTransition()

} //class FSMSystem