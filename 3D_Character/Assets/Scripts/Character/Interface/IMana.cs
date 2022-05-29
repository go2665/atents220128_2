//HealthDelegate라는 delegate type을 만든 것(리턴타입이 void이고 파라메터가 하나도 없는 함수를 저장할 수 있는 델리게이트다)
public delegate void ManaDelegate();

public interface IMana
{
    float MP { get; set; }  // 현재 MP 확인 및 설정 용도
    float MaxMP { get; }    // 최대 MP 확인 용도

    //onManaChange 변수에 리턴타입이 void이고 파라메터가 하나도 없는 함수를 저장할 수 있다.
    ManaDelegate onManaChange { get; set; } 
}