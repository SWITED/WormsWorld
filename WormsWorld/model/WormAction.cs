namespace WormsWorld.model
{
    public abstract class WormAction
    {
        protected readonly Worm Worm;
        protected readonly Direction Direction;

        protected WormAction(Worm owner, Direction direction)
        {
            Worm = owner;
            Direction = direction;
        }

        public Direction GetDirection()
        {
            return Direction;
        }

        public abstract void Approve();
    }
}