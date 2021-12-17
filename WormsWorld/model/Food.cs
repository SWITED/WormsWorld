namespace WormsWorld.model
{
    public sealed class Food : GameObject
    {
        public Food(Coord position) : base(position, SimulatorOptions.FoodHpStart)
        {
        }

        public bool DecreaseHpAndCheckDead()
        {
            return --Health <= SimulatorOptions.FoodHpBorderline;
        }
    }
}