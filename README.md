# 3rd Person Combat

From the course ["Unity 3rd Person: Combat & Traversal" by Nathan Farrer](https://www.gamedev.tv/courses/unity-3rd-person-combat). I also experimented with my own procedural level generation implementation alongside the course material.

## Key Learning Objectives

- **Building from Scratch:** Constructed a functional third-person action combat system from the ground up. 
- **Combat Mechanics:** Gained experience in techniques such as handling player inputs, managing animation state machines, and designing attack combos.
- **Agility and Precision:** Learned to implement dodging mechanics and target locking.
- **Cinematic Feel:** Added dynamic camera movements with Cinemachine.
- **Enemy Encounters:** Designed enemy AI.
- **Flexible Controls:** Configured the game to work with both mouse/keyboard and game controllers.

## Procedural Generation Features

- **Configurable Levels:** Properties of the level, number of rooms, room placement decisions, and kinds of rooms used are configurable from the inspector.
- **Room Pool:** Rooms are randomly picked to be placed on a grid according to a chosen list of hand-made rooms.
- **Gated Progression:** Progression granting rooms (with an key) and progression gated rooms (with a locked door) are intelligently distributed throughout the level.
