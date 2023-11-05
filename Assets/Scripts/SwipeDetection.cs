using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class SwipeDetection : MonoBehaviour
{
    [SerializeField] private float minDistance = .2f;
    [SerializeField] private float maxTime = 1f;
    [SerializeField, Range(0f, 1f)] private float directionThreshold = .9f;
    [SerializeField] private GameObject trail;
    private Coroutine _coroutine;
    
    
    private Vector2 startPosition;
    private float startTime;
    private Vector2 endPosition;
    private float endTime;
    private void OnEnable()
    {
        InputManager.Instance.OnStartTouch += SwipeStart;
        InputManager.Instance.OnEndTouch += SwipeEnd;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnStartTouch -= SwipeStart;
        InputManager.Instance.OnEndTouch -= SwipeEnd;
    }
    
    private void SwipeStart(Vector2 position, float time)
    {
        startPosition = position;
        startTime = time;
        trail.SetActive(true);
        trail.transform.position = position;
        _coroutine = StartCoroutine(Trail());
    }

    private IEnumerator Trail()
    {
        while(true)
        {
            trail.transform.position = InputManager.Instance.PimaryPosition();
            yield return null;
        }
    }
    
    private void SwipeEnd(Vector2 position, float time)
    {
        StopCoroutine(_coroutine);
        trail.SetActive(false);
        endPosition = position;
        endTime = time;
        DetectSwipe();
    }
    
    private void DetectSwipe()
    {
        if(Vector3.Distance(startPosition, endPosition) >= minDistance && (endTime - startTime) <= maxTime)
        {
            Debug.DrawLine(startPosition, endPosition, Color.red, 5f);
            Debug.Log("Swipe");
            Vector3 direction = endPosition - startPosition;
            Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
            SwipeDirection(direction2D);
        }
    }
// Add references to the movement class
    [FormerlySerializedAs("movement")] [SerializeField] private MovementTouch movementTouch;

    private void SwipeDirection(Vector2 direction)
    {
        if (Vector2.Dot(Vector2.up, direction) > directionThreshold)
        {
            Debug.Log("Swipe Up");
            movementTouch.Jump(); // Assume Jump is modified to be callable without arguments
        }
        else if (Vector2.Dot(Vector2.down, direction) > directionThreshold)
        {
            Debug.Log("Swipe Down");
            movementTouch.Slide(); // Assume Slide is modified to be callable without arguments
        }
        else if (Vector2.Dot(Vector2.right, direction) > directionThreshold)
        {
            Debug.Log("Swipe Right");
            movementTouch.LaneTurn(1); // Pass in direction as argument
        }
        else if (Vector2.Dot(Vector2.left, direction) > directionThreshold)
        {
            Debug.Log("Swipe Left");
            movementTouch.LaneTurn(-1); // Pass in direction as argument
        }
    }

}
