using System;
using UnityEditor;
using UnityEngine;

namespace Script.Asset
{
    public class TextureAutoImporter: AssetPostprocessor
    {
        public string path = "Assets/Sprite";
        private void OnPostprocessTexture(Texture2D texture)
        {
            if (assetImporter.assetPath.StartsWith(path))
            {
               var texImporter= assetImporter as TextureImporter;
               texImporter.textureType = TextureImporterType.Sprite;
               texImporter.spriteImportMode = SpriteImportMode.Single;
               texImporter.filterMode = FilterMode.Point;
               texImporter.textureCompression = TextureImporterCompression.Uncompressed;
            }
        }
    }
}