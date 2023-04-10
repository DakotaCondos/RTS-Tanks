using UnityEngine;

public static class ShakeObject
{
    public static void Shake(GameObject objectToShake, float shakeDuration, float shakeMagnitude, float dampingSpeed)
    {
        Vector3 initialPosition = objectToShake.transform.localPosition;
        float shakeTimer = shakeDuration;

        while (shakeTimer > 0)
        {
            objectToShake.transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

            shakeTimer -= Time.deltaTime * dampingSpeed;

            if (shakeTimer <= 0)
            {
                objectToShake.transform.localPosition = initialPosition;
            }
        }
    }
}
