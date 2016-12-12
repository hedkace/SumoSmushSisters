using UnityEngine;
using System.Collections;

public class PlayerCon : MonoBehaviour {

    public float playerNumber;
    public float dToGround;
    public float dJump;
    public float dPunch;
    public float wallop;
    public float gValue;
    public float friction;
    public float drag;
    public float jump;
    public float speed;
    public float dFix;
    public GameObject opponent;
    public bool bOnGround;
    public float slopeAngle;

    private bool bGoingSomewhere;
    private int direction;
    private Quaternion rotation;
    private Quaternion quat;
    //private Rigidbody rb;
    private Vector3 gEffect;
    private float fEffect;
    private bool bDoubleJump;
    private Vector3 velocity;
    private Vector3 planarRight;
    private Vector3 castShift;

	// Use this for initialization
	void Start ()
    {
        castShift = - Vector3.up * .4f;
        rotation = Quaternion.LookRotation(Vector3.right, Vector3.up);
        //rb = GetComponent<Rigidbody>();
        velocity = Vector3.zero;
        rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 10f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetAxis("Horizontal" + playerNumber.ToString()) > 0) direction = 1; else direction = -1;


        Debug.DrawRay(transform.position - Vector3.up * .4f - transform.right * .4f, -transform.right, Color.yellow);
        Ray toeWhisker = new Ray(transform.position - Vector3.up * .4f - Vector3.right * .4f, -transform.right);
        RaycastHit toeHit;
        bool bStubToe = Physics.Raycast(toeWhisker, out toeHit, 1f);

        Debug.DrawRay(transform.position - Vector3.up * .4f + transform.right * .4f, transform.right, Color.cyan);
        Ray tailWhisker = new Ray(transform.position - Vector3.up * .4f + Vector3.right * .4f, transform.right);
        RaycastHit tailHit;
        bool bCatchTail = Physics.Raycast(tailWhisker, out tailHit, 1f);
        Vector3 planarNorm = new Vector3(0f, 1f, 0f);
        planarRight = new Vector3(1f, 0f, 0f);

        if (bStubToe && toeHit.collider.tag.Contains("Ground"))
        {
            planarNorm = toeHit.normal;
            planarRight = new Vector3(toeHit.normal.y, -tailHit.normal.x, 0f);
            castShift = - Vector3.up * .4f - Vector3.right;

        }

        else if (bCatchTail && tailHit.collider.tag.Contains("Ground"))
        {
            planarNorm = tailHit.normal;
            planarRight = new Vector3(tailHit.normal.y, -tailHit.normal.x, 0f);
            castShift = - Vector3.up * .4f + Vector3.right;
        }/*
        else
        {
            castShift = - Vector3.up * .4f;
        }*/
        Ray downWhisker = new Ray(transform.position + castShift, Vector3.down * 5f);
        Debug.DrawRay(transform.position + castShift, Vector3.down * 5f);
        RaycastHit downHit;
        bool bUnderFoot = Physics.Raycast(downWhisker, out downHit);
        /*delY = .5f * Mathf.Sin(Vector3.Angle(new Vector3(1f, 0f, 0f), planarRight));*/
        bOnGround = false;
        if (bUnderFoot)
        {
            if (downHit.distance <= 1f + dFix)
            {
                if (downHit.collider.tag.Contains("Ground") && velocity.y <= 0)
                {
                    float sign = 0;
                    if ( planarNorm.x != 0)
                    {
                        sign = Mathf.Sign(planarNorm.x);
                    }
                    transform.Translate(downHit.point + Vector3.up * (.1f + dFix)  - (transform.position + castShift));
                    bOnGround = true;

                }
            }
        }

        if(bOnGround)
        {
            velocity = Vector3.Normalize(planarRight * Input.GetAxisRaw("Horizontal" + playerNumber.ToString())) * speed * Time.deltaTime;

            if (Input.GetButtonDown("Jump" + playerNumber.ToString()))
            {
                velocity.y = jump;
            }
        }
        else
        {
            velocity.y -= gValue * Time.deltaTime;
            velocity.Scale(Vector3.one * (1-drag * Time.deltaTime) );
            velocity.y = Mathf.Clamp(velocity.y, -5, 5);
        }

        //*** Start Spin Action

