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
}
