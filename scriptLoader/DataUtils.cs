using System.IO;
using UnityEngine;

namespace scriptLoader
{
    public static class DataUtils
    {
        public static void SaveTex2D(Texture2D tex, string name, TextureFormat fmt = TextureFormat.ARGB32)
        {
            var newTexture = new Texture2D(tex.width, tex.height, fmt, false);
            newTexture.SetPixels(0, 0, tex.width, tex.height, tex.GetPixels());
            newTexture.Apply();
            var bytes = newTexture.EncodeToPNG();
            File.WriteAllBytes(name, bytes);
        }
    }
}
