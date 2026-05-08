using UnityEditor.SceneManagement;
using UnityEngine;
using MackySoft.SerializeReferenceExtensions;


[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour {

    Rigidbody m_rb;
    [SerializeReference,SubclassSelector] PlayerMovementBase m_playerMovement;
    [SerializeField] float m_forwardThrust;
    private PlayerMovementBase m_mostRecentInitializedMovementStrategy=null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Awake() {
        m_rb = GetComponent<Rigidbody>();
    }

    private void InitializeMovementStrategyIfNeeded() {
        if (m_mostRecentInitializedMovementStrategy == m_playerMovement) {
            return;
        }
        else {
            m_playerMovement?.Initialize(m_rb);
            m_mostRecentInitializedMovementStrategy = m_playerMovement;
        }
    }

    void Update() {
        m_forwardThrust = Input.GetAxis("Vertical");

       
    }

    // Update is called once per frame
    void FixedUpdate() {
        InitializeMovementStrategyIfNeeded();

        m_playerMovement?.ForwardMove(m_forwardThrust);
    }

    public enum MovementStrategy {
        Physics_Explicit_Displacement,
        Physics_Force_Exertion,
        Physics_Explicit_Velocity
    }

    [System.Serializable]
    public abstract class PlayerMovementBase {
        protected Rigidbody m_rb;
        protected MovementStrategy m_movementStrategy;

        public PlayerMovementBase() {
        }

        public virtual void Initialize(Rigidbody mRb) {
            m_rb = mRb;
        }

        protected PlayerMovementBase(Rigidbody mRb,
                                     MovementStrategy mMovementStrategy) {
            m_rb = mRb;
            m_movementStrategy = mMovementStrategy;
        }

        public abstract void ForwardMove(float m_movementTriggerMagnitude);
    }

    [System.Serializable]
    public class PhysicsExplicitMovement : PlayerMovementBase{

        [SerializeField]  private float m_maxForwardStep=1f;

        public PhysicsExplicitMovement() { }
        public PhysicsExplicitMovement(Rigidbody mRb, MovementStrategy mMovementStrategy) : base(mRb, mMovementStrategy) {
        }

        public override void Initialize(Rigidbody mRb) {
            base.Initialize(mRb);
            m_movementStrategy = MovementStrategy.Physics_Explicit_Displacement;
            m_maxForwardStep = 10f;
        }

        public override void ForwardMove(float m_movementTriggerMagnitude) {
            Vector3 targetPosition = m_rb.position +
                                     m_rb.transform.forward *
                                     m_maxForwardStep *
                                     m_movementTriggerMagnitude *
                                     Time.deltaTime;

            m_rb.MovePosition(targetPosition);
        }
    }

    [System.Serializable]
    public class PhysicsForceExertionMovement : PlayerMovementBase {

        [SerializeField] private float m_maxForwardForce=1f;
        [SerializeField] private ForceMode m_forceMode = ForceMode.Force;

        public PhysicsForceExertionMovement() { }

        public PhysicsForceExertionMovement(Rigidbody mRb, MovementStrategy mMovementStrategy) : base(mRb, mMovementStrategy) {
        }

        public override void Initialize(Rigidbody mRb) {
            base.Initialize(mRb);
            m_movementStrategy = MovementStrategy.Physics_Force_Exertion;
            m_maxForwardForce = 10f;
        }

        public override void ForwardMove(float m_movementTriggerMagnitude) {
            Vector3 force = m_rb.transform.forward *
                            m_maxForwardForce *
                            m_movementTriggerMagnitude;

            m_rb.AddForce(force,m_forceMode);
        }
    }

    [System.Serializable]
    public class PhysicsExplicitVelocityMovement : PlayerMovementBase {

        [SerializeField] private float m_maxForwardVelocity=1f;

        public PhysicsExplicitVelocityMovement() { }
        public PhysicsExplicitVelocityMovement(Rigidbody mRb, MovementStrategy mMovementStrategy) : base(mRb, mMovementStrategy) {
        }

        public override void Initialize(Rigidbody mRb) {
            base.Initialize(mRb);
            m_movementStrategy = MovementStrategy.Physics_Explicit_Velocity;
            m_maxForwardVelocity = 10f;
        }

        public override void ForwardMove(float m_movementTriggerMagnitude) {
            Vector3 targetVelocity = m_rb.transform.forward *
                                     m_maxForwardVelocity *
                                     m_movementTriggerMagnitude;
            m_rb.linearVelocity = targetVelocity;
        }
    }
}
