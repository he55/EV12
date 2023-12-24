using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TinyJson;

namespace MacWallpaper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Ass _lastSelectedItem;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<Ass> asses = new List<Ass>();
            string[] dirs = Directory.GetDirectories(@"C:\Users\admin\Documents\GitHub\fluentui-emoji\assets");
            foreach (var item in dirs)
            {
                string v = System.IO.Path.Combine(item, "3D");
                if(!Directory.Exists(v))
                 v = System.IO.Path.Combine(item,"Default","3D");

                var files=Directory.GetFiles(v,"*.png");
                if(files.Length==0)
                    continue;

                string v2 = System.IO.Path.Combine(item, "metadata.json");
                string v3 = File.ReadAllText(v2);
                Emoji emoji = JSONParser.FromJson<Emoji>(v3);

                Ass ass = new Ass();
                ass.emoji = emoji;
                ass.id = item;
                ass.previewImage = files[0];
                ass.name=System.IO.Path.GetFileName(item);
                asses.Add(ass);
            }

            List<Cate> cates = asses.GroupBy(x=>x.emoji.group).Select(x=>new Cate { title=x.Key, assets=x.ToList() }).ToList();
            listBox.ItemsSource = cates;
            listBox.SelectedIndex = 0;
            gridView.SelectedIndex = 0;
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gridView.ItemsSource = ((Cate)listBox.SelectedItem).assets;
            if (gridView.Items.Count > 0)
                gridView.ScrollIntoView(gridView.Items[0]);
        }

        private void gridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Ass selectedItem = (Ass)gridView.SelectedItem;
            if (selectedItem != null)
            {
                if(_lastSelectedItem != null)
                    _lastSelectedItem.isSelected = false;

                _lastSelectedItem = selectedItem;
                selectedItem.isSelected = true;
                myHeaderControl.DataContext = selectedItem;
                listBox2.SelectedIndex = 0;
            }
        }
    }
    public class Cate
    {
        public string title { get; set; }
        public List<Ass> assets { get; set; }
    }
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
            string v1 = System.IO.Path.Combine(path, "3D");
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
                string v = System.IO.Path.GetFileName(item);
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
                var files=Directory.GetFiles(item);
                string v1 = files.FirstOrDefault(x => x.EndsWith(".png", StringComparison.OrdinalIgnoreCase) || x.EndsWith(".svg", StringComparison.OrdinalIgnoreCase));
                if (v1 == null)
                    continue;

                string v = System.IO.Path.GetFileName(item);
                items.Add(new AssetItem
                {
                    name = v,
                    source = v1,
                });
            }
            return items;
        }
    }
    public class Ass:INotifyPropertyChanged
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

        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

}
