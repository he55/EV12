using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MacWallpaper
{
    public class Cate
    {
        public string title { get; set; }
        public List<Ass> assets { get; set; }
    }

    public class Ass : INotifyPropertyChanged
    {
        private bool isSelected1;

        public Emoji emoji { get; set; }
        public List<EmojiAsset> assets => AssetHelper.MakeAssets(id);
        public string id { get; set; }
        public string name { get; set; }
        public string previewImage { get; set; }

        public string filepath { get; set; }
        public bool isSelected
        {
            get => isSelected1;
            set
            {
                isSelected1 = value;
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
