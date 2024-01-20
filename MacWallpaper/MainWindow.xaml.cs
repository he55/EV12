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
        Ass _lastSelectedItem;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<Ass> asses = LoadData(@"C:\Users\admin\Documents\GitHub\fluentui-emoji\assets");

            List<Cate> cates = asses.GroupBy(x => x.emoji.group).Select(x => new Cate { title = x.Key, assets = x.ToList() }).ToList();
            listBox.ItemsSource = cates;
        }

        static List<Ass> LoadData(string dir)
        {
            List<Ass> asses = new List<Ass>();
            string[] dirs = Directory.GetDirectories(dir);
            foreach (var item in dirs)
            {
                string v = System.IO.Path.Combine(item, "3D");
                if (!Directory.Exists(v))
                    v = System.IO.Path.Combine(item, "Default", "3D");

                var files = Directory.GetFiles(v, "*.png");
                if (files.Length == 0)
                    continue;

                string v2 = System.IO.Path.Combine(item, "metadata.json");
                string v3 = File.ReadAllText(v2);
                Emoji emoji = JSONParser.FromJson<Emoji>(v3);

                Ass ass = new Ass();
                ass.emoji = emoji;
                ass.id = item;
                ass.previewImage = files[0];
                ass.name = System.IO.Path.GetFileName(item);
                asses.Add(ass);
            }

            return asses;
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
                if (_lastSelectedItem != null)
                    _lastSelectedItem.isSelected = false;

                _lastSelectedItem = selectedItem;
                _lastSelectedItem.isSelected = true;
                myHeaderControl.DataContext = _lastSelectedItem;
                listBox2.SelectedIndex = 0;
                listBox2.Visibility = _lastSelectedItem.assets.Count > 1 ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EmojiAsset selectedItem = (EmojiAsset)listBox2.SelectedItem;
            if (selectedItem != null)
            {
                Process.Start("explorer.exe", $"/select, \"{selectedItem.items[0].source}\"");
            }
        }
    }
}
