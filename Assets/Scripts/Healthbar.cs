using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Image _healthbarSprite;

    public void updateHealthBar(float maxHealth, float currentHealth)
    {
        Debug.LogWarning("Update Health Bar");
        _healthbarSprite.fillAmount = currentHealth/maxHealth;
    }
}
