using Scripts.Animal.State;

using UnityEngine;

namespace Scripts.Animal
{
    public class AnimalStateIdle : AnimalState
    {
        private const int STATE_VALUE = (int)AnimalStates.Idle;

        private int m_aniHashState;
        private float m_countTime;

        private bool _isIdle { get => OwnerAnimator.GetInteger(m_aniHashState) == STATE_VALUE; }

        public AnimalStateIdle(AnimalController controller, Animator animator) : base(controller, animator) { }

        protected override void Init()
        {
            m_aniHashState = Animator.StringToHash("State");
        }

        public override void Enter()
        {
            m_countTime = OwnerController.IdleDelay;
        }

        public override void Update()
        {
            OwnerController.StateTransitions(); // 변경을 확인하고
            if (OwnerController.CurrentState != this) return; // 변경 되었다면

            if (_isIdle) return; // 이미 앉아 있다면 리턴해서 위에 변경 확인

            m_countTime -= Time.deltaTime;
            if (m_countTime <= 0)
            {
                OwnerAnimator.SetInteger(m_aniHashState, STATE_VALUE);
            }
        }

        public override void Exit()
        {

        }
    }
}
