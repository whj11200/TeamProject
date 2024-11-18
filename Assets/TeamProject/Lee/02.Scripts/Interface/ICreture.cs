using UnityEngine;
//생명체가 반드시 가져야할 함수를 미리 선언
public interface ICreture
{
    void OnDamage(object[] param);
}
