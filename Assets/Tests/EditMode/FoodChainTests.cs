using System.Collections.Generic;
using NUnit.Framework;
using TastyZoo.Animals;
using UnityEditor;
using UnityEngine;

namespace TastyZoo.Tests
{
    public sealed class FoodChainTests
    {
        private readonly List<Object> _created = new();

        private FoodGroup _prey;
        private FoodGroup _predator;

        [SetUp]
        public void SetUp()
        {
            _prey = Track(ScriptableObject.CreateInstance<FoodGroup>());
            _predator = Track(ScriptableObject.CreateInstance<FoodGroup>());
            SetDiet(_prey);
            SetDiet(_predator, _prey, _predator);
        }

        [TearDown]
        public void TearDown()
        {
            foreach (var created in _created)
                Object.DestroyImmediate(created);

            _created.Clear();
        }

        [Test]
        public void PreyMeetsPrey_NobodyEats()
            => Assert.IsNull(FoodChain.FindEater(Create(0, _prey), Create(1, _prey)));

        [Test]
        public void PredatorMeetsPrey_PredatorEats()
        {
            var predator = Create(0, _predator);
            var prey = Create(1, _prey);

            Assert.AreSame(predator, FoodChain.FindEater(predator, prey));
            Assert.AreSame(predator, FoodChain.FindEater(prey, predator));
        }

        [Test]
        public void PredatorMeetsPredator_ElderEats()
        {
            var elder = Create(0, _predator);
            var younger = Create(1, _predator);

            Assert.AreSame(elder, FoodChain.FindEater(elder, younger));
            Assert.AreSame(elder, FoodChain.FindEater(younger, elder));
        }

        private Animal Create(int spawnOrder, FoodGroup group)
        {
            var config = Track(ScriptableObject.CreateInstance<AnimalConfig>());
            var serialized = new SerializedObject(config);
            serialized.FindProperty("_foodGroup").objectReferenceValue = group;
            serialized.ApplyModifiedPropertiesWithoutUndo();
            return new(spawnOrder, config, null, null, null, null, Vector3.forward);
        }

        private T Track<T>(T created) where T : Object
        {
            _created.Add(created);
            return created;
        }

        private static void SetDiet(FoodGroup group, params FoodGroup[] diet)
        {
            var serialized = new SerializedObject(group);
            var property = serialized.FindProperty("_diet");
            property.arraySize = diet.Length;
            for (var i = 0; i < diet.Length; i++)
                property.GetArrayElementAtIndex(i).objectReferenceValue = diet[i];

            serialized.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
