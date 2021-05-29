using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace LH.GOAP {
    public class Goal {
        // Dictionary to store our goals
        public Dictionary<string, int> goalsDict;
        // Bool to store if goal should be removed after it has been achieved
        public bool remove;

        // Constructor
        public Goal(string state, int value, bool remove) {
            goalsDict = new Dictionary<string, int>();
            goalsDict.Add(state, value);
            this.remove = remove;
        }
    }

    public abstract class GAgent : MonoBehaviour {
        public const float TARGET_DISTANCE = 2f;

        // Store our list of actions
        public List<GAction> Actions { get; set; } = new List<GAction>();

        // Dictionary of subgoals
        public Dictionary<Goal, int> GoalsDict { get; set; } = new Dictionary<Goal, int>();

        // Our inventory
        public GInventory Inventory { get; set; } = new GInventory();

        // Our beliefs
        public GStates Beliefs { get; set; } = new GStates();

        // Our goal
        public Goal CurrentGoal { get; set; }

        // Our current action
        public GAction CurrentAction { get; set; }

        // Access the planner
        protected GPlanner _planner;
        // Action Queue
        protected Queue<GAction> _actionQueue;

        // Out target destination for resources
        private Vector3 _destination = Vector3.zero;
        [SerializeField]
        private GameObject _destTarget;

        [SerializeField] private Canvas _canvas;

        private Camera _camera;

        public event Action actionChangeEvent;

        // Start is called before the first frame update
        protected virtual void Start() {
            _camera = Camera.main;

            GAction[] acts = this.GetComponents<GAction>();
            foreach (GAction a in acts)
                Actions.Add(a);
        }

        public void StopCurrentAction() {
            if (CurrentAction != null && CurrentAction.IsRunning) {
                // Debug.Log("Stopping current action");
                CurrentAction.IsRunning = false;
            }
        }

        public void StopMoving() {
            CurrentAction.navMeshAgent.isStopped = true;
        }

        bool invoked = false;
        //an invoked method to allow an agent to be performing a task
        //for a set location
        void CompleteAction() {
            // Debug.Log("COMPLETING action: " + CurrentAction.actionName);

            CurrentAction.IsRunning = false;
            CurrentAction.PostPerform();
            invoked = false;
            started = false;

            // Debug.Log("Action completed: " + CurrentAction.actionName);
        }

        bool started = false;
        void LateUpdate() {
            //  in-world canvas text should face camera
            if (_canvas) {
                Quaternion rotation = _camera.transform.rotation;
                _canvas.transform.LookAt(_canvas.transform.position + rotation * Vector3.forward,
                    rotation * Vector3.up);
            }

            //if there's a current action and it is still running
            if (CurrentAction != null && CurrentAction.IsRunning) {
                // Find the distance to the target
                float distanceToTarget = Vector3.Distance(_destination, this.transform.position);

                string go = CurrentAction.destinationGO != null ? CurrentAction.destinationGO.name : "???";

                // Check the agent has a goal and has reached that goal
                if (distanceToTarget < TARGET_DISTANCE) {
                    CurrentAction.navMeshAgent.isStopped = true;

                    started = false;

                    CurrentAction.Perform();

                    // Debug.Log("Distance to Goal: " + currentAction.agent.remainingDistance);
                    if (!invoked) {
                        //if the action movement is complete wait
                        //a certain duration for it to be completed
                        Invoke(nameof(CompleteAction), CurrentAction.duration);
                        invoked = true;
                    }
                }
                else {
                    //  update our destination based on our target's current position
                    GameObject tar = CurrentAction.destinationGO;
                    if (tar != null && Vector3.Distance(_destination, tar.transform.position) > 1.0f) {
                        _destination = tar.transform.position;
                        CurrentAction.navMeshAgent.SetDestination(_destination);

                        _destTarget = tar;
                    }

                    CurrentAction.navMeshAgent.isStopped = false;
                }

                return;
            }

            if (CurrentAction != null && !CurrentAction.IsRunning) {
                _planner = null;
            }

            // Check we have a planner and an actionQueue
            if (_planner == null || _actionQueue == null) {
                _planner = new GPlanner();

                // Sort the goals in descending order and store them in sortedGoals
                var sortedGoals = from entry in GoalsDict orderby entry.Value descending select entry;

                //look through each goal to find one that has an achievable plan
                foreach (KeyValuePair<Goal, int> sg in sortedGoals) {
                    _actionQueue = _planner.Plan(Actions, sg.Key.goalsDict, Beliefs);
                    // If actionQueue is not = null then we must have a plan
                    if (_actionQueue != null) {
                        // Set the current goal
                        CurrentGoal = sg.Key;
                        break;
                    }
                }
            }

            // Have we an actionQueue but no queued actions
            if (_actionQueue != null && _actionQueue.Count == 0) {
                // Check if currentGoal is removable
                if (CurrentGoal.remove) {
                    // Remove it
                    GoalsDict.Remove(CurrentGoal);
                }

                // Set planner = null so it will trigger a new one
                _planner = null;
            }

            // Do we still have actions
            if (_actionQueue != null && _actionQueue.Count > 0) {
                // Remove the top action of the queue and put it in currentAction
                CurrentAction = _actionQueue.Dequeue();

                if (started) {
                    return;
                }

                if (CurrentAction.PrePerform()) {
                    // Debug.Log("STARTING action: " + CurrentAction.actionName);

                    // Get our current object
                    if (CurrentAction.destinationGO == null && CurrentAction.destinationTag != "") {
                        CurrentAction.destinationGO = GameObject.FindWithTag(CurrentAction.destinationTag);
                    }

                    if (CurrentAction.destinationGO != null) {
                        // Activate the current action
                        CurrentAction.IsRunning = true;
                        started = true;

                        //  Get the target's transform for the NavMeshAgent destination
                        _destination = CurrentAction.destinationGO.transform.position;

                        // Check if our target has a "Destination" assigned to it
                        Transform dest = CurrentAction.destinationGO.transform.Find("Destination");
                        // Check we got it
                        if (dest != null) {
                            _destination = dest.position;
                        }

                        _destTarget = CurrentAction.destinationGO;

                        float distanceToTarget = Vector3.Distance(_destination, this.transform.position);
                        if (distanceToTarget >= TARGET_DISTANCE) {
                            // Pass Unities AI the destination for the agent
                            CurrentAction.navMeshAgent.SetDestination(_destination);
                        }
                    }
                    
                    actionChangeEvent?.Invoke();
                }
                else {
                    // Force a new plan
                    _actionQueue = null;
                    started = false;
                }
            }
        }
    }
}
