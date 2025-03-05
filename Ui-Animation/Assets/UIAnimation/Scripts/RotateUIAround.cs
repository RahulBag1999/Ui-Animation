using UnityEngine;
using System.Collections;

public class RotateUIAround : MonoBehaviour
{
    public RectTransform targetImage; 
    public RectTransform rotatingImage; 
    public float distanceFromCenter = 100f; 
    public float rotationSpeed = 90f; 
    public float targetAngle = 360f; 
    public float angleTolerance = 1f; 
    public float scaleFactor;

    public void SetData(float dist, float angle, float scale)
    {
        distanceFromCenter = dist;
        targetAngle = angle;
        scaleFactor = scale;
    }

    // Coroutine to rotate the image anticlockwise around the target image
    private IEnumerator RotateAroundTargetAnticlockwise()
    {
        float currentAngle = GetCurrentAngle();
       
        if (Mathf.Abs(currentAngle - targetAngle) <= angleTolerance)
        {
            Debug.Log("Already at the target angle.");
            yield break; 
        }

        while (Mathf.Abs(currentAngle - targetAngle) > angleTolerance)
        {
            currentAngle -= rotationSpeed * Time.deltaTime;

            if (currentAngle < 0f)
            {
                currentAngle += 360f; 
            }

            Vector3 direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * currentAngle), Mathf.Sin(Mathf.Deg2Rad * currentAngle), 0);

            rotatingImage.position = targetImage.position + direction * distanceFromCenter;

            yield return null;
        }

        Vector3 finalDirection = new Vector3(Mathf.Cos(Mathf.Deg2Rad * targetAngle), Mathf.Sin(Mathf.Deg2Rad * targetAngle), 0);
        rotatingImage.position = targetImage.position + finalDirection * distanceFromCenter;

        Debug.Log("Rotation completed to target angle: " + targetAngle);
    }

    // Coroutine to rotate the image clockwise around the target image
    private IEnumerator RotateAroundTargetClockwise()
    {
        float currentAngle = GetCurrentAngle();

        if (Mathf.Abs(currentAngle - targetAngle) <= angleTolerance)
        {
            Debug.Log("Already at the target angle.");
            yield break; // Exit the coroutine
        }

        while (Mathf.Abs(currentAngle - targetAngle) > angleTolerance)
        {
            currentAngle += rotationSpeed * Time.deltaTime;

            if (currentAngle >= 360f)
            {
                //currentAngle -= 360f;
            }

            Vector3 direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * currentAngle), Mathf.Sin(Mathf.Deg2Rad * currentAngle), 0);

            rotatingImage.position = targetImage.position + direction * distanceFromCenter;

            yield return null;
        }

        // Ensure the final position is exactly at the target angle (in case of overshooting)
        Vector3 finalDirection = new Vector3(Mathf.Cos(Mathf.Deg2Rad * targetAngle), Mathf.Sin(Mathf.Deg2Rad * targetAngle), 0);
        rotatingImage.position = targetImage.position + finalDirection * distanceFromCenter;

        Debug.Log("Rotation completed to target angle: " + targetAngle);
    }

    // Method to get the current angle of the rotating image
    private float GetCurrentAngle()
    {
        Vector3 direction = rotatingImage.position - targetImage.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (angle < 0)
        {
            angle += 360f;
        }

        return angle;
    }

    // Method to trigger the rotation to a new target angle from another script or UI event
    public void RotateToAngle(bool isZoomIn)
    {
        StopAllCoroutines(); 
        if (isZoomIn )
            StartCoroutine(RotateAroundTargetAnticlockwise());
        else
            StartCoroutine(RotateAroundTargetClockwise());
        StartCoroutine(ZoomObject(GetComponent<RectTransform>(), scaleFactor));
    }

    private IEnumerator ZoomObject(RectTransform rectTransform, float targetScale)
    {
        float startScale = rectTransform.localScale.x;
        float elapsedTime = 0f;

        while (elapsedTime < 0.1f)
        {
            float newScale = Mathf.Lerp(startScale, targetScale, (elapsedTime / 0.1f));
            rectTransform.localScale = new Vector3(newScale, newScale, 1f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        rectTransform.localScale = new Vector3(targetScale, targetScale, 1f);
    }
}
