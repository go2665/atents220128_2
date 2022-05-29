
interface IEquippableItem
{
    void EquipItem(IEquippableCharacter target);
    void UnEquipItem(IEquippableCharacter target);
    bool ToggleEquipItem(IEquippableCharacter target);
}