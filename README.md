Part 2: Object-Oriented Programming
For OOP, I wanted one shared foundation for everything that walks and jumps in the game, so I built a Character base class. It handles movement, ground detection, jumping and the vertical screen wrap, which Bubble Bobble’s characters all share. PlayerController and Enemy both inherit from it, and EnemyWalker and EnemyJumper inherit from Enemy. I did this to avoid copying the same physics code into every character.

Inheritance: Character -> Enemy -> EnemyWalker / EnemyJUmper, and Character -> PlayerController

Abstraction: Character and Enemy are abstract, so you can’t spawn one directly. Enemy also declares an abstract Think() method, which forces each enemy type to define its own decision-making. I also made an ITrappable interface (Trap, Release, Defeat, IsTrapped) so a bubble only needs to know that something can be trapped and doesn’t need to know what the enemy is.

Polymorphism: Enemy calls Think() every physics frame without knowing which enemy it is. The walker patrols and turns at walls, while the jumper chases the player and jumps. The bubble also works with any ITrappable, so a new enemy type would work with no changes to bubble code.

Encapsulation: Fields are private or protected and exposed to the Inspector with [SerializeField]. Properties like IsTrapped and IsGrounded have private setters, so only the owning class can change them. All the trapping logic (turning off physics, shrinking the enemy, parenting it to the bubble) stays inside Enemy.

Composition: I gave the player a separate BubbleShooter component instead of building shooting into the character classes. Shooting is something the player has, not something it is, and this also fits how Unity components work.

I used inheritance here because the characters share most of their behaviour and differ only in how they decide what to do, so adding a new enemy takes very little code.

Part 3: Singleton Pattern (GameSession)
When I looked at Bubble Bobble’s mechanics, I noticed that score, lives and the “all enemies defeated” win condition belong to the whole level, not to any single object. Enemies, bubbles, fruit and the player all need to report to the same place, so I made GameSession a Singleton.

It stores the score, lives, number of enemies alive, the chain-kill combo multiplier and whether the game is still being played. Other classes talk to it directly: enemies register themselves when they spawn and report when they’re defeated, bubbles and fruit add points, and the player loses lives through it. I implemented it with a static Instance set in Awake(). A duplicate destroys itself, and OnDestroy() clears the reference so a scene reload works cleanly.

Enemies and fruit spawn at runtime, so they can’t hold a scene reference to a score manager, and a global access point solves that. There also must be only one source of truth, because two sessions would mean two scores. I deliberately didn’t use DontDestroyOnLoad, since a session represents one level and should reset with it.

Current limitation: Because of the time limit, I haven’t finished setting up the UI for GameSession. The score and lives aren’t displayed on screen yet, so you can’t see the Singleton working in the game. The logic is written, and the text fields are ready to be linked. The restart with R doesn’t work yet either, because I haven’t added the scene to Build Settings.

Part 4: Factory Pattern (FruitFactory)
In Bubble Bobble, defeating an enemy drops a bonus fruit, and different fruits are worth different points. I wanted the enemy code to be able to say “drop a fruit” without knowing how fruits are built, so I made a FruitFactory.

FruitFactory is a static class with a FruitType enum (Orange, Banana, Grape). When an enemy is defeated, Enemy.Defeat() calls FruitFactory.Create(FruitFactory.RandomType(), position). The factory loads one Fruit prefab from the Resources folder, spawns it, then uses a switch to configure it with the right points, color and size (Orange 200, Banana 300, Grape 500). The caller never sees any of those details.

I used one prefab with different settings instead of three separate prefabs, since it was faster and kept the project simple. Adding a new fruit only takes a new enum value and a new case, and the enemy code doesn’t change.

Current limitation: The fruit drop isn’t working yet because I forgot to put the Fruit prefab inside Assets/Resources, so the factory can’t find it. The factory code is complete, and the fix is only to move the prefab into that folder.
