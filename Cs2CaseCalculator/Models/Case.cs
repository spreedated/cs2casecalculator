namespace Cs2CaseCalculator.Models
{
    public record Case
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Imagepath { get; set; }
    }
}
