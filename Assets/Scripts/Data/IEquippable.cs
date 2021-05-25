namespace LH.Data {
    public interface IEquippable {
        public bool IsEquipped { get; set; }
        
        public void EquipItem();
    }
}
