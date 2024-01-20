﻿using System.Collections.Generic;
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
            List<EmojiAsset> asses = LoadData(@"C:\Users\admin\Documents\GitHub\fluentui-emoji\assets");

            List<EmojiCategory> cates = asses.GroupBy(x => x.emoji.group).Select(x => new EmojiCategory { title = x.Key, assets = x.ToList() }).ToList();
            listBox.ItemsSource = cates;
        }

        static List<EmojiAsset> LoadData(string dir)
        {
            List<EmojiAsset> asses = new List<EmojiAsset>();
            string[] dirs = Directory.GetDirectories(dir);
            foreach (var item in dirs)
            {
                string v = Path.Combine(item, "3D");
                if (!Directory.Exists(v))
                    v = Path.Combine(item, "Default", "3D");

                var files = Directory.GetFiles(v, "*.png");
                if (files.Length == 0)
                    continue;

                string v2 = Path.Combine(item, "metadata.json");
                string v3 = File.ReadAllText(v2);
                Emoji2 emoji = JSONParser.FromJson<Emoji2>(v3);

                EmojiAsset ass = new EmojiAsset();
                ass.emoji = emoji;
                ass.id = item;
                ass.previewImage = files[0];
                ass.name = Path.GetFileName(item);
                asses.Add(ass);
            }

            return asses;
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
