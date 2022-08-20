using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plane : MonoBehaviour
{
    private Rigidbody PlaneRigidbody;

    public int i = 0;
    public float power = 0;
    public float x = 0.5f;

    private Vector3 backFrameVec;
    void Start()
    {
        PlaneRigidbody = GetComponent<Rigidbody>();
        backFrameVec = this.transform.position;
    }

    private void FixedUpdate() {
        if(i == 1){
            PlaneRigidbody.constraints = RigidbodyConstraints.None;
            PlaneRigidbody.AddRelativeForce(Vector3.forward * power);
            i = 0;
        }

        Debug.Log(this.transform.localEulerAngles.x);

        Vector3 nowFrameVec = this.transform.position;
        float speed = Mathf.Sqrt(Mathf.Pow(nowFrameVec.x - backFrameVec.x , 2) + Mathf.Pow(nowFrameVec.y - backFrameVec.y , 2) + Mathf.Pow(nowFrameVec.z - backFrameVec.z , 2)) / 0.01f;
        backFrameVec = this.transform.position;

        float liftForce = speed * speed * Mathf.Sin(this.transform.localEulerAngles.x * (Mathf.PI / 180f)) * -1f * x;
        if(liftForce > 0.98f){liftForce = 0.98f;}
        Vector3 addForce = new Vector3(0,liftForce,0);
        PlaneRigidbody.AddRelativeForce(addForce , ForceMode.Force);

        float troque = speed * speed * 0.00002f *Mathf.Cos(this.transform.localEulerAngles.x * (Mathf.PI / 180f));
        if(this.transform.localEulerAngles.x < 270){troque = 0;}
        Vector3 addTroque = new Vector3(troque , 0 ,0);
        PlaneRigidbody.AddRelativeTorque(addTroque , ForceMode.Force);
        Debug.Log("Speed:" + liftForce);
    }
}
