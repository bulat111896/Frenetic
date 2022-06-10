using System.Collections;
using UnityEngine;

public class GrassController : MonoBehaviour
{
    public static bool isOnTheStreet;
    bool isActive = true;
    public Transform player;
    Transform[] grass;
    int[] mass;

    bool isCan(ref int i)
    {
        return Vector3.Distance(player.position, grass[i].position) < 8.5f && Vector3.Angle(player.TransformDirection(Vector3.forward), new Vector3(grass[i].position.x - player.position.x, 0, grass[i].position.z - player.position.z)) < 50f;
    }

    IEnumerator Wait()
    {
        while (true)
        {
            if (isOnTheStreet)
            {
                if (!isActive)
                {
                    for (int i = 0; i < grass.Length; i++)
                        grass[i].gameObject.SetActive(true);
                    isActive = true;
                }
                for (int j = 0; j < 4; j++)
                {
                    for (int i = mass[j]; i < mass[j + 1]; i++)
                        grass[i].gameObject.SetActive(isCan(ref i));
                    yield return new WaitForSeconds(0.02f);
                }
            }
            else if (isActive)
            {
                for (int i = 0; i < grass.Length; i++)
                    grass[i].gameObject.SetActive(false);
                isActive = false;
            }
            yield return null;
        }
    }

    void Start()
    {
        isOnTheStreet = false;
        grass = new Transform[transform.childCount];
        for (int i = 0; i < grass.Length; i++)
        {
            grass[i] = transform.GetChild(i);
        }
        mass = new int[] { 0, grass.Length / 4, grass.Length / 2, grass.Length / 2 + grass.Length / 4, grass.Length };
        StartCoroutine(Wait());
    }
}