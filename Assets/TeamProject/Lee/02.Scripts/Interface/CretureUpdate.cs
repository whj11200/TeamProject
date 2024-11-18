using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//������, ���� �Լ�, ü��ȸ���� ���� �Լ����� �⺻ ���ڸ� ����, ����ü�鿡�� �ʿ��� ��ɸ� �̾Ƽ� ���.
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
