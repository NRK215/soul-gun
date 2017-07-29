using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EX3.Framework.FSM
{
    /// <summary>
    /// State for a Finite State Machine.
    /// </summary>
    public abstract class FSMState
    {
        #region Properties
        /// <summary>
        /// Finite State Machine owner.
        /// </summary>
        public FSM FSM { get; internal set; }

        /// <summary>
        /// The GameObject that implement the FSM.
        /// </summary>
        public GameObject Owner { get { return this.FSM.Owner; } }

        /// <summary>
        /// The available states to jump from this state.
        /// </summary>
        public List<string> Transitions = new List<string>();
        #endregion

        #region Virtual Methods
        /// <summary>
        /// Use this to initialize the state members and settings.
        /// </summary>
        public virtual void Initialize()
        {

        }

        /// <summary>
        /// Code executed when FSM active this state.
        /// </summary>
        public virtual void OnEnter()
        {

        }

        /// <summary>
        /// Code executed when FSM update this state.
        /// </summary>
        public virtual void Update()
        {

        }

        /// <summary>
        /// Code executed when FSM jump to other state.
        /// </summary>
        public virtual void OnExit()
        {

        }
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Jump to another state.
        /// </summary>
        /// <param name="state">The name of state to jump.</param>
        public void JumpTo(string state)
        {
            this.FSM.JumpTo(state);
        } 
        #endregion
    }
}