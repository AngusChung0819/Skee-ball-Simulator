using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeeBall : MonoBehaviour {

    public GameObject newBallPos;
    public GameObject skeeball;

    public float maxForce = 1550;
    public float minForce = 950;
    public float force = 1000;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyUp("a")) {
            rb.AddForce(0, 0, force);
        }
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.gameObject.tag);
        if(other.gameObject.tag == "Hole") {
            Instantiate(skeeball, newBallPos.transform.position, newBallPos.transform.rotation);
            Destroy(gameObject);
        }
    }

}
