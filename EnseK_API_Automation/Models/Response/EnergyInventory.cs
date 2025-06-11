namespace EnseK_API_Automation.Models.Response
{
    public class EnergyInventory
    {
        public EnergySource Electric { get; set; }
        public EnergySource Gas { get; set; }
        public EnergySource Nuclear { get; set; }
        public EnergySource Oil { get; set; }
    }

    public class EnergySource
    {
        public int Energy_Id { get; set; }
        public double Price_Per_Unit { get; set; }
        public int Quantity_Of_Units { get; set; }
        public string Unit_Type { get; set; }
    }
}
