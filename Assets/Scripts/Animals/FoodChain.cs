namespace TastyZoo.Animals
{
    public static class FoodChain
    {
        public static Animal FindEater(Animal first, Animal second)
        {
            var firstEats = first.CanEat(second);
            var secondEats = second.CanEat(first);
            if (firstEats && secondEats)
                return first.SpawnOrder < second.SpawnOrder ? first : second;

            if (firstEats)
                return first;

            return secondEats ? second : null;
        }
    }
}
