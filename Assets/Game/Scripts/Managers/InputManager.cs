using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts.Managers
{
    public class InputManager:MonoBehaviour
    {
        public event Action InitialClick;
        public event Action<Vector3> Clicked;
        public event Action<Vector3> ClickedUp;
        public event Action<Vector3> ClickHold;
        public event Action<Vector3> AutoMoveToTarget;
        
        public Vector3 previousMousePosition;

        private static string buttonName { get; } = "Fire1";

        void Update()
        {
            CheckInput();
        }

        void CheckInput()
        {
            if (Input.GetButtonDown(buttonName))
            {
                OnClicked(Input.mousePosition);
            }
            
            if (Input.GetButtonUp(buttonName))
            {
                OnClickedUp(Input.mousePosition);
            }
            
            if (Input.GetButton(buttonName))
            {
                OnClickHold(Input.mousePosition);
            }
            else
            {
                OnAutoMoveToTarget(Input.mousePosition);
            }
            
            previousMousePosition = Input.mousePosition;
        }
        private void OnClicked(Vector3 mousePosition)
        {
            Clicked?.Invoke(mousePosition);
        }
        private void OnClickedUp(Vector3 mousePosition)
        {
            ClickedUp?.Invoke(mousePosition);
        }
        private void OnClickHold(Vector3 mousePosition)
        {
            ClickHold?.Invoke(mousePosition);
        }
        private void OnAutoMoveToTarget(Vector3 mousePosition)
        {
            AutoMoveToTarget?.Invoke(mousePosition);
        }
        // private void OnInitialClick()
        // {
        //     Clicked -= OnInitialClick;
        //     InitialClick?.Invoke();
        // }
    }
}