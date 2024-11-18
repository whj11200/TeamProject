using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//데미지, 죽음 함수, 체력회복과 같은 함수들의 기본 골자를 제작, 생명체들에서 필요한 기능만 뽑아서 사용.
public class CretureUpdate : MonoBehaviour,ICreture
{
    protected float startHealth = 10.0f;
    public float health { get; protected set; }
    public bool dead { get; protected set; }

    protected virtual void OnEnable()
    {
        dead = false;
        health = startHealth;
    }

    public virtual void OnDamage(object[] param)
    {
        health -= (float)param[1];
        if(health <= 0 && !dead)
        {
            Die();
        }
    }

    public virtual void AddHealth(float AddHealth)
    {
        if(dead) return;
        health += AddHealth;
    }

    public virtual void Die()
    {
        dead = true;
    }
}
