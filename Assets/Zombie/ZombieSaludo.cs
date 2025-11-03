using UnityEngine;

public class ZombieSaludo : MonoBehaviour
{
    [SerializeField] GameObject myGameObj;

    private void Awake()
    {
        myGameObj.SetActive(true); // empieza prendido
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleGameObject();
        }
    }

    void ToggleGameObject()
    {
        myGameObj.SetActive(!myGameObj.activeSelf); // cambia entre on/off
    }
}
