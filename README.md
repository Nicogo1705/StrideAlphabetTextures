# Alphabet Textures

**26 ready-to-use letter textures (A–Z)** for [Stride](https://www.stride3d.net/) — one `Texture`
asset per glyph (512×512). Handy for prototype UI, debug labels, signage, or decals.

## What's in the box

| File | Role |
|------|------|
| `Assets/A.sdtex` … `Z.sdtex` | 26 Stride `Texture` assets, one per letter, sourced from `Resources/<Letter>.png` (512×512). |

Every texture is a package **root asset**, so it is always compiled and loadable by URL from any
project that references the pack.

## Quick start

Reference the pack, then drop a texture onto a sprite, UI image or material in Game Studio — or load
one in code by its name:

```csharp
var a = Content.Load<Texture>("A");   // "A" … "Z"
```

## Demo

Open `StrideAlphabetTextures.sln`, set **Demo.Windows** as the startup project and run — every
letter is shown on a grid of textured quads.

## License

MIT. See [LICENSE.md](LICENSE.md).
