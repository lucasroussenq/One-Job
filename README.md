# ONE JOB

Cooperative 3D party-game prototype. Each player gets one simple responsibility; coupled reactor systems turn small mistakes into escalating failures.

## Current prototype

- Unity 6.3 LTS project targeting URP.
- First-person keyboard/mouse movement.
- Generic raycast interaction through `IInteractable`.
- Prototype button, lever, valve, and door interactions.
- Runtime-built Reactor Room graybox using primitives.
- Configurable temperature, pressure, cooling, reactor danger states, and a three-minute round loop.
- Prototype reactor HUD with live values and success/failure result.

Jobs, Netcode for GameObjects, and Steam integration are not implemented yet.

## Open and test

1. Install Unity `6000.3.24f1` with Windows Build Support.
2. Open this directory as a Unity project and allow packages to resolve. First import creates and assigns default URP assets under `Assets/Settings`.
3. Open `Assets/Scenes/ReactorRoom.unity`.
4. Press Play.
5. Move with `WASD`, look with the mouse, and interact with `E`. Press `Esc` to release the cursor and click the Game view to capture it again.

Unity Editor is required to perform the first import, generate the package lock, and verify Console/build status.
