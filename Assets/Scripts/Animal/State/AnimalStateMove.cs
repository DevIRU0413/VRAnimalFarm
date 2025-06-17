using Scripts.Animal.State;

using UnityEngine;

namespace Scripts.Animal
{
    public class AnimalStateMove : AnimalState
    {
        private const int STATE_VALUE = (int)AnimalStates.Move;

        private float m_stopDistance  = 0.5f;
        private float m_minSpeed      = 2f;
        private float m_speedPerMeter = 0.8f;

        private int m_aniHashState; // 상태 관련 애니 해시
        private int m_animHashSpeed; // 이동 값 애니 해시

        public AnimalStateMove(AnimalController dogController, Animator dogAnimator) : base(dogController, dogAnimator) { }

        protected override void Init()
        {
            m_aniHashState = Animator.StringToHash("State");
            m_animHashSpeed = Animator.StringToHash("Speed");
        }

        public override void Enter()
        {
            OwnerAnimator.SetInteger(m_aniHashState, STATE_VALUE);
            OwnerAnimator.SetFloat(m_animHashSpeed, m_minSpeed);
        }

        public override void Update()
        {
            Vector3 targetPos = OwnerController.MoveTransform.position;
            Vector3 curPos    = OwnerController.transform.position;

            Vector3 dirVector   = targetPos - curPos;
            dirVector.y = 0;

            Vector3 dir         = dirVector.normalized;
            float   dist        = dirVector.magnitude;

            if (dist <= m_stopDistance)
            {
                OwnerController.ChangeState(AnimalStates.Idle);
                return;
            }

            var lookRot = Quaternion.LookRotation(dir);
            OwnerController.transform.rotation = lookRot;

            float rawSpeed = dist * m_speedPerMeter;
            float speed    = Mathf.Max(m_minSpeed, rawSpeed);

            /*Vector3 newHeight = OwnerController.transform.position;
            newHeight.y = targetPos.y;
            OwnerController.transform.position = newHeight;*/

            OwnerController.transform.Translate(dir * (speed * Time.deltaTime), Space.World);

            OwnerAnimator.SetFloat(m_animHashSpeed, speed);
        }

        public override void Exit()
        {
            OwnerAnimator.SetFloat(m_animHashSpeed, 0f);
        }
    }
}
