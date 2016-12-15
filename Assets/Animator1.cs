using UnityEngine;
using System.Collections;

public class Animator1 : MonoBehaviour {

    private Animator anim;
    public float direction;

    private Quaternion rotation;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        direction = 1;
	}
	
	// Update is called once per frame
	void Update ()
    {
        float move = Mathf.Abs(Input.GetAxis("Horizontal" + GetComponentInParent<PlayerCon>().playerNumber.ToString()));
        anim.SetFloat("Speed", move);
        /*
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
        transform.rotation = Quaternion.Euler(0f, 90f * direction, 0f);*/

        //*** Start Spin Action

        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 20f);
        if (Quaternion.Angle(rotation, transform.rotation) < .02) transform.rotation = rotation;


        if (Input.GetButton("Horizontal" + GetComponentInParent<PlayerCon>().playerNumber.ToString()))
        {
            if (Input.GetAxis("Horizontal" + GetComponentInParent<PlayerCon>().playerNumber.ToString()) > 0)
            {
                rotation = Quaternion.LookRotation(Vector3.right, Vector3.up);
            }
            else
            {
                rotation = Quaternion.LookRotation(Vector3.left, Vector3.up);
            }
        }

        if (GetComponentInParent<PlayerCon>().punching)
        {
            anim.SetBool("Punching", true);
            StartCoroutine("stopPunching");

        }
        

        //*** End Spin Action

    }

    IEnumerator stopPunching()
    {
        yield return new WaitForSeconds(.4f);
        anim.SetBool("Punching", false);
    }
}
