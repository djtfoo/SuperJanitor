using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject crosshairRegular;
    [SerializeField]
    private GameObject crosshairSelected;

    // Start is called before the first frame update
    void Start()
    {
        SetCrosshairRegular();
    }

    public void SetCrosshairRegular()
    {
        crosshairRegular.SetActive(true);
        crosshairSelected.SetActive(false);
    }
    public void SetCrosshairSelected()
    {
        crosshairRegular.SetActive(false);
        crosshairSelected.SetActive(true);
    }
}
