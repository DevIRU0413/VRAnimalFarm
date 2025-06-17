using Scripts.Animal.State;

using UnityEngine;

namespace Scripts.Animal
{
    public class AnimalStateEat : AnimalState
    {
        private const int STATE_VALUE = (int)AnimalStates.Eat;

        private int m_aniHashState;
        private float _countTime;

        public AnimalStateEat(AnimalController controller, Animator animator) : base(controller, animator) { }

        protected override void Init()
        {
            m_aniHashState = Animator.StringToHash("State");
        }

        public override void Enter()
        {
            OwnerAnimator.SetInteger(m_aniHashState, STATE_VALUE);
            _countTime = OwnerController.Food.EatDuration;
        }

        public override void Update()
        {
            _countTime -= Time.deltaTime;
            if (_countTime <= 0)
            {
                OwnerController.ConsumeFood();
                OwnerController.ChangeState(AnimalStates.Idle);
            }
        }

        public override void Exit()
        {

        }
    }
}
