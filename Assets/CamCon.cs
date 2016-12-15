using UnityEngine;
using System.Collections;

public class CamCon : MonoBehaviour {

    public GameObject player1;
    public GameObject player2;

    public float smoothness;
    private Vector3 focus;



    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        focus = (player1.transform.position + player2.transform.position) / 2f;
        transform.position = focus + Vector3.back*setD(player1.transform.position,player2.transform.position) + Vector3.up*5f;
        transform.LookAt(focus);

    }



    float setD(Vector3 p1position, Vector3 p2position)
    {
        float distance = (p1position - p2position).magnitude;

        return 15f + 15f / (1 + Mathf.Exp((20f - distance)) * smoothness) + 15f / (1 + Mathf.Exp((60f - distance) * smoothness));
    }
}
