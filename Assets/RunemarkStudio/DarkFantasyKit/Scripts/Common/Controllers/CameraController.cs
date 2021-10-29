namespace Runemark.Common.Gameplay
{
    using UnityEngine;

    public class CameraController : MonoBehaviour
    {
        public float mainSpeed  = 100;     // regular speed
        public float shiftAdd = 250;       // multiplied by how long shift is held.  Basically running
        public float maxShift = 1000;      // Maximum speed when holdin gshift
        public float camSens = 0.25f;      // How sensitive it with mouse

        Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
        float totalRun = 1;
    
        void Update()
        {
            lastMouse = Input.mousePosition - lastMouse;
            lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0);
            lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0);
            transform.eulerAngles = lastMouse;
            lastMouse = Input.mousePosition;
            //Mouse  camera angle done.  

            //Keyboard commands
            var p = GetBaseInput();
            if (Input.GetKey(KeyCode.LeftShift))
            {
                totalRun += Time.deltaTime;
                p = p * totalRun * shiftAdd;
                p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
                p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
                p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
            }
            else
            {
                totalRun = Mathf.Clamp(totalRun * 0.5f, 1, 1000);
                p = p * mainSpeed;
            }

            p = p * Time.deltaTime;
            if (Input.GetKey(KeyCode.Space))
            { //If player wants to move on X and Z axis only
                p.y = transform.position.y;
                transform.Translate(p);
            }
            else
            {
                transform.Translate(p);
            }

        }

        private Vector3 GetBaseInput()
        {
            Vector3 p_Velocity = Vector3.zero;
            if (Input.GetKey (KeyCode.W))
            {
                p_Velocity += new Vector3(0, 0 , 1);
            }
            if (Input.GetKey (KeyCode.S)){
                p_Velocity += new Vector3(0, 0 , -1);
            }
            if (Input.GetKey (KeyCode.A)){
                p_Velocity += new Vector3(-1, 0 , 0);
            }
            if (Input.GetKey (KeyCode.D)){
                p_Velocity += new Vector3(1, 0 , 0);
            }
            return p_Velocity;
        }
    }
}