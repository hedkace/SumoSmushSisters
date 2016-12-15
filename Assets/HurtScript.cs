using UnityEngine;
using System.Collections;

public class HurtScript : MonoBehaviour {

    CapsuleCollider myBody;

	// Use this for initialization
	void Start () {
        myBody = GetComponent<CapsuleCollider>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter()
    {
        print("Sasquasch");
    }


    void TakeAHit(Vector3 sourceLocation, float strength)
    {
        Vector3 displacement = transform.position - sourceLocation;
        displacement.Normalize();
        displacement *= strength;
        BroadcastMessage("explode", displacement);
    }
}
