using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.System;
using Windows.System.Diagnostics;
using To_do_list_WinUI3.Views;
using to_do_list_WinUI3;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace To_do_list_WinUI3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Focus_Task : Window
    {
        List<Process> UsefulApps = new List<Process>();
        TaskTodo TaskSelected = (App.Current as App).TaskSelected;
        public Focus_Task()
        {
            this.InitializeComponent();
            ListView_Apps.ItemsSource = GetProcessesWithWindow();
        }

        private List<Process> GetProcessesWithWindow()
        {
            List<Process> processwithwindow = new List<Process>();

            Process[] processes = Process.GetProcesses();

            foreach (Process p in processes)
            {
                if (!String.IsNullOrEmpty(p.MainWindowTitle))
                {
                    processwithwindow.Add(p);

                }
            }
            return processwithwindow;

        }

        private async void Start_Click(object sender, RoutedEventArgs e)
        {
           await BlockApps();
           var f_window = new Task_screen();
           f_window.Activate();
          
           this.Close(); ;
        }

        private async Task  BlockApps()
        {
            int hours;
            int minutes;
            int M = 0;

            int.TryParse(Hours_Combobox.Text, out hours);
            int.TryParse(Minutes_combobox.Text, out minutes);
            List<Process> processwithwindow = new List<Process>();

            Task timer = new Task(() =>
            {
                for (M = 0; M <= minutes; M++)
                {
                    Thread.Sleep(minutes * 10000);

                }

            });

            Task Blocker = new Task(() =>
            {
                while (M != minutes)
                {
                    processwithwindow = GetProcessesWithWindow();

                    foreach (Process p in processwithwindow)
                    {

                        if (UsefulApps.Any(program => program.Id == p.Id) == false)
                        {
                            p.Kill();
                        }

                    }
                };
            }
                );

            timer.Start();
            Blocker.Start();
        }
        private void ListView_Apps_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UsefulApps.Clear();
            foreach (Process p in ListView_Apps.SelectedItems)
            {
                UsefulApps.Add(p);
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
            => ListView_Apps.ItemsSource = GetProcessesWithWindow();

        private void return_button_Click(object sender, RoutedEventArgs e)
        {
            var m_window = new MainWindow();
            m_window.Activate();
            this.Close();
        }


    }
}