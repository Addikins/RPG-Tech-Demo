using UnityEngine;
using Cinemachine;
using Cinemachine.Utility;
public class SimpleFollowRecenter : MonoBehaviour
{
    public bool recenter;
    public float recenterTime = 0.5f;
    CinemachineFreeLook vcam;
    CinemachineOrbitalTransposer[] orbital = new CinemachineOrbitalTransposer[3];
    CinemachineVirtualCamera[] rigs = new CinemachineVirtualCamera[3];

    void Start()
    {
        vcam = GetComponent<CinemachineFreeLook>();
        for (int i = 0; vcam != null && i < 3; ++i)
        {
            rigs[i] = vcam.GetRig(i);
            orbital[i] = rigs[i].GetCinemachineComponent<CinemachineOrbitalTransposer>();
        }
    }

    void Update()
    {
        Transform target = vcam != null ? vcam.Follow : null;
        if (target == null)
            return;

        // Disable the transposers while recentering
        for (int i = 0; i < 3; ++i)
            orbital[i].enabled = !recenter;

        if (recenter)
        {
            // How far away from centered are we?
            Vector3 up = vcam.State.ReferenceUp;
            Vector3 back = vcam.transform.position - target.position;
            float angle = UnityVectorExtensions.SignedAngle(
                back.ProjectOntoPlane(up), -target.forward.ProjectOntoPlane(up), up);
            if (Mathf.Abs(angle) < UnityVectorExtensions.Epsilon)
                recenter = false; // done!

            // Do the recentering on all 3 rigs
            angle = Damper.Damp(angle, recenterTime, Time.deltaTime);
            for (int i = 0; recenter && i < 3; ++i)
            {
                Vector3 pos = rigs[i].transform.position - target.position;
                pos = Quaternion.AngleAxis(angle, up) * pos;
                rigs[i].transform.position = pos + target.position;
            }
        }
    }
}