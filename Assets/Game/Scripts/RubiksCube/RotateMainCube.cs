using System;
using System.Collections.Generic;
using Game.Scripts.Enum;
using Game.Scripts.Managers;
using UnityEngine;
using Zenject;

namespace Game.Scripts.RubiksCube
{
    public class RotateMainCube : MonoBehaviour
    {
        [Inject] private InputManager _inputManager;

        [SerializeField] private GameObject target;
        [SerializeField] private float rotationSpeed = 200f;
        [SerializeField] private float dragSensivity = .1f;

        private Vector2 _firstPressPosition;
        private Vector2 _secondPressPosition;
        private Vector2 _currentSwipe;
        private Vector3 _mouseDelta;

        private readonly Dictionary<SwipeDirection, Vector3> _rotationMappings = new Dictionary<SwipeDirection, Vector3>
        {
            {SwipeDirection.Left, new Vector3(0, 90, 0)},
            {SwipeDirection.Right, new Vector3(0, -90, 0)},
            {SwipeDirection.UpLeft, new Vector3(90, 0, 0)},
            {SwipeDirection.UpRight, new Vector3(0, 0, -90)},
            {SwipeDirection.DownLeft, new Vector3(0, 0, 90)},
            {SwipeDirection.DownRight, new Vector3(-90, 0, 0)}
        };

        private void Start()
        {
            SubscribeToInputEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromInputEvents();
        }

        private void UpdateRotation()
        {
            if (IsRotationComplete()) return;

            RotateTowardsTarget();
        }

        private bool IsRotationComplete()
        {
            return transform.rotation == target.transform.rotation;
        }

        private void RotateTowardsTarget()
        {
            var step = rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target.transform.rotation, step);
        }

        private void RotateTarget(SwipeDirection swipeDirection)
        {
            if (swipeDirection == SwipeDirection.None) return;
            target.transform.Rotate(_rotationMappings[swipeDirection], Space.World);
        }

        private SwipeDirection GetSwipeDirection(Vector2 swipe)
        {
            if (swipe.x < 0 && swipe.y > -0.5f && swipe.y < 0.5f)
                return SwipeDirection.Left;

            if (swipe.x > 0 && swipe.y > -0.5f && swipe.y < 0.5f)
                return SwipeDirection.Right;

            if (swipe.y > 0 && swipe.x < 0)
                return SwipeDirection.UpLeft;

            if (swipe.y > 0 && swipe.x > 0)
                return SwipeDirection.UpRight;

            if (swipe.y < 0 && swipe.x < 0)
                return SwipeDirection.DownLeft;

            if (swipe.y < 0 && swipe.x > 0)
                return SwipeDirection.DownRight;

            return SwipeDirection.None;
        }

        #region Events
        
        private void SubscribeToInputEvents()
        {
            _inputManager.Clicked += OnClick;
            _inputManager.ClickedUp += OnClickUp;
            _inputManager.ClickHold += OnClickHold;
            _inputManager.AutoMoveToTarget += OnAutoMoveToTarget;
        }

        private void UnsubscribeFromInputEvents()
        {
            _inputManager.Clicked -= OnClick;
            _inputManager.ClickedUp -= OnClickUp;
            _inputManager.ClickHold -= OnClickHold;
            _inputManager.AutoMoveToTarget -= OnAutoMoveToTarget;
        }
        
        private void OnClick(Vector3 mousePosition)
        {
            _firstPressPosition = new Vector2(mousePosition.x, mousePosition.y);
        }

        private void OnClickHold(Vector3 mousePosition)
        {
            _mouseDelta = mousePosition - _inputManager.previousMousePosition;
            _mouseDelta *= dragSensivity;
            transform.rotation = Quaternion.Euler(_mouseDelta.y, -_mouseDelta.x, 0) * transform.rotation;
        }
        
        private void OnAutoMoveToTarget(Vector3 obj)
        {
            UpdateRotation();
        }

        private void OnClickUp(Vector3 mousePosition)
        {
            _secondPressPosition = new Vector2(mousePosition.x, mousePosition.y);
            _currentSwipe = new Vector2(_secondPressPosition.x - _firstPressPosition.x,
                _secondPressPosition.y - _firstPressPosition.y);
            _currentSwipe.Normalize();

            SwipeDirection swipeDirection = GetSwipeDirection(_currentSwipe);
            RotateTarget(swipeDirection);
        }
        
        #endregion
    }
}