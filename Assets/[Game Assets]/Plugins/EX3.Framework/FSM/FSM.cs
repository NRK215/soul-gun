using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections.ObjectModel;

namespace EX3.Framework.FSM
{
    /// <summary>
    /// Finite State Machine
    /// </summary>
    public class FSM
    {
        #region Internal vars
        Dictionary<string, FSMState> _states;
        FSMState _currentState;
        FSMState _initialState;
        #endregion

        #region Properties
        public GameObject Owner { get; private set; }

        /// <summary>
        /// List of the all states in this FSM.
        /// </summary>
        public ReadOnlyCollection<string> States { get; private set; }

        /// <summary>
        /// Current active state name.
        /// </summary>
        public string CurrentState { get; internal set; }

        /// <summary>
        /// The next state to jump.
        /// </summary>
        public string NextState { get; private set; }

        /// <summary>
        /// Show in console the transitions between states.
        /// </summary>
        public bool Debug { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="owner">GameObject that implement the FSM. Uses it to access MonoBehaviour components members.</param>
        public FSM(GameObject owner)
        {
            this.Owner = owner;
            this._states = new Dictionary<string, FSMState>();
            this.Debug = false;
        }
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Add state to FSM.
        /// </summary>
        /// <param name="name">State name</param>
        /// <param name="state">State instance</param>
        /// <param name="initialState">Set this state how initial state.</param>
        public void Add(string name, FSMState state, bool initialState = false)
        {
            if (!this._states.ContainsKey(name))
            {
                state.FSM = this;
                this._states.Add(name, state);
                this.States = this._states.Keys.ToList().AsReadOnly();
                state.Initialize();

                if (initialState)
                {
                    this._initialState = state;
                    this.CurrentState = name;
                }
            }
            else
            {
                throw new System.Exception(string.Format("The FSM already contains a state with the following name: '{0}'", name));
            }
        }

        /// <summary>
        /// Remove state from FSM.
        /// </summary>
        /// <param name="state">State name</param>
        public void Remove(string state)
        {
            if (this._states.ContainsKey(state))
            {
                this._states.Remove(state);
                this.States = this._states.Keys.ToList().AsReadOnly();
            }
            else
            {
                throw new System.Exception(string.Format("The FSM not contains a state with the following name: '{0}'", state));
            }
        }

        /// <summary>
        /// Set the initial state of this FSM.
        /// </summary>
        /// <param name="state">State name.</param>
        public void SetStartState(string state)
        {
            if (this._states.ContainsKey(state))
            {
                this._initialState = this._states[state];
                this.CurrentState = state;
            }
            else
            {
                throw new System.Exception(string.Format("The FSM not contains a state with the following name: '{0}'", state));
            }
        }

        /// <summary>
        /// Update the FSM current state logic.
        /// </summary>
        public void Update()
        {
            if (this._currentState != null)
            {
                this._currentState.Update();
            }
            else
            {
                if (this._initialState != null)
                {                    
                    this._initialState.OnEnter();
                    this._currentState = this._initialState;
                }
            }
        }

        /// <summary>
        /// Jump from current state to other state.
        /// </summary>
        /// <param name="state">Name of state to jump. This state may exists in the transition list in the current state.</param>
        public void JumpTo(string state)
        {
            if (this.Debug)
                UnityEngine.Debug.Log(string.Format("Jump to '{0}' state.", state));

            // Check first if the state exist in the transitions list of current state, and then check if the state exits in the FSM:
            if (this._states.ContainsKey(state))
            {
                if (this._currentState.Transitions.Contains(state))
                {
                    this.NextState = state;
                    this._currentState.OnExit();
                    this._currentState = this._states[state];
                    this._currentState.OnEnter();
                    this.CurrentState = state;
                }
                else
                {
                    throw new System.Exception(string.Format("The '{0}' state not exist in the transition list of this current state '{1}'.", state, this.CurrentState));
                }
            }
            else
            {
                throw new System.Exception(string.Format("The '{0}' state not exist in this FSM.", state));
            }
        }

        /// <summary>
        /// Force to change a selected state.
        /// </summary>
        /// <param name="state">Name of state to jump.</param>
        public void SetState(string state)
        {
            if (this.Debug)
                UnityEngine.Debug.Log(string.Format("Set to '{0}' state.", state));

            // Check first if the state exist in the transitions list of current state, and then check if the state exits in the FSM:
            if (this._states.ContainsKey(state))
            {
                if (this._currentState != null)
                {
                    this._currentState.OnExit();
                }
                this._currentState = this._states[state];
                this._currentState.OnEnter();
                this.CurrentState = state;
            }
            else
            {
                throw new System.Exception(string.Format("The '{0}' state not exist in this FSM.", state));
            }
        }
        #endregion
    }
}
