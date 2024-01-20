using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacWallpaper
{
    public class AssetItem
    {
        public string name { get; set; }
        public string source { get; set; }
    }

    public class EmojiAsset
    {
        public string name { get; set; }
        public List<AssetItem> items { get; set; }
    }

    public class AssetHelper
    {
        public static List<EmojiAsset> MakeAssets(string path)
        {
            List<EmojiAsset> assets = new List<EmojiAsset>();
            string v1 = Path.Combine(path, "3D");
            if (Directory.Exists(v1))
            {
                assets.Add(new EmojiAsset
                {
                    name = "Default",
                    items = MakeItems(path)
                });
                return assets;
            }

            string[] dirs = Directory.GetDirectories(path);
            foreach (var item in dirs)
            {
                string v = Path.GetFileName(item);
                assets.Add(new EmojiAsset
                {
                    name = v,
                    items = MakeItems(item)
                });
            }
            return assets;
        }

        static List<AssetItem> MakeItems(string path)
        {
            List<AssetItem> items = new List<AssetItem>();
            string[] dirs = Directory.GetDirectories(path);
            foreach (var item in dirs)
            {
                var files = Directory.GetFiles(item);
                string v1 = files.FirstOrDefault(x => x.EndsWith(".png", StringComparison.OrdinalIgnoreCase) || x.EndsWith(".svg", StringComparison.OrdinalIgnoreCase));
                if (v1 == null)
                    continue;

                string v = Path.GetFileName(item);
                items.Add(new AssetItem
                {
                    name = v,
                    source = v1,
                });
            }
            return items;
        }
    }
}