        /*
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 10f);
        if (Quaternion.Angle(rotation, transform.rotation) < .02) transform.rotation = rotation;


        if (Input.GetButton("Horizontal" + playerNumber.ToString()))
        {
            if (Input.GetAxis("Horizontal" + playerNumber.ToString()) > 0)
            {
                rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
            }
            else
            {
                rotation = Quaternion.LookRotation(Vector3.back, Vector3.up);
            }
        }

        */

        //*** End Spin Action



        if (Mathf.Abs(velocity.x) < .01f) velocity.x = 0;
        if (Mathf.Abs(velocity.y) < .01f) velocity.y = 0;
        if (Mathf.Abs(velocity.z) < .01f) velocity.z = 0;
        
        transform.Translate(velocity);
        transform.position  = Vector3.ProjectOnPlane(transform.position, Vector3.forward);

        /*   Old Controls


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
        bOnGround = false;
        //velocity = .99f * velocity;
        if (bDoubleJump)
        {
            if (Input.GetButtonDown("Jump"))
            {
                velocity += Vector3.up * jump;
                bDoubleJump = false;
            }
        }
        bGoingSomewhere = Input.GetButton("Horizontal");
        if (bGoingSomewhere)
        {
            velocity = Vector3.ProjectOnPlane(transform.forward, Vector3.right) * Time.deltaTime;
        }
        gEffect = Vector3.down / 2f * gValue;
        RaycastHit hit;
        try
        {
            if (Physics.Raycast(new Ray(transform.position - Mathf.Sign(slopeAngle)* transform.forward * .5f - Vector3.up*.5f, Vector3.down), out hit))
            {
                Debug.DrawRay(transform.position - Mathf.Sign(slopeAngle) * transform.forward * .5f - Vector3.up * .5f, Vector3.down * 10f, Color.yellow);
                if (hit.distance < dJump)
                {
                    if (hit.collider.tag.Contains("Ground") && velocity.y <= 0)
                    {
                        dToGround = hit.distance;
                        bOnGround = true;
                        slopeAngle = Vector3.Dot(Vector3.right, hit.normal);
                        //transform.rotation.eulerAngles.Set(slopeAngle, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                        bDoubleJump = false;
                        //rb.velocity = Vector3.ProjectOnPlane(rb.velocity, Vector3.right);
                        gEffect = Vector3.zero;
                        //velocity = velocity * .995f;
                        transform.Translate(Vector3.up * (dFix - dToGround)); 
                        //Jumping
                        if (Input.GetButtonDown("Jump"))
                        {
                            velocity += Vector3.up * jump;
                            bDoubleJump = true;
                        }
                        //Movement
                        if (bGoingSomewhere)
                        {
                            Vector3 temp = Vector3.Cross(hit.normal, Vector3.forward);
                            velocity = Vector3.Project(transform.forward, temp) * 1000f* speed * Time.deltaTime;
                        }
                    }
                }
            }
        }catch
        {

        }
        //Debug.Log(gEffect);
        velocity += gEffect;


        
        transform.Translate(Vector3.ProjectOnPlane(velocity, Vector3.right));
        transform.position = Vector3.ProjectOnPlane(transform.position, Vector3.forward);


        */
        //*** End Controls


        //*** Punch

        Debug.DrawRay(this.transform.position, Vector3.right );
        if (Input.GetKeyDown(KeyCode.Alpha0  ) || Input.GetKeyDown(KeyCode.Keypad0))
        {
            Debug.Log("Input Registered");
            Debug.Log(Physics.Raycast(new Ray(transform.position, Vector3.right), out downHit));
            try
            {
                Debug.Log("try ran");
                if (Physics.Raycast(new Ray(transform.position, Vector3.right), out downHit))
                {
                    Debug.Log("Hello");
                    if (downHit.distance < dPunch)
                    {
                        Debug.Log("In range");
                        if (downHit.collider.gameObject == opponent.gameObject)
                        {
                            Debug.Log("Impulse Applied");
                            //opponent.GetComponent<Rigidbody>().AddForce(wallop* Vector3.right, ForceMode.Impulse);
                            print("moo!");
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
            velocity = Vector3.zero;
        }
        //Check if off sides

        //*** End Boundary Check


        //*** Start Region
        //*** End Region


        //*** Start Region

        //*** End Region
        



    
    }

}
