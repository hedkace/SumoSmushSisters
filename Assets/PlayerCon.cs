using UnityEngine;
using System.Collections;

public class PlayerCon : MonoBehaviour {

    public float dToGround;
    public float dJump;
    public float dPunch;
    public float wallop;
    public float gValue;
    public float friction;
    public float jump;
    public float speed;
    public float dFix;
    public GameObject opponent;

    private bool bGoingSomewhere;
    private Quaternion direction;
    private Quaternion quat;
    private Rigidbody rb;
    private Vector3 gEffect;
    private float fEffect;
    private bool bDoubleJump;

	// Use this for initialization
	void Start () {
        direction = Quaternion.LookRotation(Vector3.right, Vector3.up);
	    rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {


        //*** Start Spin Action
        transform.rotation = Quaternion.RotateTowards(transform.rotation, direction, 10f);

        if (Input.GetButton("Horizontal")) {
            if (Input.GetAxis("Horizontal") > 0)
            {
                direction = Quaternion.LookRotation(Vector3.right, Vector3.up);
            }
            else
            {
                direction = Quaternion.LookRotation(Vector3.left, Vector3.up);
            }
        }
        //*** End Spin Action


        //*** Start Controls
        rb.velocity *= .99f;
        if (bDoubleJump)
        {
            if (Input.GetButtonDown("Jump"))
            {
                rb.AddForce(Vector3.up * 1200f * jump);
                bDoubleJump = false;
            }
        }
        bGoingSomewhere = Input.GetButton("Horizontal");
        if (bGoingSomewhere)
        {
            rb.velocity += transform.forward * .1f;
        }
        gEffect = Vector3.down / 2f * gValue;
        RaycastHit hit;
        //Debug.Log(Physics.Raycast(new Ray(transform.position, Vector3.down), out hit));
        try
        {
            if (Physics.Raycast(new Ray(transform.position, Vector3.down), out hit))
            {
                //Debug.Log(hit.distance);
                if (hit.distance < dJump)
                {
                    if (hit.collider.tag.Contains("Ground") && rb.velocity.y <= 0)
                    {
                        bDoubleJump = false;
                        rb.velocity = Vector3.ProjectOnPlane(rb.velocity, Vector3.up);
                        gEffect = Vector3.zero;
                        rb.velocity *= .9f;
                        //rb.MovePosition(transform.position - Vector3.up * (dFix - hit.distance)); 
                        //Jumping
                        if (Input.GetButtonDown("Jump"))
                        {
                            rb.AddForce(Vector3.up * 1000f * jump);
                            bDoubleJump = true;
                        }
                        //Movement
                        if (bGoingSomewhere)
                        {
                            rb.velocity += transform.forward * 1.75f * speed;
                        }
                    }
                }
            }
        }catch
        {

        }
        //Debug.Log(gEffect);
        rb.velocity += gEffect;

        //*** End Controls


        //*** Punch

        Debug.DrawRay(this.transform.position, this.transform.forward );
        if (Input.GetKeyDown(KeyCode.Alpha0  ) || Input.GetKeyDown(KeyCode.Keypad0))
        {
            Debug.Log("Input Registered");
            Debug.Log(Physics.Raycast(new Ray(transform.position, transform.forward), out hit));
            try
            {
                Debug.Log("try ran");
                if (Physics.Raycast(new Ray(transform.position, transform.forward), out hit))
                {
                    Debug.Log("Hello");
                    if (hit.distance < dPunch)
                    {
                        Debug.Log("In range");
                        if (hit.collider.gameObject == opponent.gameObject)
                        {
                            Debug.Log("Impulse Applied");
                            opponent.GetComponent<Rigidbody>().AddForce(wallop*transform.forward,ForceMode.Impulse);
                        }
                    }
                }
            }
            catch
            {

            }
        }
        


        //*** Start Boundary Check

        //Check if fall off
        if (transform.position.y < -30f)
        {
            transform.position = Vector3.up * 10f;
            rb.velocity = Vector3.zero;
        }
        //Check if off sides

        //*** End Boundary Check


        //*** Start Region
        //*** End Region


        //*** Start Region
        //*** End Region








    }

}
