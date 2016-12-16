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
    public int direction;
    public float stanceWidth;
    public float stanceHeight;
    public float toeLength;
    public float LandingHeight;
    public float mass;
    public bool punching;
    public bool kicking;

    private bool bGoingSomewhere;
    private Quaternion rotation;
    private Quaternion quat;
    //private Rigidbody rb;
    private Vector3 gEffect;
    private float fEffect;
    private bool bDoubleJump;
    private Vector3 velocity;
    private Vector3 planarRight;
    private bool jumpLock;
    private int jumps;

    // Use this for initialization
    void Start ()
    {
        rotation = Quaternion.LookRotation(Vector3.right, Vector3.up);
        //rb = GetComponent<Rigidbody>();
        velocity = Vector3.zero;
        rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 10f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float num = Input.GetAxis("Horizontal" + GetComponentInParent<PlayerCon>().playerNumber.ToString());
        if (num > .01)
        {
            direction = 1;
        }
        else if (num < -.01)
        {
            direction = -1;
        }

        Debug.DrawRay(transform.position - Vector3.up * (stanceHeight/2f - .1f) - transform.right * (stanceWidth / 2f - .1f), -transform.right * toeLength, Color.yellow);
        Ray toeWhisker = new Ray(transform.position - Vector3.up * (stanceHeight / 2f - .1f) - Vector3.right * (stanceWidth / 2f - .1f), -transform.right * toeLength);
        RaycastHit toeHit;
        bool bStubToe = Physics.Raycast(toeWhisker, out toeHit, toeLength);

        Debug.DrawRay(transform.position - Vector3.up * (stanceHeight / 2f - .1f) + transform.right * (stanceWidth / 2f - .1f), transform.right * toeLength, Color.cyan);
        Ray tailWhisker = new Ray(transform.position - Vector3.up * (stanceHeight / 2f - .1f) + Vector3.right * (stanceWidth / 2f - .1f), transform.right * toeLength);
        RaycastHit tailHit;
        bool bCatchTail = Physics.Raycast(tailWhisker, out tailHit, toeLength);
        Vector3 planarNorm = new Vector3(0f, 1f, 0f);
        planarRight = new Vector3(1f, 0f, 0f);
        
        if (bStubToe && toeHit.collider.tag.Contains("Ground") && (Vector3.Angle(toeHit.normal, Vector3.up) <= (90-30)/180*Mathf.PI))
        {
            planarNorm = toeHit.normal;
            planarRight = new Vector3(toeHit.normal.y, -tailHit.normal.x, 0f);

        }

        else if (bCatchTail && tailHit.collider.tag.Contains("Ground") && (Vector3.Angle(tailHit.normal, Vector3.right) <= (90 - 30) / 180 * Mathf.PI))
        {
            planarNorm = tailHit.normal;
            planarRight = new Vector3(tailHit.normal.y, -tailHit.normal.x, 0f);
        }/*
        else
        {
            castShift = - Vector3.up * .4f;
        }*/




        Ray downWhiskerL = new Ray(transform.position - Vector3.up * (stanceHeight / 2f - .1f) - Vector3.right * (stanceWidth / 2f - .1f), Vector3.down * dJump);
        Debug.DrawRay(transform.position - Vector3.up * (stanceHeight / 2f - .1f) - Vector3.right * (stanceWidth / 2f - .1f), Vector3.down * LandingHeight);
        Ray downWhiskerR = new Ray(transform.position - Vector3.up * (stanceHeight / 2f - .1f) + Vector3.right * (stanceWidth / 2f - .1f), Vector3.down * 5f);
        Debug.DrawRay(transform.position - Vector3.up * (stanceHeight / 2f - .1f) + Vector3.right * (stanceWidth / 2f - .1f), Vector3.down * LandingHeight);
        RaycastHit downHitL;
        RaycastHit downHitR;
        bool bUnderLFoot = Physics.Raycast(downWhiskerL, out downHitL);
        bool bUnderRFoot = Physics.Raycast(downWhiskerR, out downHitR);
        bOnGround = false;
        if (bUnderLFoot && bUnderRFoot)
        {
            if (downHitL.distance <= downHitR.distance)
            {
                if (downHitL.distance <= LandingHeight + dFix)
                {
                    if (downHitL.collider.tag.Contains("Ground") && velocity.y <= 0)
                    {
                        float sign = 0;
                        if (planarNorm.x != 0)
                        {
                            sign = Mathf.Sign(planarNorm.x);
                        }
                        transform.Translate(downHitL.point + Vector3.up * (.1f + dFix) - (transform.position - Vector3.up * (stanceHeight / 2f - .1f) - Vector3.right * (stanceWidth / 2f - .1f)));
                        bOnGround = true;

                    }
                }
            }
            else
            {
                if (downHitR.distance <= LandingHeight + dFix)
                {
                    if (downHitR.collider.tag.Contains("Ground") && velocity.y <= 0)
                    {
                        float sign = 0;
                        if (planarNorm.x != 0)
                        {
                            sign = Mathf.Sign(planarNorm.x);
                        }
                        transform.Translate(downHitR.point + Vector3.up * (.1f + dFix) - (transform.position - Vector3.up * (stanceHeight / 2f - .1f) + Vector3.right * (stanceWidth / 2f - .1f)));
                        bOnGround = true;

                    }
                }
            }
        }
        else if (bUnderRFoot)
        {
            if (downHitR.distance <= LandingHeight + dFix)
            {
                if (downHitR.collider.tag.Contains("Ground") && velocity.y <= 0)
                {
                    float sign = 0;
                    if (planarNorm.x != 0)
                    {
                        sign = Mathf.Sign(planarNorm.x);
                    }
                    transform.Translate(downHitR.point + Vector3.up * (.1f + dFix) - (transform.position - Vector3.up * (stanceHeight / 2f - .1f) + Vector3.right * (stanceWidth / 2f - .1f)));
                    bOnGround = true;

                }
            }
        }
        else if (bUnderLFoot)
        {
            if (downHitL.distance <= LandingHeight + dFix)
            {
                if (downHitL.collider.tag.Contains("Ground") && velocity.y <= 0)
                {
                    float sign = 0;
                    if (planarNorm.x != 0)
                    {
                        sign = Mathf.Sign(planarNorm.x);
                    }
                    transform.Translate(downHitL.point + Vector3.up * (.1f + dFix) - (transform.position - Vector3.up * (stanceHeight / 2f - .1f) - Vector3.right * (stanceWidth / 2f - .1f)));
                    bOnGround = true;

                }
            }
        }
        else
        {

        }

        bool pressingUp = Input.GetAxisRaw("Vertical" + playerNumber.ToString()) >= 0.25f;

        if (bOnGround)
        {
            velocity = Vector3.Normalize(planarRight * Input.GetAxisRaw("Horizontal" + playerNumber.ToString())) * speed * Time.deltaTime;
             
            if (pressingUp)
            {
                jumps = 1;
                velocity.y = jump;
                jumpLock = true;
            }
        }
        else
        {
            if (!pressingUp)
            {
                jumpLock = false;
            }
            else
            {
                if (!jumpLock && jumps > 0)
                {
                    velocity.y = jump;
                    jumps -= 1;
                }
                jumpLock = true;
            }
            //velocity += Vector3.Normalize(Vector3.right * Input.GetAxisRaw("Horizontal" + playerNumber.ToString())) * speed * .03f * Time.deltaTime;
            velocity.y -= gValue * Time.deltaTime;
            velocity.Scale(Vector3.one * (1-drag * Time.deltaTime) );
            velocity.y = Mathf.Clamp(velocity.y, -5, 5);
        }






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
        RaycastHit downHit;
        Debug.DrawRay(this.transform.position, Vector3.right );
        Debug.DrawRay(transform.position, -transform.forward - transform.up * 2f, Color.red);
        kicking = false;
        punching = false;
        if (/*Input.GetKeyDown(KeyCode.Alpha0  ) || Input.GetKeyDown(KeyCode.Keypad0) || */Input.GetButtonDown("Fire1" + playerNumber.ToString()))
        {
            Debug.Log("Input Registered");
            //opponent.BroadcastMessage("explode", (-Vector3.right + Vector3.up * 2f)*5f);

            //kicking = true;
            punching = true;


            /*Debug.Log(Physics.Raycast(new Ray(transform.position, Vector3.right), out downHit));
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
                            opponent.gameObject.BroadcastMessage("explode", transform.forward*10f);
                        }
                    }
                }
            }
            catch
            {

            }*/
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


        //*** Start Wall and Player Check
        /*
        BoxCollider box = new BoxCollider();
        box.size = new Vector3(stanceWidth / 2f, stanceWidth / 2f, stanceWidt / 2f);
        box.transform.position = transform.position + transform.forward*/

        Collider[] hitColliders = Physics.OverlapBox(transform.position + Vector3.right * (stanceWidth/2f+.1f) * direction,  new Vector3(.05f, stanceHeight/4f, .05f));
        Debug.DrawRay(transform.position + Vector3.right * (toeLength + .05f) * direction - stanceHeight/8f*Vector3.up, new Vector3(.1f * direction, stanceHeight / 2f, .1f), Color.green);
        foreach (Collider col in hitColliders)
        {
            if ((col.gameObject.tag.Contains("Player") && col.gameObject != gameObject) || col.gameObject.tag.Contains("Ground"))
            {
                print("moo!");
                velocity = Vector3.ProjectOnPlane(velocity, Vector3.right);
            }

        }



        //*** End Wall and Player Check


        //*** Start Region

        //*** End Region



        //*** Kill Tiny Movements
        if (Mathf.Abs(velocity.x) < .01f) velocity.x = 0;
        if (Mathf.Abs(velocity.y) < .01f) velocity.y = 0;
        if (Mathf.Abs(velocity.z) < .01f) velocity.z = 0;

        //*** Move within the plane (pseudo-3D)
        transform.Translate(velocity);
        transform.position = Vector3.ProjectOnPlane(transform.position, Vector3.forward);


    }

    void explode(Vector3 explosion)
    {
        velocity += explosion / mass;
    }

}
