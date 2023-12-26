using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using DG.Tweening;
using MyBox;

public class Joystick : MonoBehaviour
{
    [SerializeField] Image joystick;
    [SerializeField] private Canvas UICanvas;
    [SerializeField] private RectTransform NavRoot;

    private Finger fingerID = null;
    private bool isDragging = false;
    private float travelDist; // Travel dist is how far our joystick can move from its center
    private Vector2 centerPos;

    [ReadOnly] public float angle;

    // Start is called before the first frame update
    void Awake()
    {
        travelDist = joystick.rectTransform.sizeDelta.x / 2;
        centerPos = joystick.transform.localPosition;
        joystick.transform.DOScale(Vector3.one * 0.8f, 0.2f);
    }

    private void OnEnable()
    {
        // Subscribe to all relevant Input Manager events
        MobileInput.instance.OnStartTouch += CheckForJoystickTouch;
        MobileInput.instance.OnTouchMoved += MoveJoystick;
        MobileInput.instance.OnEndTouch += CleanUpFromInteraction;
    }

    private void OnDisable()
    {
        // Unsubscribe from all relevant Input Manager events
        MobileInput.instance.OnStartTouch -= CheckForJoystickTouch;
        MobileInput.instance.OnTouchMoved += MoveJoystick;
        MobileInput.instance.OnEndTouch -= CleanUpFromInteraction;
    }

    private void CheckForJoystickTouch(Finger finger)
    {
        if (JoystickContainsPoint(finger.screenPosition) && fingerID == null)
        {
            fingerID = finger;
            isDragging = true;
            joystick.transform.DOScale(Vector3.one, 0.2f);
        }
    }

    private void MoveJoystick(Finger finger)
    {
        if (isDragging && fingerID == finger)
        {
            // Translate screen interaction into a position on the UI Canvas
            // regardless of rendermode.
            Vector2 anchoredPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(NavRoot,
                                                                        finger.screenPosition,
                                                                        UICanvas.renderMode == RenderMode.ScreenSpaceOverlay ?
                                                                        null : PlayerNEW.instance.mainCam,
                                                                        out anchoredPos);

            angle = Mathf.Atan2(anchoredPos.y, anchoredPos.x) * Mathf.Rad2Deg;
            if (Vector2.Distance(anchoredPos, centerPos) > travelDist)
                joystick.rectTransform.anchoredPosition = anchoredPos.normalized * travelDist;
            else
                joystick.rectTransform.anchoredPosition = anchoredPos;
        }
    }

    private void CleanUpFromInteraction(Finger finger)
    {
        if (fingerID == finger)
        {
            fingerID = null;
            isDragging = false;

            angle = float.NaN;
            joystick.transform.DOScale(Vector3.one * 0.8f, 0.2f);
            joystick.rectTransform.DOLocalMove(centerPos, 0.2f, false).SetEase(Ease.InOutSine);
        }
    }

    bool JoystickContainsPoint(Vector2 point)
    {
        float width = joystick.rectTransform.sizeDelta.x;
        float height = joystick.rectTransform.sizeDelta.y;

        Vector2 pos = joystick.transform.position;

        return pos.x - (width / 2f) < point.x &&
                point.x < pos.x + (width / 2f) &&
                pos.y - (height / 2f) < point.y &&
                point.y < pos.y + (height / 2f);
    }
}
