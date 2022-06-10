using System.Collections;
using UnityEngine;

public class Flashing : MonoBehaviour
{
    Light _light;
    float intensity;

    IEnumerator Wait()
    {
        while (true)
        {
            if (Random.Range(0, 3) % 2 == 0)
                yield return new WaitForSeconds(Random.Range(15f, 25f));
            yield return new WaitForSeconds(Random.Range(2f, 5f));
            for (int j = Random.Range(1, 4); j > 0; j--)
            {
                while (true)
                {
                    _light.intensity = Mathf.Lerp(_light.intensity, 0, Time.deltaTime * 100);
                    yield return null;
                    if (_light.intensity < intensity / 2)
                        break;
                }
                yield return new WaitForSeconds(Random.Range(0f, 0.15f));
                while (true)
                {
                    _light.intensity = Mathf.Lerp(_light.intensity, intensity, Time.deltaTime * 120);
                    yield return null;
                    if (_light.intensity > intensity - 0.01f)
                        break;
                }
            }
        }
    }

    void Start()
    {
        _light = GetComponent<Light>();
        intensity = _light.intensity;
        StartCoroutine(Wait());
    }
}