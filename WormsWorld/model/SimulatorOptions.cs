namespace WormsWorld.model
{
    public class SimulatorOptions
    {
        public static int Iterations { get; set; } = 100;

        public static int WormHpStart { get; set; } = 10;

        public static int WormHpBorderline { get; set; } = 0;

        public static int FoodHpStart { get; set; } = 10;

        public static int FoodHpBorderline { get; set; } = 0;

        public static int FoodHpGain { get; set; } = 10;

        public static int ReproductionHpCost { get; set; } = 10;

        public static int ReproductionLifeRequirement { get; set; } = 10;
    }
}