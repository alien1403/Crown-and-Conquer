using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    [SerializeField] private float maxHealth = 10;
    [SerializeField] private GameObject _hiteffect, _deatheffect;
    private float _currentHealth;
    public Image img;
    public List<Sprite> spriteChoices;
    [SerializeField] private Healthbar _healthbar;
    
    private void Start()
    {
        _currentHealth = maxHealth;
      //  img.sprite = spriteChoices[0];
        _healthbar.updateHealthBar(maxHealth, _currentHealth);

    }

  
    void OnMouseDown()
    {
        
        _currentHealth -= Random.Range(0.3f, 1f);
        if (_currentHealth < 0)
        {
            Destroy(gameObject);
        } 
        else
        {
            
            _healthbar.updateHealthBar(maxHealth, _currentHealth);
            if(img != null){
                img.sprite = spriteChoices[1];
            }
           // img.sprite = spriteChoices[0];
            //  Instantiate(_hiteffect, transform.position, Quaternion.identity);
            Debug.LogWarning("Da");
        }
    }
}
