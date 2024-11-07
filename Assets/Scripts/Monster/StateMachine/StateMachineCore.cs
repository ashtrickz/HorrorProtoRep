using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using StateMachine.States;
using UnityEngine;

namespace StateMachine
{
    public abstract class StateMachineCore : MonoBehaviour
    {
        [NonSerialized] public PlayerManager Player;
        [NonSerialized] public EnemyBehaviour Enemy;
        
        [SerializeField] public Animator Animator;

        public StateMachine StateMachine;
        public BaseState CurrentState => StateMachine?.CurrentState;

        public void SetupInstances(BaseState[] statesList)
        {
            StateMachine = new StateMachine();

            foreach (var state in statesList)
                state.SetCore(this);
        }

    }
}
