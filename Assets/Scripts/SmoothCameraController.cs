using UnityEngine;
using System.Collections;

public class SmoothCameraController : MonoBehaviour
{
    [SerializeField]
    Transform target;

    [SerializeField]
    float dampTime = 0.15f;

    [SerializeField]
    new Camera camera;

    Vector3 velocity = Vector3.zero;

    void Update()
    {
        if (target)
        {
            #region Smooth camera with constant X and Y offsets

            //Vector3 point = camera.WorldToViewportPoint(target.position);
            //Vector3 delta = target.position - camera.ViewportToWorldPoint(new Vector3(xOffSet, yOffSet, point.z));
            //Vector3 destination = transform.position + delta;
            //transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);

            #endregion
            Vector3 position = (target.position + camera.ScreenToWorldPoint(Input.mousePosition)) / 2f;
            Vector3 destination = new Vector3(position.x, position.y, -10);
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }
    }
}