using UnityEngine;
using System.Collections;
using Nova;

public static class NovaUIBlock2DShake
{
    public static void Shake(UIBlock2D block, float duration, float magnitude)
    {
        block.StartCoroutine(ShakeCoroutine(block, duration, magnitude));
    }

    private static IEnumerator ShakeCoroutine(UIBlock2D block, float duration, float magnitude)
    {
        Vector3 originalPos = block.transform.position;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            block.transform.position = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        block.transform.position = originalPos;
    }
}
