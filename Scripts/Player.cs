using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameController gameCtrl;
    public GameObject lantern, fire, reloading, aim, aimDot, pistol;
    public Image gunSprite;
    public Text count;
    public Transform gun, cam, Null;
    float saveFieldOfView;

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.name != "Rahmen")
            col.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3((transform.position - col.transform.position).x, 0, (transform.position - col.transform.position).z).normalized * -120f);
    }

    void SetActiveNotRealGun(bool isActive)
    {
        for (int i = 0; i < 3; i++)
            gun.GetChild(i).gameObject.SetActive(false);
        pistol.SetActive(!isActive);
        lantern.SetActive(isActive);
        fire.SetActive(!isActive);
        reloading.SetActive(!isActive);
        aim.SetActive(!isActive);
        aimDot.SetActive(!isActive);
        if (gunSprite.color.g > 0.5f)
        {
            gunSprite.color = new Color(1, 1, 1, isActive ? 0.4f : 0.8f);
            count.color = new Color(1, 1, 1, isActive ? 0.4f : 0.8f);
        }
    }

    void SetActive(bool isActive)
    {
        pistol.SetActive(false);
        for (int i = 0; i < 3; i++)
            gun.GetChild(i).gameObject.SetActive(!isActive);
        lantern.SetActive(isActive);
        fire.SetActive(!isActive);
        reloading.SetActive(!isActive);
        aim.SetActive(!isActive);
        aimDot.SetActive(!isActive);
        if (gunSprite.color.g > 0.5f)
        {
            gunSprite.color = new Color(1, 1, 1, isActive ? 0.4f : 0.8f);
            count.color = new Color(1, 1, 1, isActive ? 0.4f : 0.8f);
        }
    }

    public void GetNotRealGun()
    {
        if (!pistol.activeSelf)
            gameCtrl.Drop();
        SetActiveNotRealGun(pistol.activeSelf);
    }

    public void GetGunButton()
    {
        if (fire.GetComponent<Fire>().isPistol)
            GetNotRealGun();
        else
            GetGun();
    }

    public void GetGun()
    {
        if (gunSprite.gameObject.activeSelf)
        {
            if (lantern.activeSelf)
                gameCtrl.Drop();
            SetActive(gun.GetChild(0).gameObject.activeSelf);
            if (lantern.activeSelf)
            {
                Null.transform.localPosition = cam.localPosition;
                Camera.main.fieldOfView = saveFieldOfView;
                Fire.isAim = false;
                gun.GetComponent<GunPhisics>().Disable();
            }
            else
            {
                gun.GetComponent<GunPhisics>()._Enter();
            }
        }
    }

    void Start()
    {
        saveFieldOfView = Camera.main.fieldOfView;
    }

    void Update()
    {
        Null.rotation = Fire.isAim ? cam.rotation : Quaternion.Lerp(Null.rotation, cam.rotation, 10 * Time.deltaTime);
    }
}