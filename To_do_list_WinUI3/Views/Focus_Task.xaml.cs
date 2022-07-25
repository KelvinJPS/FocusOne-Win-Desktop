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
using To_do_list_WinUI3.Class;
using Windows.UI.Core;

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

        private void Start_Click(object sender, RoutedEventArgs e)
        {

            // Get the minutes and hours 
            TimeSpan time = new TimeSpan(int.Parse(Hours_Combobox.Text), int.Parse(Minutes_combobox.Text), 0);
                  
           //Update the variable in app.cs BlockTime to be able to share data between pages 
            (App.Current as App).blockTime = time;

            //Update the variable in app.cs Useful apps to be able to share data between pages 
            (App.Current as App).UsefulApps = UsefulApps;

            //Activate the new window  task screen 
            var f_window = new Task_screen();
            f_window.Activate();
            this.Close(); ; //close the window 

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