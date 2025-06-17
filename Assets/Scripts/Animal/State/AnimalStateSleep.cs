using Scripts.Animal.State;

using UnityEngine;

namespace Scripts.Animal
{
    public class AnimalStateSleep : AnimalState
    {
        private const int STATE_VALUE = (int)AnimalStates.Sleep;

        private int m_aniHashState;
        
        public AnimalStateSleep(AnimalController dogController, Animator dogAnimator) : base(dogController, dogAnimator)
        {
        }

        protected override void Init()
        {
            m_aniHashState = Animator.StringToHash("State");
        }

        public override void Enter()
        {
            OwnerAnimator.SetInteger(m_aniHashState, STATE_VALUE);
            // 잠잘 때, 배고픔 감소율 더 디게
            OwnerController.SetSleepHungerDecreaseRate(true);
        }

        public override void Update()
        {
            OwnerController.StateTransitions();
        }

        public override void Exit()
        {
            // 잠잘 때, 배고픔 감소율 기본으로
            OwnerController.SetSleepHungerDecreaseRate(false);
        }
    }
}
