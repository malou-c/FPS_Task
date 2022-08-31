using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private HPBar _healthBar;
    [SerializeField] [Min(1)] private int _maxHealth;

    private int _currHealth;

    void Start()
    {
        _currHealth = _maxHealth;
    }

    public void OnTakeDamage(int damage)
    {
        _currHealth -= damage;
        _healthBar.SetFillAmount((float)_currHealth / (float)_maxHealth);
    }
    public bool IsDie() => _currHealth <= 0;
    public void ResetHp()
    {
        _currHealth = _maxHealth;
        _healthBar.SetFillAmount((float)_currHealth / (float)_maxHealth);
    }

}
