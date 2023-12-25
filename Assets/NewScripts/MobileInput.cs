using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

[DefaultExecutionOrder(-1)]
public class MobileInput : MonoBehaviour
{
    public static MobileInput instance;

    public delegate void StartTouchEvent(Vector2 position);
    public event StartTouchEvent OnStartTouch;

    public delegate void EndTouchEvent(Vector2 position);
    public event EndTouchEvent OnEndTouch;

    public delegate void TouchMovedEvent(Vector2 position);
    public event TouchMovedEvent OnTouchMoved;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnEnable()
    {
        TouchSimulation.Enable();
        EnhancedTouchSupport.Enable();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += FingerDown;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += FingerUp;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove += FingerMoved;
    }

    private void OnDisable()
    {
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= FingerDown;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp -= FingerUp;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove -= FingerMoved;
        TouchSimulation.Disable();
        EnhancedTouchSupport.Disable();
    }

    private void FingerDown(Finger finger)
    {
        OnStartTouch?.Invoke(finger.screenPosition);
    }

    private void FingerUp(Finger finger)
    {
        OnEndTouch?.Invoke(finger.screenPosition);
    }

    private void FingerMoved(Finger finger)
    {
        OnTouchMoved?.Invoke(finger.screenPosition);
    }
}