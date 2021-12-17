using WormsWorld.model;

namespace WormsWorld.service
{
    public class WormAiService : IWormAiService
    {
        private readonly IAiService _foodService;

        public WormAiService(IAiService foodService)
        {
            _foodService = foodService;
        }

        public WormAction MakeDecision(Worm worm, Coord curCoord)
        {
            var nearestFoodCoord = _foodService.GetNearest(curCoord);
            if (nearestFoodCoord.GetX() > 0)
            {
                return new Worm.MoveAction(worm, Direction.Right);
            }

            if (nearestFoodCoord.GetX() < 0)
            {
                return new Worm.MoveAction(worm, Direction.Left);
            }

            if (nearestFoodCoord.GetY() > 0)
            {
                return new Worm.MoveAction(worm, Direction.Up);
            }

            if (nearestFoodCoord.GetY() < 0)
            {
                return new Worm.MoveAction(worm, Direction.Down);
            }

            return null;
        }
    }
}