using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour {

    Rigidbody m_rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Awake() {
        m_rb = GetComponent<Rigidbody>();
    }


    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
