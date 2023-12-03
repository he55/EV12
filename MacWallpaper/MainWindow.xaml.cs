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

                string v1 = Directory.GetFiles(v)[0];
                string v2 = System.IO.Path.Combine(item, "metadata.json");
                string v3 = File.ReadAllText(v2);
                Emoji emoji = JSONParser.FromJson<Emoji>(v3);

                Ass ass = new Ass();
                ass.emoji = emoji;
                ass.id = v1;
                ass.previewImage = v1;
                ass.str1=System.IO.Path.GetFileName(item);
                asses.Add(ass);
            }

            List<Cate> cates = asses.GroupBy(x=>x.emoji.group).Select(x=>new Cate { str1=x.Key, assets=x.ToList() }).ToList();
            listBox.ItemsSource = cates;
            listBox.SelectedIndex = 0;
            gridView.SelectedIndex = 0;
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
            }
        }
    }
    public class Cate
    {
        public string str1 { get; set; }
        public string des { get; set; }
        public List<Ass> assets { get; set; }
    }
    public class Ass:INotifyPropertyChanged
    {
        private bool isSelected1;

        public Emoji emoji { get; set; }
        public string id { get; set; }
        public string str1 { get; set; }
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
