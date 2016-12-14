using UnityEngine;
using System.Collections;

public class Animator1 : MonoBehaviour {

    private Animator anim;
    public float direction;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        direction = 1;
	}
	
	// Update is called once per frame
	void Update () {
        float move = Mathf.Abs(Input.GetAxis("Horizontal"));
        anim.SetFloat("Speed", move);
        float num = Input.GetAxis("Horizontal" + GetComponentInParent<PlayerCon>().playerNumber.ToString());
        if (num > .01)
        {
            direction = 1;
        }
        else if (num < -.01)
        {
            direction = -1;
        }
        print(Vector3.right * direction);
        transform.rotation = Quaternion.Euler(0f, 90f * direction, 0f);
	}
}
