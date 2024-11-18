using UnityEngine;
public interface IItem
{  //느슨한 커플링, 모든 아이템이 해당 인터페이스를 상속. 사용했을 때 효과를 해당 함수에서 구현
    void CatchItem(); //사용자가 아이템을 주웠을 때 호출되는 함수
    void Use(); //사용자가 아이템을 사용했을 때 호출되는 함수.
    void ItemUIOn(); //각각 UIManager에 구현된 함수를 호출, 매개변수 true를 사용하여 하나의 함수로 UI관리 
}
