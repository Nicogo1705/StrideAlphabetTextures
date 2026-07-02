using System;
using System.Collections.Generic;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Graphics;
using Stride.Graphics.GeometricPrimitives;
using Stride.Extensions;
using Stride.Rendering;
using Stride.Rendering.Materials;
using Stride.Rendering.Materials.ComputeColors;

namespace Demo
{
    /// <summary>
    /// Loads every texture from the StrideAlphabetTextures pack (A..Z) and lays them out on a
    /// grid of textured quads that slowly rotate around the Y axis.
    ///
    /// TEXTURE URL SCHEME (verify here first if a letter fails to appear):
    ///   The library package (AssetData/StrideAlphabetTextures/StrideAlphabetTextures.sdpkg) declares
    ///   its asset folder as "Assets" and each glyph lives directly at that root as "&lt;Letter&gt;.sdtex"
    ///   (e.g. Assets/A.sdtex). A Stride Content URL is the asset path relative to the package
    ///   asset-folder root, WITHOUT extension, so the URL for letter A is simply "A", B is "B", etc.
    ///   Hence the URL list below is just the single characters "A".."Z".
    ///   If a texture does not show up, dump Content-visible URLs and confirm this mapping.
    ///
    /// QUAD API CHOICE:
    ///   We build the quad with Stride.Graphics.GeometricPrimitives.GeometricPrimitive.Plane.New,
    ///   which (with the default NormalDirection.UpZ) produces a plane in the XY plane facing +Z,
    ///   i.e. a vertical billboard-style quad facing a camera sitting on +Z. generateBackFace:true
    ///   keeps it visible after it rotates past 90 degrees. The primitive is turned into a Mesh via
    ///   ToMeshDraw() and wrapped in a runtime Model + textured Material. No scene GUID wiring.
    /// </summary>
    public class TextureGridShowcase : SyncScript
    {
        // Number of columns in the grid.
        public int Columns = 7;

        // Distance between quad centers (quads are 1x1 units).
        public float Spacing = 1.4f;

        // Radians per second the quads spin around Y.
        public float RotationSpeed = 0.5f;

        private readonly List<Entity> quads = new List<Entity>();

        // Keep the geometric primitives alive: their GPU vertex/index buffers back the MeshDraws.
        private readonly List<GeometricPrimitive> primitives = new List<GeometricPrimitive>();

        public override void Start()
        {
            // STEP 1: build the A..Z URL list (see class comment for the scheme).
            var urls = new List<string>();
            for (char c = 'A'; c <= 'Z'; c++)
                urls.Add(c.ToString());

            // Pass 1: load every texture (skip + warn on failure so one bad URL cannot kill the demo).
            var loaded = new List<Texture>();
            foreach (var url in urls)
            {
                try
                {
                    var tex = Content.Load<Texture>(url);
                    if (tex != null)
                        loaded.Add(tex);
                    else
                        Log.Warning($"TextureGridShowcase: Content.Load<Texture>(\"{url}\") returned null; skipping.");
                }
                catch (Exception ex)
                {
                    Log.Warning($"TextureGridShowcase: failed to load texture URL \"{url}\": {ex.Message}");
                }
            }

            if (loaded.Count == 0)
            {
                Log.Warning("TextureGridShowcase: no textures loaded. Check the URL scheme documented in this file.");
                return;
            }

            // Pass 2: place a rotating quad per loaded texture, centering the grid on the origin.
            int rows = (loaded.Count + Columns - 1) / Columns;
            float xOffset = (Columns - 1) * Spacing / 2f;
            float yOffset = (rows - 1) * Spacing / 2f;

            for (int i = 0; i < loaded.Count; i++)
            {
                int col = i % Columns;
                int row = i / Columns;

                var model = CreateTexturedQuad(loaded[i]);

                var entity = new Entity($"Quad_{i}")
                {
                    new ModelComponent { Model = model },
                };

                entity.Transform.Position = new Vector3(
                    col * Spacing - xOffset,
                    yOffset - row * Spacing,
                    0f);

                // Add under the Showcase entity so no scene GUID wiring is needed.
                Entity.AddChild(entity);
                quads.Add(entity);
            }
        }

        private Model CreateTexturedQuad(Texture texture)
        {
            var material = Material.New(GraphicsDevice, new MaterialDescriptor
            {
                Attributes =
                {
                    Diffuse = new MaterialDiffuseMapFeature(new ComputeTextureColor { Texture = texture }),
                    DiffuseModel = new MaterialDiffuseLambertModelFeature(),
                },
            });

            var primitive = GeometricPrimitive.Plane.New(GraphicsDevice, 1f, 1f, generateBackFace: true);
            primitives.Add(primitive);

            var mesh = new Mesh
            {
                Draw = primitive.ToMeshDraw(),
                MaterialIndex = 0,
            };

            var model = new Model();
            model.Materials.Add(material);
            model.Meshes.Add(mesh);
            return model;
        }

        public override void Update()
        {
            float dt = (float)Game.UpdateTime.Elapsed.TotalSeconds;
            var spin = Quaternion.RotationY(RotationSpeed * dt);
            foreach (var e in quads)
                e.Transform.Rotation *= spin;
        }
    }
}
