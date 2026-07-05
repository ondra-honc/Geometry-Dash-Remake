# Geometry Dash Remake

Geometry Dash Remake made completely from scratch, using a custom programmed engine, everything is made in C#

## Project Structure

```text
GeometryDash/
├── Program.cs
GeometryDash.Engine/
├── Core/
│   ├── Engine.cs          # Main engine loop
|   ├── GameSettings.cs    # Static game settings
|   └── TimeStep.cs        # Fixed time step accumulator
├── Entities/
│   ├── GameObject.cs      # Base game entity configuration
|   ├── ObjectPool.cs      # Entity recycling system using FIFO queues
│   └── PlayerCube.cs      # Jump physics and gravity handling
├── Physics/
│   └── CollisionEngine.cs  # AABB collision detection and state tracking
└──World/
    ├── LevelStreamer.cs # Used for streaming the level data into Engine.cs for rendering
    └── LevelManager.cs  # Loading level data with pretedermined structure
