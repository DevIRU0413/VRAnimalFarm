using Scripts.Animal.State;

using UnityEngine;

namespace Scripts.Animal
{
    public class AnimalStateDead : AnimalState
    {
        private const int STATE_VALUE = (int)AnimalStates.Dead;

        private int m_aniHashState;
        private float m_countTime;

        private bool m_isDead;

        public AnimalStateDead(AnimalController controller, Animator animator) : base(controller, animator) { }

        protected override void Init()
        {
            m_aniHashState = Animator.StringToHash("State");
        }

        public override void Enter()
        {
            m_countTime = OwnerController.DeadDelay;
            m_isDead = false;
        }

        public override void Update()
        {
            if (m_isDead) return;

            m_countTime -= Time.deltaTime;
            if (m_countTime <= 0)
            {
                OwnerAnimator.SetInteger(m_aniHashState, STATE_VALUE);
                m_isDead = true;
            }
        }

        public override void Exit()
        {

        }
    }
}
