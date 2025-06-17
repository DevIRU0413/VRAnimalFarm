using System.Collections.Generic;

using Scripts.Animal.State;
using Scripts.Util;

using UnityEngine;


namespace Scripts.Animal
{
    public class AnimalController : MonoBehaviour
    {
        private const float HEALTH_GOOD_HUNGER_CAT_VALUE = 80.0f;
        private const float HEALTH_NORMAL_HUNGER_CAT_VALUE = 40.0f;

        public enum HealthState
        {
            Good,   // < 75
            Normal, // < 50
            Bad,    // < 25
        }




        // 애니메이터
        [field: SerializeField] public Animator animalAnimator { get; private set; }
        private int m_healthAnimationHash;

        // 상태 관련
        public AnimalState CurrentState { get; private set; } // 현 상태
        private Dictionary<AnimalStates, AnimalState> m_states = new(); // 전환 가능 상태들


        [field: Header("Hunger Config")]
        [field: SerializeField] public float Hunger { get; private set; } = 100f; // 배고픔
        [field: SerializeField] public float BaseHungerDecreaseRate { get; private set; } = 1f; // 배고픔 감소율
        [field: SerializeField] public float SleepHungerDecreaseRate { get; private set; } = 0.5f; // 배고픔 감소율
        private float CurrentHungerDecreaseRate = 1f;

        [field: SerializeField] public HealthState Health { get; private set; } = HealthState.Good; // 건강 상태

        [field: Header("Eatable Config")]
        [field: SerializeField] private GameObjectScanner m_scanner;
        [field: SerializeField] public FoodController Food;
        [field: SerializeField] private float m_eatableRange = 0.4f;

        [field: Header("State Config")]
        [field: SerializeField] public float IdleDelay { get; private set; }
        [field: SerializeField] public float DeadDelay { get; private set; }
        private bool m_isTrans = false;

        public Transform MoveTransform { get; set; }

        public bool IsStateIdle { get => CurrentState == m_states[AnimalStates.Idle]; }

        public bool IsHeadPet { get; set; }
        public bool IsTailPet { get; set; }

        private void Awake() => Init();
        private void Update() => HandleControl();

        private void OnTriggerEnter(Collider other)
        {
            /*if (_foodLayer == (1 << other.gameObject.layer))
            {
                Food = ComponentRegistry
                    .GetAs<FoodController>(other.gameObject);
            }*/
        }

        // 변경할 상태
        public void ChangeState(AnimalStates state)
        {
            CurrentState?.Exit();
            CurrentState = m_states[state];
            Debug.Log($"Trans State[{this.gameObject.name}]: {state.ToString()}");
            CurrentState?.Enter();
        }

        // 음식 섭취
        public void ConsumeFood()
        {
            Hunger += Food.SaveHunger;
            Food.ConsumeFood();
            Food = null;
        }

        // 잠잘 시, 감소됨
        public void SetSleepHungerDecreaseRate(bool isDecreaseRate)
        {
            float decreaseRate = (isDecreaseRate) ? SleepHungerDecreaseRate : 1.0f;

            CurrentHungerDecreaseRate = BaseHungerDecreaseRate * decreaseRate;
        }

        // 상태 전환(외부 전환(Idle, Sleep))
        public void StateTransitions()
        {
            m_isTrans = false;
            switch (Health)
            {
                case HealthState.Good:
                    TransMove();
                    break;
                case HealthState.Normal:
                    TransFood();
                    TransMove();
                    break;
                case HealthState.Bad:
                    TransFood();
                    TransSleep();
                    break;
            }
        }
        // 건강 전환(내부 전환)
        private void HealthTransition()
        {
            Hunger -= Time.deltaTime * CurrentHungerDecreaseRate;

            // 건강 체크
            if (Health == HealthState.Bad && Hunger == 0f)
            {
                ChangeState(AnimalStates.Dead);
                return;
            }
            else
            {
                // 허그 값으로 건강 채크
                if (HEALTH_GOOD_HUNGER_CAT_VALUE < Hunger)
                    Health = HealthState.Good;
                else if (HEALTH_NORMAL_HUNGER_CAT_VALUE < Hunger)
                    Health = HealthState.Normal;
                else
                    Health = HealthState.Bad;

                // 건강 상태가 다를 때 전환됨.
                if (animalAnimator.GetInteger(m_healthAnimationHash) != (int)Health)
                    animalAnimator.SetInteger(m_healthAnimationHash, (int)Health);
            }
        }


        // 상태 전환 조건
        private void TransFood()
        {
            if (m_isTrans) return;

            // 스캐너 안에 내가 찾고자하는 레이어를 가진 오브젝트가 있는가?
            var isFindFood = m_scanner.ScanForLayerObject();
            if (isFindFood)
                Food = m_scanner.ClosestObject.GetComponent<FoodController>();

            if (Food != null)
            {
                float foodDist = Vector3.Distance(transform.position, Food.transform.position);

                if (foodDist > m_eatableRange)
                {
                    MoveTransform = Food.transform;
                    ChangeState(AnimalStates.Move);
                }
                else
                {
                    ChangeState(AnimalStates.Eat);
                }

                m_isTrans = true;
            }
        }
        private void TransMove()
        {
            if (m_isTrans) return;

            // 지정 위치 이동 상태
            if (MoveTransform != null &&
                Vector3.Distance(transform.position, MoveTransform.position) > 1f)
            {
                ChangeState(AnimalStates.Move);
                m_isTrans = true;
            }
        }
        private void TransSleep()
        {
            if (m_isTrans) return;
            if (CurrentState != m_states[AnimalStates.Sleep])
                ChangeState(AnimalStates.Sleep);
            m_isTrans = true;
        }


        // 초기화
        private void Init()
        {
            m_states.Add(AnimalStates.Idle, new AnimalStateIdle(this, animalAnimator));
            m_states.Add(AnimalStates.Move, new AnimalStateMove(this, animalAnimator));
            m_states.Add(AnimalStates.Eat, new AnimalStateEat(this, animalAnimator));
            m_states.Add(AnimalStates.Sleep, new AnimalStateSleep(this, animalAnimator));
            m_states.Add(AnimalStates.Dead, new AnimalStateDead(this, animalAnimator));

            m_healthAnimationHash = Animator.StringToHash("Health");
            CurrentHungerDecreaseRate = BaseHungerDecreaseRate;
            ChangeState(AnimalStates.Idle);
        }

        // Update
        private void HandleControl()
        {
            // 죽었다면 아래 무시
            if (CurrentState == m_states[AnimalStates.Dead]) return;

            HealthTransition();
            CurrentState.Update();
        }

        private void OnDrawGizmos()
        {
            Color color = Color.red;
            color.a = 0.5f;
            Gizmos.color = color;
            transform.DrawGizmoDisk(m_eatableRange);
        }
    }
}
