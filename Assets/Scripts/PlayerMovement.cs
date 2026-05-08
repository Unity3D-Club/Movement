using UnityEditor.SceneManagement;
using UnityEngine;
using MackySoft.SerializeReferenceExtensions;


[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour {

    Rigidbody m_rb;
    [SerializeReference,SubclassSelector] PlayerMovementBase m_playerMovement;
    [SerializeField] float m_forwardThrust;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Awake() {
        m_rb = GetComponent<Rigidbody>();
        
    }


    void Update() {
        m_forwardThrust = Input.GetAxis("Vertical");
    }

    // Update is called once per frame
    void FixedUpdate() {

        m_playerMovement?.ForwardMove(m_forwardThrust);
    }

    public enum MovementStrategy {
        Physics_Explicit_Displacement,
        Physics_Force_Exertion,
        Physics_Explicit_Velocity
    }

    [System.Serializable]
    public abstract class PlayerMovementBase {
        private Rigidbody m_rb;
        [SerializeReference] private MovementStrategy m_movementStrategy;

        public PlayerMovementBase() {
        }

        public void Initialize(Rigidbody mRb, MovementStrategy mMovementStrategy) {
            m_rb = mRb;
            m_movementStrategy = mMovementStrategy;
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

        [SerializeField]  private float m_maxForwardStep;

        public PhysicsExplicitMovement() { }
        public PhysicsExplicitMovement(Rigidbody mRb, MovementStrategy mMovementStrategy) : base(mRb, mMovementStrategy) {
        }
        public override void ForwardMove(float m_movementTriggerMagnitude) {
            // Implement explicit movement logic here
        }
    }

    [System.Serializable]
    public class PhysicsForceExertionMovement : PlayerMovementBase {

        public PhysicsForceExertionMovement() { }

        public PhysicsForceExertionMovement(Rigidbody mRb, MovementStrategy mMovementStrategy) : base(mRb, mMovementStrategy) {
        }
        public override void ForwardMove(float m_movementTriggerMagnitude) {
            // Implement force exertion movement logic here
        }
    }

    [System.Serializable]
    public class PhysicsExplicitVelocityMovement : PlayerMovementBase {
        public PhysicsExplicitVelocityMovement() { }
        public PhysicsExplicitVelocityMovement(Rigidbody mRb, MovementStrategy mMovementStrategy) : base(mRb, mMovementStrategy) {
        }
        public override void ForwardMove(float m_movementTriggerMagnitude) {
            // Implement explicit velocity movement logic here
        }
    }
}
