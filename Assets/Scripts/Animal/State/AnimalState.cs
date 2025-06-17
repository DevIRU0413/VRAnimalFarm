using System;

using UnityEngine;

namespace Scripts.Animal
{
    [Serializable]
    public abstract class AnimalState
    {
        protected AnimalController OwnerController;
        protected Animator OwnerAnimator;

        public AnimalState(AnimalController controller, Animator animator)
        {
            OwnerController = controller;
            OwnerAnimator = animator;
            Init();
        }

        protected abstract void Init();

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }
}
