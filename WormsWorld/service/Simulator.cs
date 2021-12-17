using Microsoft.Extensions.Logging;
using WormsWorld.model;
using WormsWorld.service.@interface;

namespace WormsWorld.service
{
    internal class Simulator : ISimulator
    {
        private int _step;
        private readonly IFoodService _foodService;
        private readonly IWormService _wormService;
        private readonly ILogger<ISimulator> _logger;

        public Simulator(IFoodService foodService, IWormService wormService, ILogger<ISimulator> logger)
        {
            _foodService = foodService;
            _wormService = wormService;
            _logger = logger;
        }

        private void NextStep()
        {
            Coord foodCoord;
            do
            {
                foodCoord = _foodService.GenerateCell();
            } while (!_foodService.IsCellEmpty(foodCoord));

            _foodService.NextStep(foodCoord);
            _wormService.NextStep(foodCoord);
            _logger.LogInformation($"{_wormService}, {_foodService}");
        }

        public void Start()
        {
            while (_step < SimulatorOptions.Iterations)
            {
                NextStep();
                _step++;
            }
        }

        public void Start(int iterations)
        {
            while (_step < iterations)
            {
                NextStep();
                _step++;
            }
        }
    }
}