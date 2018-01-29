﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Worker : Entity
{

    // These are all the tasks which a worker can do.
    // Currently Wandering doesnt do anything and just serves as an empty default state.
    public enum State
    {
        Wandering,
        ChoppingTrees,
        Mining,
        Smithing,
        HarvestingGrain,
        Baking
    }
    [Header("Worker State")]
    public State state;

    // This is for the 'loading bar' which appears above the worker when he is doing his job.
    public GameObject workProgressObject;
    public Image workProgressFill;

    // Variable to keep track of the workers current task.
    public Task currentTask;

    // Available actions.
    public Action_GetResources action_GetResources = new Action_GetResources();
    public Action_DepositResources action_DepositResources = new Action_DepositResources();


    public override void Awake()
    {
        base.Awake();
    }

    // Every worker goes to work automatically when starting the game.
    private void Start()
    {
        TaskManager.instance.GetNewTask(this);
    }

    public override void Update()
    {
        base.Update();
    }

    // Sets a certain task depending on the next state.
    // For example:
    // If you call SetTask(State.Mining) it creates a new Task_Mine in currentTask, sets it up and tells the worker do work on that task.
    public void SetTask(State nextState)
    {
        // Sets the next task based on this methods overload
        switch (nextState)
        {
            case State.ChoppingTrees:

                currentTask = new Task_Forest();
                break;
            case State.Mining:

                currentTask = new Task_Mine();
                break;
            case State.Smithing:

                currentTask = new Task_Smith();
                break;
            case State.HarvestingGrain:

                currentTask = new Task_GrainField();
                break;
            case State.Baking:

                currentTask = new Task_Bake();
                break;
        }

        // If the task is unavailable for some reason, abort.
        if (currentTask.state == Task.State.Unavailable)
        {
            Debug.LogError("Task is unavailable");
            return;
        }

        // Sets a state and commands the agent to move to the objectives destination.
        state = nextState;

        // Make the task ready.
        currentTask.Setup();

        // Actually starts the task.
        currentTask.StartTask(this);
    }
}
