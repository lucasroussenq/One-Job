# ONE JOB repository rules

## Product and scope

- Unity `6000.3` LTS, C#, URP, Windows x64, keyboard and mouse first.
- Cooperative host/client game for 2-4 players. Treat authoritative state and ownership as multiplayer concerns even before networking is installed.
- Current goal is gameplay validation with primitives. Do not add final art, Steamworks, progression, cosmetics, matchmaking, replay, voice chat, or character customization.

## Code

- Keep components small and single-purpose. Prefer direct, readable composition over frameworks or service locators.
- Player code may discover `IInteractable`; it must never branch on concrete interactable types.
- Interactive state belongs to the interactable or scenario system that owns it. Expose explicit state-setting methods so a future network authority can apply replicated state.
- Keep input isolated from movement, camera, and interaction. Remote players must later be able to disable local input components.
- Put runtime code under `Assets/Scripts/<Area>` and use namespace `OneJob.<Area>`.
- Inspector-configurable gameplay values must not be unexplained literals in behavior code.
- Character appearance must remain separate from gameplay collider and movement values.

## Assets and scenes

- Commit `.meta` files. Never edit or commit `Library/`, `Temp/`, `Logs/`, `obj/`, or generated IDE files.
- Use primitives and simple materials until gameplay is proven.
- Warn before adding large binary files. Git LFS is required for large models, textures, audio, and similar assets.

## Validation

- After C# changes, run Unity batch-mode compilation/tests when an editor is available.
- If Unity is unavailable, report that limitation; never claim compilation succeeded.
- Check Console errors, missing scripts, scene references, and relevant warnings before calling a milestone complete.
- Keep commits small and logical. Never push without explicit authorization.
