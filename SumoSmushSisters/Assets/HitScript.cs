using UnityEngine;
using System.Collections;

public class HitScript : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius);

        foreach (Collider col in hitColliders)
        {
            if (col.gameObject.tag.Contains("Player") && col.gameObject != transform.root.gameObject)
            {
                print("Hit Opponent");
                if(GetComponentInParent<PlayerCon>().punching) col.BroadcastMessage("explode", (col.transform.position - transform.position).normalized * 1f);
            }

        }
    }


    private void OnTriggerEnter(Collider otherThing)
    {
    }
}
