# Alphabet Textures

A small Stride asset package providing **26 letter textures (A–Z)**, one
`Texture` asset per glyph. Handy for prototype UI, debug labels, signage,
decals, or learning how a texture-only Stride asset package is laid out.

## Contents

```
AssetData/
  AlphabetTextures.csproj   # Stride 4.4 package project (used for version detection)
  AlphabetTextures.sdpkg    # Stride package: Assets/ + Resources/
  Assets/                   # A.sdtex ... Z.sdtex  (one Texture asset per letter)
  Resources/                # A.png   ... Z.png    (the source images, 512x512)
  thumbnail.png             # store thumbnail
```

Each `Assets/<Letter>.sdtex` is a standard Stride `Texture` asset whose
`Source` points at `../Resources/<Letter>.png`.

## Usage

1. Install via the Stride Asset Store (clones the repo and wires a
   `ProjectReference` into your game), or simply copy `AssetData/Assets` and
   `AssetData/Resources` into your own Stride project.
2. Open the project in Game Studio — the 26 textures appear under the
   `Assets` folder, ready to drop onto sprites, UI images, or materials.

## License

MIT. See the store entry for details.
