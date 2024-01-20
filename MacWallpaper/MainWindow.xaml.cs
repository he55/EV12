using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TinyJson;

namespace MacWallpaper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        EmojiAsset _lastSelectedItem;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<EmojiAsset> assets = LoadData(@"C:\Users\admin\Documents\GitHub\fluentui-emoji\assets");
            List<EmojiCategory> categories = assets.GroupBy(x => x.emoji.group)
                .Select(x => new EmojiCategory { title = x.Key, assets = x.ToList() })
                .ToList();
            listBox.ItemsSource = categories;
        }

        static List<EmojiAsset> LoadData(string path)
        {
            List<EmojiAsset> assets = new List<EmojiAsset>();
            string[] dirs = Directory.GetDirectories(path);
            foreach (var dir in dirs)
            {
                string imageDir = Path.Combine(dir, "3D");
                if (!Directory.Exists(imageDir))
                    imageDir = Path.Combine(dir, @"Default\3D");

                var files = Directory.GetFiles(imageDir, "*.png");
                if (files.Length == 0)
                    continue;

                string filePath = Path.Combine(dir, "metadata.json");
                string json = File.ReadAllText(filePath);
                Emoji2 emoji = JSONParser.FromJson<Emoji2>(json);

                EmojiAsset asset = new EmojiAsset();
                asset.emoji = emoji;
                asset.id = dir;
                asset.previewImage = files[0];
                asset.name = Path.GetFileName(dir);
                assets.Add(asset);
            }

            return assets;
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gridView.ItemsSource = ((EmojiCategory)listBox.SelectedItem).assets;
            if (gridView.Items.Count > 0)
                gridView.ScrollIntoView(gridView.Items[0]);
        }

        private void gridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EmojiAsset selectedItem = (EmojiAsset)gridView.SelectedItem;
            if (selectedItem != null)
            {
                if (_lastSelectedItem != null)
                    _lastSelectedItem.isSelected = false;

                _lastSelectedItem = selectedItem;
                _lastSelectedItem.isSelected = true;
                myHeaderControl.DataContext = _lastSelectedItem;
                listBox2.SelectedIndex = 0;
                listBox2.Visibility = _lastSelectedItem.items.Count > 1 ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AssetItem selectedItem = (AssetItem)listBox2.SelectedItem;
            if (selectedItem != null)
            {
                Process.Start("explorer.exe", $"/select, \"{selectedItem.subitems[0].source}\"");
            }
        }
    }
}
