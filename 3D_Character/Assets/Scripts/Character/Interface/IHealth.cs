//HealthDelegate라는 delegate type을 만든 것(리턴타입이 void이고 파라메터가 하나도 없는 함수를 저장할 수 있는 델리게이트다)
public delegate void HealthDelegate();

public interface IHealth
{
    float HP { get; set; }       // 현재 HP 확인 및 설정 용도
    float MaxHP { get; }    // 최대 HP 확인 용도

    //onHealthChange라는 변수에 리턴타입이 void이고 파라메터가 하나도 없는 함수를 저장할 수 있다.
    HealthDelegate onHealthChange { get; set; } 
}