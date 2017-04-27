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

    [Range (1, 30)]
    public float speed;
    [Range (1,1000)]
    public float jumpForce;
    Vector3 moveDir = Vector3.zero;
    Rigidbody rb;

    public bool isGrounded;
    public Dictionary<int, Vector3> magForces = new Dictionary<int, Vector3>();

    public Polarity polarity;

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
        if(isGrounded)
        {
            moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            transform.Translate(moveDir * speed * Time.deltaTime, Space.Self);
        }
        #endregion
    }

    private void FixedUpdate()
    {
        #region Check if should be pushed off ground
        foreach (Vector3 force in magForces.Values)
        {
            if(force.normalized == transform.up)
            {
                SetIsGrounded(false);
            }
        }
        #endregion

        if (!isGrounded)
        {
            #region Move
            moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            rb.AddRelativeForce(moveDir * speed * Time.deltaTime, ForceMode.Impulse);
            #endregion

            #region Add magnet forces
            foreach (Vector3 force in magForces.Values)
            {
                rb.AddForce(force * (int)polarity, ForceMode.Force);
            }
            #endregion
        }
        else
        {
            #region Jump
            if (Input.GetAxis("Jump") > 0)
            {
                SetIsGrounded(false);

                rb.AddRelativeForce(Vector3.up * jumpForce);
            }
            #endregion
        }

    }

    private void OnTriggerStay(Collider other)
    {
        int id = other.GetInstanceID();

        if (other.tag.Equals("Magnet"))
        {
            if (!magForces.ContainsKey(id))
            {
                magForces.Add(id, other.GetComponent<MagnetConstantForce>().GetForce());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        int id = other.GetInstanceID();

        if (other.tag.Equals("Magnet"))
        {
            if (magForces.ContainsKey(id))
            {
                magForces.Remove(id);
            }
        }
    }

    void SetIsGrounded(bool value)
    {
        rb.isKinematic = value;
        isGrounded = value;
    }
}