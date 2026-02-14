# GraphiteUi JS Interop Code Style

Mandatory rules for all files in `GraphiteUi/wwwroot/js`.

## Naming
- Use `camelCase` for functions, variables, and object members.
- Use `UPPER_SNAKE_CASE` for module-level constants.
- Prefer module `WeakMap<Element, State>` for element-owned interop state.
- Avoid attaching module state to custom DOM string properties.

## Exports
- Use verb-first names for exported interop APIs:
  - `refresh*` for scanning and setup
  - `setup*` for host/component setup
  - `destroy*` for explicit teardown APIs
- Prefer canonical camelCase exports.
- If an interop export is renamed, keep a compatibility alias when needed.

## Lifecycle and Idempotence
- Setup functions must be idempotent.
- JS modules own listener/timer/observer lifecycle.
- Repeated refresh/setup calls must not duplicate handlers.
- If teardown is needed, provide `destroy*` exports and keep cleanup deterministic.

## DOM and Attributes
- Guard null/invalid elements early and return fast.
- Keep selectors scoped to the current root when possible.
- Boolean data/aria attributes represented as strings must use explicit `'true'` / `'false'` handling.

## Globals and Side Effects
- Do not write to `window.*` unless explicitly required and documented.
- Avoid hidden cross-module mutable state.
- Keep module-level state minimal and intentional.

## Comments and Readability
- Add comments only when behavior is non-obvious.
- Prefer small helper functions over deeply nested inline logic.
- Keep event names/constants centralized (no magic literals spread through the module).

## Compatibility
- Public behavior must stay stable unless a change is intentional and documented.
- Preserve existing interop contracts during refactors unless callers are updated in the same change.
