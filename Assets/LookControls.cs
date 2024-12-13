using UnityEngine;

//KUDOS to Emma Prats: https://youtu.be/rDJOilo4Xrg?list=WL

public class LookControls : MonoBehaviour
{
    #region --- helper ---
    public static class BtnMouse
    {
        public static int primary = 0;
        public static int secondary = 1;
        public static int middle = 2;
    }
    #endregion

    public Camera cam = null;
    public GameObject target = null;
    private Vector3 previousPosition;
    private Vector3 camDistance;

    private void Update()
    {
        //mouse down starts the drag
        if (Input.GetMouseButtonDown(BtnMouse.primary) == true)
        {
            previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
            camDistance = target.transform.position - cam.transform.position;
        }

        //drag mouse to perform rotations of camera around target object
        if (Input.GetMouseButton(BtnMouse.primary) == true)
        {
            Vector3 direction = previousPosition - cam.ScreenToViewportPoint(Input.mousePosition);

            //need to use space.world to keep y axis straight
            cam.transform.position = target.transform.position;
            cam.transform.Rotate(Vector3.right, direction.y * 180f);
            cam.transform.Rotate(Vector3.up, -direction.x * 180f, Space.World);
            cam.transform.Translate(new Vector3(0f, 0f, -camDistance.z));

            previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
        }
    }
}
