using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Xps;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace pr2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Main1();
            InitializeComponent();
            ExampleDtp.SelectedDate = DateTime.Now;
            ExampleDtp.DisplayDateStart = new DateTime(2023, 01, 01);
            ExampleDtp.DisplayDateEnd = new DateTime(2023, 12, 31);
        }
        public void Main1()
        {
            Deserialize();
        }
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            vars.count++;
            box2.Text = vars.count.ToString();
            if (vars.count >= 5)
            {
                /*box1.Text = vars.count.ToString();*/
                allnames.ItemsSource = null;
                vars.listday.Add(vars.listname.ToList());
                vars.listday.Add(vars.listdesc.ToList());
                vars.listname.Clear();
                vars.listdesc.Clear();
                foreach (var dz in vars.listdate)
                {
                    if (dz == Convert.ToDateTime(ExampleDtp.Text))
                    {
                        allnames.ItemsSource = null;
                        box2.Text = (vars.listdate.IndexOf(dz) * 2).ToString();

                        allnames.ItemsSource = vars.listday[vars.listdate.IndexOf(dz) * 2];
                        
                        vars.listname = vars.listday[vars.listdate.IndexOf(dz) * 2 ];
                        vars.listdesc = vars.listday[vars.listdate.IndexOf(dz) * 2 + 1];

                        allnames.ItemsSource = null;
                        allnames.ItemsSource = vars.listname;
                        /*allnames.Items.Add(vars.listname);*/
                        break;
                    }
                }
            }
            //DateTime date = Convert.ToDateTime(ExampleDtp.Text);
            //если вот date которая наверху соответствует дате в заметке, тогда вывыести ее в листбокс
        }

        private void Button_Create(object sender, RoutedEventArgs e)
        {
            vars.listname.Add(box.Text);
            vars.listdesc.Add(box1.Text);
            vars.date = Convert.ToDateTime(ExampleDtp.Text);
            vars.listdate.Add(Convert.ToDateTime( ExampleDtp.Text));
            allnames.ItemsSource = null;
            allnames.ItemsSource = vars.listname;
        }
        private void Button_Save(object sender, RoutedEventArgs e)
        {
            allnames.SelectedIndex = 0;
            vars.listname[allnames.SelectedIndex] = box.Text;
            vars.listdesc[allnames.SelectedIndex] = box1.Text;
            vars.date = Convert.ToDateTime(ExampleDtp.Text);
            allnames.ItemsSource = null;
            allnames.ItemsSource = vars.listname;
        }

        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            vars.listname.RemoveAt(allnames.SelectedIndex);
            vars.listdesc.RemoveAt(allnames.SelectedIndex);
            vars.date = ExampleDtp.SelectedDate.Value;
            allnames.ItemsSource = null;
            allnames.ItemsSource = vars.listname;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (vars.listname.Count != 0 && vars.listdesc.Count != 0)
            {
                /*box2.Text = Convert.ToString(allnames.SelectedIndex);*/
                if (allnames.SelectedIndex == -1)
                {
                    box.Text = vars.listname[0];
                    box1.Text = vars.listdesc[0];
                }
                else 
                {
                    box.Text = vars.listname[allnames.SelectedIndex];
                    box1.Text = vars.listdesc[allnames.SelectedIndex];
                }
            }
        }
        private void final_save(object sender, EventArgs e)
        {
            vars.path = "C:\\заметки\\" + Convert.ToString(vars.date);
            Serialize();

        }
        public void Serialize()
        {
            string json = JsonConvert.SerializeObject(vars.txt1);
            string a = string.Join("", json);
            File.WriteAllText(vars.path, a);
        }
        public static void Deserialize()
        {
            vars.txt = File.ReadAllText(vars.path);
            vars.txt1 = JsonConvert.DeserializeObject<List<string>>(vars.txt);
            foreach (var f in vars.txt1)
            {
                string[] lines = File.ReadAllLines(vars.path);
                int a = 1;
                foreach (string line in lines)
                {
                    if (a == 1)
                    {
                        vars.listname.Add(line);
                    }
                    if (a == 2)
                    {
                        vars.listdesc.Add(line);
                    }
                    if (a == 3)
                    {
                        vars.date = Convert.ToDateTime(line);
                        a = 0;
                    }
                    a++;
                }

            }
        }
    }
}