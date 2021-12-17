namespace WormsWorld.model
{
    public sealed class Worm : GameObject
    {
        private readonly string _name;

        public Worm(Coord position, string name) : base(position, SimulatorOptions.WormHpStart)
        {
            _name = name;
        }

        public override string ToString()
        {
            return $"{_name}-{Health} " + base.ToString();
        }

        public bool DecreaseHpAndCheckDead()
        {
            return --Health <= SimulatorOptions.WormHpBorderline;
        }

        public void IncreaseHealth()
        {
            Health += SimulatorOptions.FoodHpGain;
        }

        public class MoveAction : WormAction
        {
            public MoveAction(Worm owner, Direction direction) : base(owner, direction)
            {
            }

            public override void Approve()
            {
                Worm.Position += Direction;
            }
        }

        public class SplitAction : WormAction
        {
            public SplitAction(Worm owner, Direction direction) : base(owner, direction)
            {
            }

            public override void Approve()
            {
                Worm.Health -= SimulatorOptions.ReproductionHpCost;
            }
        }
    }
}