using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MacWallpaper
{
    public class EmojiCategory
    {
        public string title { get; set; }
        public List<EmojiAsset> assets { get; set; }
    }

    public class EmojiAsset : INotifyPropertyChanged
    {
        private bool _isSelected;
        private List<EmojiAsset2> _assets;

        public Emoji2 emoji { get; set; }
        public List<EmojiAsset2> assets
        {
            get
            {
                if (_assets == null)
                    _assets = EmojiAssetHelper.MakeAssets(id);
                return _assets;
            }
        }
        public string id { get; set; }
        public string name { get; set; }
        public string previewImage { get; set; }

        public string filepath { get; set; }
        public bool isSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class AssetItem2
    {
        public string name { get; set; }
        public string source { get; set; }
    }

    public class EmojiAsset2
    {
        public string name { get; set; }
        public List<AssetItem2> items { get; set; }
    }

    public class EmojiAssetHelper
    {
        public static List<EmojiAsset2> MakeAssets(string path)
        {
            List<EmojiAsset2> assets = new List<EmojiAsset2>();
            string v1 = Path.Combine(path, "3D");
            if (Directory.Exists(v1))
            {
                assets.Add(new EmojiAsset2
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
                assets.Add(new EmojiAsset2
                {
                    name = v,
                    items = MakeItems(item)
                });
            }
            return assets;
        }

        static List<AssetItem2> MakeItems(string path)
        {
            List<AssetItem2> items = new List<AssetItem2>();
            string[] dirs = Directory.GetDirectories(path);
            foreach (var item in dirs)
            {
                var files = Directory.GetFiles(item);
                string v1 = files.FirstOrDefault(x => x.EndsWith(".png", StringComparison.OrdinalIgnoreCase) || x.EndsWith(".svg", StringComparison.OrdinalIgnoreCase));
                if (v1 == null)
                    continue;

                string v = Path.GetFileName(item);
                items.Add(new AssetItem2
                {
                    name = v,
                    source = v1,
                });
            }
            return items;
        }
    }
}
