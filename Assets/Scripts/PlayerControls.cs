using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class PlayerControls : MonoBehaviour
{
    [Range (1, 10)]
    public float lookXSensitivity, lookYSensitivity;
    public bool invertedLookY;
    [Range (0,50)]
    public float lookYMax;
    public Camera playerCam;

    [Range (1, 100)]
    public float speed;
    Vector3 moveDir = Vector3.zero;
    Rigidbody rb;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        #region Look
        //  Look X
        transform.Rotate(transform.up, Input.GetAxis("Mouse X") * lookXSensitivity);

        #region Look Y
        playerCam.transform.Rotate(Vector3.right,
            Input.GetAxis("Mouse Y") * (invertedLookY ? 1 : -1) * lookYSensitivity, Space.Self);

        //  Check if hit y look max
        float angle;
        Vector3 axis;
        playerCam.transform.localRotation.ToAngleAxis(out angle, out axis);
        if(angle > 90f - lookYMax)
        {
            if(playerCam.transform.localRotation.x > 0)
                playerCam.transform.localEulerAngles = new Vector3(90f - lookYMax, 0, 0);
            else
                playerCam.transform.localEulerAngles = new Vector3(-90f + lookYMax, 0, 0);
        }
        #endregion
        #endregion

        #region Move
        moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Debug.Log(moveDir);
        rb.AddRelativeForce(moveDir * speed * Time.deltaTime, ForceMode.Impulse);
        #endregion
    }
}