using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using WormsWorld.model;

namespace WormsWorld.service
{
    public class WormService : IWormService
    {
        private readonly Dictionary<Coord, Worm> _worms = new();
        private readonly IRemovable _foodService;
        private readonly IWormAiService _wormAi;
        private readonly INameService _nameService;

        public WormService(IRemovable foodService, IWormAiService wormAi, INameService nameService)
        {
            _foodService = foodService;
            _wormAi = wormAi;
            _nameService = nameService;
            _worms.Add(new Coord(0, 0), new Worm(new Coord(0, 0), _nameService.GenerateRandomName()));
        }

        public void NextStep(Coord newFoodCoord)
        {
            var forRemove = new List<Coord>();
            var changeCoord = new Dictionary<Coord, Worm>();
            var newWorms = new Dictionary<Coord, Worm>();
            if (_worms.ContainsKey(newFoodCoord))
            {
                _worms[newFoodCoord].IncreaseHealth();
                _foodService.ClearCell(newFoodCoord);
            }
            foreach (var (curCoord, worm) in _worms)
            {
                var wormAction = _wormAi.MakeDecision(worm, curCoord);
                if (!ValidateAction(worm, curCoord, wormAction))
                {
                    continue;
                }

                var newCoord = curCoord + wormAction.GetDirection();
                switch (wormAction)
                {
                    case Worm.MoveAction:
                        forRemove.Add(curCoord);
                        if (!_foodService.IsCellEmpty(newCoord))
                        {
                            worm.IncreaseHealth();
                            _foodService.ClearCell(newCoord);
                        }

                        changeCoord.Add(newCoord, worm);
                        break;
                    case Worm.SplitAction:
                        newWorms.Add(newCoord, new Worm(newCoord, _nameService.GenerateRandomName()));
                        break;
                }
                wormAction.Approve();
            }

            foreach (var (coord, worm) in changeCoord)
            {
                _worms.Add(coord, worm);
            }
            
            foreach (var (coord, worm) in newWorms)
            {
                _worms.Add(coord, worm);
            }

            foreach (var coord in forRemove)
            {
                _worms.Remove(coord);
            }

            FinishStep(newWorms);
        }

        public ImmutableDictionary<Coord, Worm> GetState()
        {
            return _worms.ToImmutableDictionary();
        }

        private bool ValidateAction(Worm worm, Coord curCoord, WormAction action)
        {
            if (action == null)
            {
                return false;
            }

            var newPosition = curCoord + action.GetDirection();
            if (_worms.ContainsKey(newPosition))
            {
                return false;
            }

            if (action is not Worm.SplitAction)
            {
                return true;
            }
            
            return worm.GetHealth() > SimulatorOptions.ReproductionHpCost && _foodService.IsCellEmpty(newPosition);
        }

        private void FinishStep(Dictionary<Coord, Worm> newWorms)
        {
            var forRemove = _worms
                .Where(keyValuePair => !newWorms.ContainsKey(keyValuePair.Key) && keyValuePair.Value.DecreaseHpAndCheckDead())
                .Select(keyValuePair => keyValuePair.Key).ToList();
            foreach (var coord in forRemove)
            {
                _worms.Remove(coord);
            }
        }

        public bool IsCellEmpty(Coord coord)
        {
            return !_worms.ContainsKey(coord);
        }

        public override string ToString()
        {
            return $"Worms: [{string.Join(", ", _worms.Select(worm => worm.Value.ToString()).ToList())}]";
        }
    }
}