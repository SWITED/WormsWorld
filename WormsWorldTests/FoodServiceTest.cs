using System;
using NUnit.Framework;
using WormsWorld;
using WormsWorld.model;
using WormsWorld.service;

namespace WormsWorldTests
{
    public class Tests
    {
        private IFoodService _foodService;

        [SetUp]
        public void Setup()
        {
            _foodService = new FoodService();
        }

        [Test]
        public void CreateFood_CellEmpty_FoodCreated()
        {
            Coord cellForFood = _foodService.GenerateCell();
            _foodService.NextStep(cellForFood);
            Assert.False(_foodService.IsCellEmpty(cellForFood));
        }

        [Test]
        public void CreateFood_CellContainsFood_ThrowArgumentException()
        {
            Coord cellForFood = _foodService.GenerateCell();
            _foodService.NextStep(cellForFood);
            Assert.Throws<ArgumentException>(() => _foodService.NextStep(cellForFood));
        }
    }
}