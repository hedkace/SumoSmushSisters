using UnityEngine;
using System.Collections;

public class HitScript : MonoBehaviour {

    public Collider[] hitBoxes;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Collider[] cols = Physics.OverlapBox(hitBoxes[0].bounds.center, hitBoxes[0].bounds.extents, hitBoxes[0].transform.rotation);
        foreach (Collider c in cols)
        {
            print(c.name);
        }
	}


    void OnCollisionEnter(Collision otherThing)
    {
        print("hit");
        /*
        if (otherThing.tag.Contains("Player") && otherThing != null)
        {
            PlayerCon player = otherThing.gameObject.GetComponent<PlayerCon>();
            if (player.playerNumber != 1)
            {
                print(player.playerNumber);
            }
        }*/
    }
}
