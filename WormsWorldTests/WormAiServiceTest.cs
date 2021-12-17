using NSubstitute;
using NUnit.Framework;
using WormsWorld;
using WormsWorld.model;
using WormsWorld.service;

namespace WormsWorldTests
{
    public class WormAiServiceTest
    {
        private IWormAiService _wormAiService;
        private IAiService _foodService;
        private readonly Coord _nearest = new(1, 1);
        private readonly Coord _startCoord = new(0, 0);

        [SetUp]
        public void Setup()
        {
            _foodService = Substitute.For<IAiService>();
            _wormAiService = new WormAiService(_foodService);
        }

        [Test]
        public void MakeDecision_MoveToNearestFood_DistanceShrinking()
        {
            var worm = new Worm(new Coord(0, 0), "name");
            _foodService.GetNearest(default).ReturnsForAnyArgs(_nearest);

            var action = _wormAiService.MakeDecision(worm, _startCoord);
            Assert.True(WorldUtil.TaxicabDistance(_startCoord + action.GetDirection(), _nearest) <
                        WorldUtil.TaxicabDistance(_startCoord, _nearest));
        }
    }
}