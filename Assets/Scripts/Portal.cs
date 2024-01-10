using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
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
    public GameObject portalPrefab;  // Drag your Portal Prefab here in the Inspector
    private bool portalSpawned = false;
    int ok = 1;
    private void Start()
    {
        _currentHealth = maxHealth;
      //  img.sprite = spriteChoices[0];
        _healthbar.updateHealthBar(maxHealth, _currentHealth);
        Debug.LogWarning("AICI E STARTTT!!");
        if (!portalSpawned)
        {
         //   SpawnPortal();
            portalSpawned = true;
        }

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

    
    void SpawnPortal()
    {
        // Set the spawn position
        Vector3 spawnPosition = new Vector3(-3f, 0f, 0f);  // Set the desired spawn position

        // Instantiate the portal prefab at the spawn position
        GameObject portalInstance = Instantiate(portalPrefab, spawnPosition, Quaternion.identity);

        // Optionally, you can do additional setup or modifications to the spawned portal instance
        // For example, you might want to set its properties or attach it to a specific parent object.

        // portalInstance.transform.parent = someParentGameObject;  // Set a parent if needed
    }
    
}
