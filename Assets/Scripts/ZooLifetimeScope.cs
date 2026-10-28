using MessagePipe;
using TastyZoo.Animals;
using TastyZoo.Spawning;
using TastyZoo.UI;
using TastyZoo.World;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TastyZoo
{
    public sealed class ZooLifetimeScope : LifetimeScope
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private SpawnConfig _spawnConfig;
        [SerializeField] private float _arenaMargin = 3f;
        [SerializeField] private Transform _animalsRoot;
        [SerializeField] private DeathCounterView _deathCounter;
        [SerializeField] private TastyLabelView _tastyLabelPrefab;
        [SerializeField] private RectTransform _labelsRoot;

        protected override void Configure(IContainerBuilder builder)
        {
            var options = builder.RegisterMessagePipe();
            builder.RegisterMessageBroker<AnimalEaten>(options);

            builder.RegisterInstance(_camera);
            builder.RegisterInstance(_spawnConfig);
            builder.RegisterComponent(_deathCounter);
            builder.Register<ArenaBounds>(Lifetime.Singleton).WithParameter("margin", _arenaMargin);
            builder.Register<AnimalFactory>(Lifetime.Singleton).WithParameter(_animalsRoot);

            builder.RegisterEntryPoint<AnimalsController>().AsSelf();
            builder.RegisterEntryPoint<SpawnController>();
            builder.RegisterEntryPoint<DeathCounterController>();
            builder.RegisterEntryPoint<TastyLabelsController>()
                .WithParameter(_tastyLabelPrefab)
                .WithParameter(_labelsRoot);
        }
    }
}
