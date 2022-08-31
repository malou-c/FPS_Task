using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField] private Image _hpBarFront;

    public void SetFillAmount(float newAmount)
    {
        Debug.Log(newAmount);
        _hpBarFront.fillAmount = newAmount;
    }

    private void ResetBar()
    {
        _hpBarFront.fillAmount = 1f;
    }
}
