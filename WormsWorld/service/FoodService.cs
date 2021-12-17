using System;
using System.Collections.Generic;
using System.Linq;
using WormsWorld.model;

namespace WormsWorld.service
{
    public class FoodService : IFoodService, IAiService
    {
        private static readonly Random Random = new();
        private readonly Dictionary<Coord, Food> _food = new();

        public bool IsCellEmpty(Coord coord)
        {
            return !_food.ContainsKey(coord);
        }

        public void ClearCell(Coord coord)
        {
            _food.Remove(coord);
        }

        public Coord GetNearest(Coord myCoord)
        {
            var coord = _food.Keys.Aggregate((curMin, x)
                => curMin == null ||
                   WorldUtil.TaxicabDistance(myCoord, x) < WorldUtil.TaxicabDistance(myCoord, curMin)
                    ? x
                    : curMin);
            return coord - myCoord;
        }

        public Coord GenerateCell()
        {
            return new Coord(Random.NextNormal(0, Math.Sqrt(5)), Random.NextNormal(0, Math.Sqrt(5)));
        }

        private void DecreaseFoodHpAndRemoveDead()
        {
            var forRemove = _food.Where(keyValuePair =>
                    keyValuePair.Value.DecreaseHpAndCheckDead())
                .Select(keyValuePair => keyValuePair.Key).ToList();
            foreach (var coord in forRemove)
            {
                _food.Remove(coord);
            }
        }

        public Coord NextStep(Coord newCoord)
        {
            DecreaseFoodHpAndRemoveDead();
            var food = new Food(newCoord);
            _food.Add(newCoord, food);
            return newCoord;
        }

        public override string ToString()
        {
            return $"Food: [{string.Join(", ", _food.Select(coordFood => coordFood.Value.ToString()).ToList())}]";
        }
    }
}