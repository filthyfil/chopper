using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineFreeLook))]
public class CameraController : MonoBehaviour
{
    private CinemachineFreeLook freeLookCam;

    public float xSensitivity = 2.0f;
    public float ySensitivity = 2.0f;

    public float zoomSpeed = 10f;
    public float minZoom = 2f;
    public float maxZoom = 15f;
    public float smoothTime = 0.3f;

    private float currentZoomLevel;
    private float currentZoomVelocity0;
    private float currentZoomVelocity1;
    private float currentZoomVelocity2;

    private void Start()
    {
        freeLookCam = GetComponent<CinemachineFreeLook>();
        currentZoomLevel = (minZoom + maxZoom) / 2; // Set initial zoom level in the middle of min and max
    }

    private void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (freeLookCam != null)
        {
            currentZoomLevel -= scrollInput * zoomSpeed;
            currentZoomLevel = Mathf.Clamp(currentZoomLevel, minZoom, maxZoom);

            float targetZoom0 = currentZoomLevel;
            float targetZoom1 = currentZoomLevel;
            float targetZoom2 = currentZoomLevel;

            freeLookCam.m_Orbits[0].m_Radius = Mathf.SmoothDamp(freeLookCam.m_Orbits[0].m_Radius, targetZoom0, ref currentZoomVelocity0, smoothTime);
            freeLookCam.m_Orbits[1].m_Radius = Mathf.SmoothDamp(freeLookCam.m_Orbits[1].m_Radius, targetZoom1, ref currentZoomVelocity1, smoothTime);
            freeLookCam.m_Orbits[2].m_Radius = Mathf.SmoothDamp(freeLookCam.m_Orbits[2].m_Radius, targetZoom2, ref currentZoomVelocity2, smoothTime);
        }

        if (Input.GetMouseButton(1)) // Right click
        {
            Cursor.visible = false;
            freeLookCam.m_XAxis.m_InputAxisValue = Input.GetAxis("Mouse X") * xSensitivity;
            freeLookCam.m_YAxis.m_InputAxisValue = Input.GetAxis("Mouse Y") * ySensitivity;
        }
        else
        {
            Cursor.visible = true;
            freeLookCam.m_XAxis.m_InputAxisValue = 0;
            freeLookCam.m_YAxis.m_InputAxisValue = 0;
        }
    }
}
