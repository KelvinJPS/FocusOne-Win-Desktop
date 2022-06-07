using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.System;
using Windows.System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace To_do_list_WinUI3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Focus_Task : Page
    {
        List<Process> UsefulApps = new List<Process>();
        public Focus_Task()
        {
            this.InitializeComponent();
            ListBox_Apps.ItemsSource = GetProcessesWithWindow();
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

        private void Add_app_button_Click(object sender, RoutedEventArgs e)
        {
            if (ListBox_Apps.SelectedItem != null)
            {
                UsefulApps.Add((ListBox_Apps.SelectedItem as Process));
            }
            
        }

        private async void Start_Click(object sender, RoutedEventArgs e)
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

        private void Delete_apps_button_Click(object sender, RoutedEventArgs e)
        {
            if (ListBox_Apps.SelectedItem != null)
            {
                UsefulApps.Remove((ListBox_Apps.SelectedItem as Process));
            }
        }
    }
}