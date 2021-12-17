namespace WormsWorld.model
{
    public abstract class GameObject
    {
        protected int Health;
        protected Coord Position;

        protected GameObject(Coord position, int startHp)
        {
            Position = position;
            Health = startHp;
        }

        public int GetHealth()
        {
            return Health;
        }

        public override string ToString()
        {
            return $"{Position}";
        }
    }
}