using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public class TPPCamera : MonoBehaviour
    {
        public Transform FollowTarget;
        public Transform LookAt;
        public float Distance = 10;
        Vector3 CurrentAngle = new Vector3(180, 0);

        public Vector2 MouseSensitivity;
        [SerializeField] Vector2 MouseVelocity = new Vector2(0, 0);
        [SerializeField] Vector2 MaxSpeed = new Vector2(0.1f, 0.1f);
        [SerializeField] float Daming = 10;
        [SerializeField] bool flipX = false;
        [SerializeField] bool flipY = false;
        [SerializeField] float MaxAngle = 10;
        float ScreenShakeTime = 0;
        float MaxScreenShake = 1;
        float ScreenShakeScale = 1;
        float PMouseSensitivity = 1;
        [SerializeField] AnimationCurve ShakeCurve;
        int EnviormentMask;
        bool isPaused = false;
        Vector2 mouseDelta;
        // Start is called before the first frame update
        void Start()
        {
            EnviormentMask = LayerMask.GetMask("Enviorment", "CameraCollider");
            PMouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 0.5f);
        }
        public void TogglePause(bool isPaused)
        {
            this.enabled = !isPaused;
        }
        public void SetMouseDelta(Vector2 delta)
        {
            this.mouseDelta = delta;
        }
        // Update is called once per frame
        void Update()
        {
           
            Vector2 Sensitivity = MouseSensitivity;
            Sensitivity *= PMouseSensitivity * 2;
            mouseDelta *= Sensitivity;
            MouseVelocity += mouseDelta * Time.deltaTime;

            if (flipX) MouseVelocity.x -= 2 * mouseDelta.x;
            if (flipY) MouseVelocity.y -= 2 * mouseDelta.y;

            if (Mathf.Abs(MouseVelocity.x) > MaxSpeed.x)
            {
                MouseVelocity.x = MaxSpeed.x * Mathf.Sign(MouseVelocity.x);
            }
            if (Mathf.Abs(MouseVelocity.y) > MaxSpeed.y)
            {
                MouseVelocity.y = MaxSpeed.y * Mathf.Sign(MouseVelocity.y);
            }

            MouseVelocity = MouseVelocity - (MouseVelocity * Daming * Time.deltaTime);

            //horizontal
            float x, y, z;
            x = Mathf.Cos(CurrentAngle.x) * Distance + FollowTarget.position.x;
            y = FollowTarget.position.y;
            z = Mathf.Sin(CurrentAngle.x) * Distance + FollowTarget.position.z;
            CurrentAngle.x += MouseVelocity.x;

            //vertical

            float TempVertical = CurrentAngle.y + MouseVelocity.y;
            if (TempVertical < MaxAngle && TempVertical > -MaxAngle)
            {
                CurrentAngle.y += MouseVelocity.y;
            }
            else
            {
                MouseVelocity.y = 0;
            }


            this.transform.position = new Vector3(x, y - TempVertical, z);
            // ScreenShake

            if (ScreenShakeTime > 0)
            {
                this.transform.position += Random.insideUnitSphere * ScreenShakeScale;
                ScreenShakeTime -= Time.deltaTime;

            }
            else
            {
                ScreenShakeTime = 0.0f;
            }

            this.transform.LookAt(LookAt);
            CheckCollision();

        }
        void CheckCollision()
        {

            RaycastHit HitInfo;
            bool DidHit = Physics.Linecast(LookAt.position, this.transform.position, out HitInfo, EnviormentMask);
            if (DidHit)
            {
                this.transform.position = HitInfo.point + this.transform.forward;
                this.transform.Translate(new Vector3(0, 0, 0.01f));
            }
        }
        public void SetScreenShake(float time, float Scale)
        {
            MaxScreenShake = time;
            ScreenShakeTime = MaxScreenShake;
            ScreenShakeScale = Scale;
        }
    }
}
