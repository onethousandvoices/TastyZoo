# Tasty Zoo

A Unity 6 top-down zoo simulation where animals spawn on a meadow, wander around, collide by physics and follow a data-driven food chain.

## Overview

Every 1-2 seconds a random animal appears at a free spot of the visible arena and starts wandering: it picks a random heading and periodically turns by a random angle. Frogs jump a fixed distance on a fixed interval, snakes crawl at a fixed speed. When an animal heads out of the screen it turns back towards a random point inside the arena (the turn starts a small margin before the edge, so animals never vanish), and log walls in the middle of the meadow reflect animals like a mirror.

Animals interact on collision. Two prey bounce apart, a predator eats anything it meets with an attack animation while the prey is pulled into its mouth, and when two predators meet the elder one (spawned earlier) survives. Every meal shows a "Tasty!" label under the predator, and the counter in the top-right corner tracks how many prey and predators were eaten.

## Key Features

- **Data-driven food chain** - food groups are ScriptableObjects with a diet list, so new relations need no code; the prey/predator role is a separate asset that only drives the counter
- **Pluggable movement and behaviour** - `MovementConfig` creates how an animal moves, `BehaviourConfig` creates where it wants to go; both are strategies, so a new way of moving or deciding needs only new files
- **Physics-based movement** - `JumpArc` solves the projectile for an exact distance and height and the frog stops on landing; crawling accelerates the body along its facing, so external impulses still work
- **Animation-synced jumps** - the jump clip is split into crouch, flight and landing; the impulse fires at take-off and the flight clip is stretched to the physical flight time
- **Obstacles** - any static wall contact reflects the heading; obstacle colliders are taller than a jump, spawning waits for a free spot
- **Screen bounds** - the arena is the camera frustum projected on the ground, inset by a margin, and follows resolution changes
- **Decoupled events** - `AnimalEaten` carries only the eaten role and the meal position, UI subscribes through MessagePipe without touching animals
- **Object pooling** - animals and labels are recycled with `UnityEngine.Pool.ObjectPool`
- **uGUI** - one counter row per role generated from the spawn config, "Tasty!" labels pinned to the meal point
- **EditMode tests** - food chain rules and jump math are covered by NUnit tests

## Tech Stack

| Technology | Role |
|---|---|
| Unity 6 (6000.3.21f1) | Engine |
| Universal Render Pipeline | Rendering |
| VContainer 1.19.0 | Dependency injection and entry points |
| MessagePipe 1.8.2 | In-process pub/sub |
| UniTask 2.5.10 | Required by MessagePipe, not used by game code (async uses `Awaitable`) |
| C# 10 (`Assets/csc.rsp`) | `readonly record struct` for messages and value data |
| DOTween | Spawn, death, counter and label animations |
| uGUI + TextMesh Pro | Counter and "Tasty!" labels |
| Unity Test Framework | EditMode tests |

## Architecture

```
Assets/Scripts
+-- ZooLifetimeScope.cs          (composition root)
+-- Animals
|   +-- Animal.cs                (runtime animal: heading, screen turn, wall reflection, diet check)
|   +-- AnimalConfig.cs          (prefab + role + food group + movement + behaviour)
|   +-- AnimalRole.cs            (prey or predator, counter title)
|   +-- FoodGroup.cs             (diet: which groups it eats)
|   +-- AnimalView.cs            (body, animator, collision events, appear, eat, be eaten, bounce)
|   +-- AnimalFactory.cs         (pools per config)
|   +-- AnimalsController.cs     (registry, fixed tick, collision routing)
|   +-- FoodChain.cs             (who eats whom)
|   +-- AnimalEaten.cs           (message)
|   +-- Behaviours
|   |   +-- IAnimalBehaviour.cs
|   |   +-- BehaviourConfig.cs   (abstract factory asset)
|   |   +-- WanderBehaviour(Config).cs
|   +-- Movement
|       +-- IAnimalMovement.cs
|       +-- MovementConfig.cs    (abstract factory asset)
|       +-- JumpArc.cs           (projectile math)
|       +-- JumpMovement(Config).cs
|       +-- CrawlMovement(Config).cs
+-- Spawning
|   +-- SpawnConfig.cs
|   +-- SpawnController.cs       (Awaitable loop, waits for a free spot)
+-- World
|   +-- ArenaBounds.cs
+-- UI
|   +-- DeathCounterController.cs / DeathCounterView.cs / DeathCounterRowView.cs
|   +-- TastyLabelsController.cs / TastyLabelView.cs
+-- Utilities
    +-- IsExternalInit.cs        (enables records on Unity's runtime)

Assets/Tests/EditMode
+-- AnimalTests.cs
+-- FoodChainTests.cs
+-- JumpArcTests.cs
```

Runtime flow:

1. `ZooLifetimeScope` registers configs, services and entry points.
2. `SpawnController` waits a random delay and spawns a random `AnimalConfig` at the first free point of `ArenaBounds`.
3. `AnimalsController` ticks every animal in `FixedTick`; the behaviour steers the heading, the animal turns back when heading out of `ArenaBounds`, and the movement drives the body.
4. `AnimalView` reports collisions. A wall hit reflects the heading, an animal hit asks `FoodChain` for the eater; the eater plays its attack, the food is pulled in and returned to the pool, and `AnimalEaten` is published.
5. `DeathCounterController` and `TastyLabelsController` react to `AnimalEaten`.

## Adding An Animal

1. Create a prefab with `AnimalView`, a `Rigidbody`, a collider and an `Animator` on the `Animals` layer. The animator needs an `Eat` trigger; jumpers also use `Jump` and `FlightSpeed`.
2. Create an `AnimalConfig` (Create > TastyZoo > Animal) with the prefab, a role, a food group, a movement and a behaviour asset.
3. Add the config to `Assets/Configs/SpawnConfig.asset`.

A new diet relation is a `FoodGroup` asset (a spider group that eats flies and is eaten by birds), while the counter keeps showing prey and predators through `AnimalRole`. A new way of moving is a `MovementConfig` subclass plus an `IAnimalMovement` that drives the rigidbody and animator. A new way of choosing a heading (hunting, schooling, ambush) is a `BehaviourConfig` subclass plus an `IAnimalBehaviour`. No existing code changes in any of these cases.

## Design Decisions

- **Role and food group are separate.** The spec counts prey and predators, while diets need finer groups for 1000 species. `AnimalRole` answers "which counter row", `FoodGroup` answers "who eats whom".
- **Planar space.** Headings live on the ground plane and the arena is the screen projected on it. Animals that need their own space (fish in water, birds in the air) would add a habitat to `AnimalConfig` and `ArenaBounds`; nothing needs it yet, so it is not built.
- **Jump timing contract.** `JumpMovementConfig.Windup` equals the `Frog_JumpStart` clip length, and the `JumpAir` state speed equals the `Frog_JumpAir` clip length, so `FlightSpeed = 1 / flightTime` stretches the flight clip to the physical flight.
- **Elder predator wins.** `FoodChain` compares `SpawnOrder`, which is deterministic and needs no extra state.

## Assets

Frog and snake by Quaternius, nature props and UI panel by Kenney (CC0), Lilita One font (SIL OFL). See `Assets/Art/CREDITS.md`.

## Running

1. Open `Assets/Scenes/MainScene.unity`.
2. Enter Play Mode in Unity 6 with a landscape Game view.
