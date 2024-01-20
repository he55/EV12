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
        private List<AssetItem> _items;

        public Emoji2 emoji { get; set; }
        public List<AssetItem> items
        {
            get
            {
                if (_items == null)
                    _items = AssetItemHelper.MakeItems(id);
                return _items;
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

    public class AssetSubitem
    {
        public string name { get; set; }
        public string source { get; set; }
    }

    public class AssetItem
    {
        public string name { get; set; }
        public List<AssetSubitem> subitems { get; set; }
    }

    public class AssetItemHelper
    {
        public static List<AssetItem> MakeItems(string path)
        {
            List<AssetItem> assets = new List<AssetItem>();
            string v1 = Path.Combine(path, "3D");
            if (Directory.Exists(v1))
            {
                assets.Add(new AssetItem
                {
                    name = "Default",
                    subitems = MakeSubitems(path)
                });
                return assets;
            }

            string[] dirs = Directory.GetDirectories(path);
            foreach (var item in dirs)
            {
                string v = Path.GetFileName(item);
                assets.Add(new AssetItem
                {
                    name = v,
                    subitems = MakeSubitems(item)
                });
            }
            return assets;
        }

        static List<AssetSubitem> MakeSubitems(string path)
        {
            List<AssetSubitem> items = new List<AssetSubitem>();
            string[] dirs = Directory.GetDirectories(path);
            foreach (var item in dirs)
            {
                var files = Directory.GetFiles(item);
                string v1 = files.FirstOrDefault(x => x.EndsWith(".png", StringComparison.OrdinalIgnoreCase) || x.EndsWith(".svg", StringComparison.OrdinalIgnoreCase));
                if (v1 == null)
                    continue;

                string v = Path.GetFileName(item);
                items.Add(new AssetSubitem
                {
                    name = v,
                    source = v1,
                });
            }
            return items;
        }
    }
}
