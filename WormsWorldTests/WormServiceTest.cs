using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using NSubstitute;
using NUnit.Framework;
using WormsWorld;
using WormsWorld.model;
using WormsWorld.service;

namespace WormsWorldTests
{
    public class WormServiceTest
    {
        private IWormService _wormService;
        private IFoodService _foodService;
        private IWormAiService _wormAiService;
        private INameService _nameService;
        private readonly Coord _startWormCoord = new(0, 0);
        private readonly string _defaultName = "name";
        
        
        [SetUp]
        public void Setup()
        {
            _foodService = Substitute.For<IFoodService>();
            _wormAiService = Substitute.For<IWormAiService>();
            _nameService = Substitute.For<INameService>();
            _nameService.GenerateRandomName().ReturnsForAnyArgs(_defaultName);
            _wormService = new WormService(_foodService, _wormAiService, _nameService);
        }

        [Test]
        public void NextStep_MoveToEmptyCell_MoveApproved()
        {
            var startWorm = _wormService.GetState()[_startWormCoord];
            var action = new Worm.MoveAction(startWorm, Direction.Up);
            _wormAiService.MakeDecision(startWorm, _startWormCoord)
                .ReturnsForAnyArgs(action);
            _foodService.IsCellEmpty(_startWormCoord + action.GetDirection()).Returns(true);
            
            _wormService.NextStep(new Coord(1, 1));
            var result = _wormService.GetState();
            Assert.True(result.ContainsKey(_startWormCoord + action.GetDirection()));
            Assert.AreEqual("Worms: [name-9 (0, 1)]",_wormService.ToString());
        }
        
        [Test]
        public void NextStep_MoveToCellWithFood_MoveApproved()
        {
            var startWorm = _wormService.GetState()[_startWormCoord];
            var action = new Worm.MoveAction(startWorm, Direction.Up);
            _wormAiService.MakeDecision(startWorm, _startWormCoord)
                .ReturnsForAnyArgs(action);
            _foodService.IsCellEmpty(_startWormCoord + action.GetDirection()).Returns(false);
            
            _wormService.NextStep(new Coord(1, 1));
            var result = _wormService.GetState();
            Assert.True(result.ContainsKey(_startWormCoord + action.GetDirection()));
            Assert.AreEqual("Worms: [name-19 (0, 1)]",_wormService.ToString());
        }
        
        [Test]
        public void NextStep_MoveToCellWithoutWorm_MoveDeclined()
        {
            var startWorm = _wormService.GetState()[_startWormCoord];
            startWorm.IncreaseHealth();
            var splitAction = new Worm.SplitAction(startWorm, Direction.Up);
            _wormAiService.MakeDecision(startWorm, _startWormCoord)
                .Returns(splitAction);
            var newWormCoord = _startWormCoord + splitAction.GetDirection();
            _foodService.IsCellEmpty(Arg.Any<Coord>()).Returns(true);

            _wormService.NextStep(new Coord(1, 1));
            
            var newWorm = _wormService.GetState()[newWormCoord];
            var moveAction = new Worm.MoveAction(newWorm, Direction.Down);
            _wormAiService.MakeDecision(newWorm, newWormCoord)
                .Returns(moveAction);
            _wormService.NextStep(new Coord(1, 1));
            var state = _wormService.GetState();
            Assert.AreEqual("name-8 (0, 0)",state[_startWormCoord].ToString());
            Assert.AreEqual("name-9 (0, 1)",state[newWormCoord].ToString());
        }
        
        [Test]
        public void NextStep_SplitToCellWithoutWorm_MoveApproved()
        {
            var startWorm = _wormService.GetState()[_startWormCoord];
            startWorm.IncreaseHealth();
            var action = new Worm.SplitAction(startWorm, Direction.Up);
            _wormAiService.MakeDecision(startWorm, _startWormCoord)
                .ReturnsForAnyArgs(action);
            _foodService.IsCellEmpty(_startWormCoord + action.GetDirection()).Returns(true);
            
            _wormService.NextStep(new Coord(1, 1));
            var state = _wormService.GetState();
            Assert.AreEqual("name-9 (0, 0)",state[_startWormCoord].ToString());
            Assert.AreEqual("name-10 (0, 1)",state[_startWormCoord + action.GetDirection()].ToString());
        }
        
        [Test]
        public void NextStep_SplitToCellWithWorm_MovedDeclined()
        {
            var startWorm = _wormService.GetState()[_startWormCoord];
            startWorm.IncreaseHealth();
            var action = new Worm.SplitAction(startWorm, Direction.Up);
            _wormAiService.MakeDecision(startWorm, _startWormCoord)
                .ReturnsForAnyArgs(action);
            _foodService.IsCellEmpty(_startWormCoord + action.GetDirection()).Returns(true);
            
            _wormService.NextStep(new Coord(1, 1));
            _wormService.NextStep(new Coord(1, 1));
            var state = _wormService.GetState();
            Assert.AreEqual("name-8 (0, 0)",state[_startWormCoord].ToString());
            Assert.AreEqual("name-9 (0, 1)",state[_startWormCoord + action.GetDirection()].ToString());
        }
        
        [Test]
        public void NextStep_SplitToCellNotEnoughHp_MovedDeclined()
        {
            var startWorm = _wormService.GetState()[_startWormCoord];
            var action = new Worm.SplitAction(startWorm, Direction.Up);
            _wormAiService.MakeDecision(startWorm, _startWormCoord)
                .ReturnsForAnyArgs(action);
            _foodService.IsCellEmpty(_startWormCoord + action.GetDirection()).Returns(true);
            
            _wormService.NextStep(new Coord(1, 1));

            Assert.AreEqual("Worms: [name-9 (0, 0)]",_wormService.ToString());
        }
        
        [Test]
        public void NextStep_SplitToCellWithFood_MovedDeclined()
        {
            var startWorm = _wormService.GetState()[_startWormCoord];
            startWorm.IncreaseHealth();
            var action = new Worm.SplitAction(startWorm, Direction.Up);
            _wormAiService.MakeDecision(startWorm, _startWormCoord)
                .ReturnsForAnyArgs(action);
            _foodService.IsCellEmpty(_startWormCoord + action.GetDirection()).Returns(false);
            
            _wormService.NextStep(new Coord(1, 1));

            Assert.AreEqual("Worms: [name-19 (0, 0)]",_wormService.ToString());
        }
    }
}