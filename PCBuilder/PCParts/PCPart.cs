namespace PCBuilder.PCParts
{
    public abstract class PCPart
    {
        public string Name { get; set; }
        public bool IsSuppliesPower { get; set; }
        public int Power { get; set; }
        public bool IsAbsorbHeat { get; set; }
        public int Temperature { get; set; }
    }
}