# Galagan

A simple Galaga-style Unity game for my CS 4423 Midterm.

- Procedural asteroids with random shapes, rendered with `LineRenderer` and collisions provided by `PolygonCollider2D`.
- Simple sound effects [BeepBox][beepbox]
- Automatic builds created by [GameCI][gameci]
- Online Demo available with [GitHub Pages][demo]

### Problems

- Projects can spawn on top of eachother, potentially causing problems.
- WebGL Build doesn't have working audio, due to how it's loaded.
- Untested on Windows/Linux

[demo]: https://xevion.github.io/Galagan/
[beepbox]: https://www.beepbox.co
[gameci]: https://game.ci/docs/getting-started/