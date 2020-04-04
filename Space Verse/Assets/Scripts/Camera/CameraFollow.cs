using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target = null;        //  Target to follow
    public float distance = 10;     //  Distance to the Target
    public float cameraHeight = 5.0f;     //  Height of Camera about the target
    public float heightDamping = 2.0f;      //  Smooth Height Damping
    public float rotationDamping = 3.0f;    //  Smooth Rotation damping
    public float rotationX = 30.0f;

    private void LateUpdate()
    {
        if (!target)
            return;

        Vector3 followPosition = new Vector3(0, cameraHeight, -distance);
        Quaternion lookRotation = Quaternion.identity;

        lookRotation.eulerAngles = new Vector3(rotationX, 0, 0);

        Matrix4x4 m1 = Matrix4x4.TRS(target.position, target.rotation, Vector3.one);
        Matrix4x4 m2 = Matrix4x4.TRS(followPosition, lookRotation, Vector3.one);
        Matrix4x4 combinedMatrix = m1 * m2;

        //  Get Position from Matrix4x4
        Vector3 position = combinedMatrix.GetColumn(3);

        //  Get Rotation from Matrix4x4
        Quaternion rotation = Quaternion.LookRotation(combinedMatrix.GetColumn(2), combinedMatrix.GetColumn(1));

        Quaternion wantedRotation = rotation;
        Quaternion currentRotation = transform.rotation;

        Vector3 wantedPosition = position;
        Vector3 currentPosition = transform.position;

        currentRotation = Quaternion.Lerp(currentRotation, wantedRotation, rotationDamping * Time.deltaTime);
        currentPosition = Vector3.Lerp(currentPosition, wantedPosition, heightDamping * Time.deltaTime);

        transform.localPosition = currentPosition;
        transform.localRotation = currentRotation;
    }
}
