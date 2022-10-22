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
using Windows.UI.Core;
using System.Text.RegularExpressions;

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
                  
           //Update the variable in app.cs BlockTime to be able to share data between pages 
            (App.Current as App).blockTime = GetBlockingTime(hours_textbox.Text,minutes_textbox.Text);

            //Update the variable in app.cs Useful apps to be able to share data between pages 
            (App.Current as App).UsefulApps = UsefulApps;

            //Activate the new window  task screen 
            var f_window = new Task_screen();
            f_window.Activate();
            this.Close(); ; //close the window 

         }
        private TimeSpan GetBlockingTime (string shours, string sminutes)
        {             
            int.TryParse(shours, out int hours);
            int.TryParse(sminutes, out int minutes);    
          
            return  new TimeSpan(hours, minutes, 0);
          
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

        private void Hours_Combobox_TextSubmitted(ComboBox sender, ComboBoxTextSubmittedEventArgs args)
        {
            Regex NumberRegex = new Regex(@"^\d+$");
            if (!NumberRegex.IsMatch(args.Text))
            {
                Debug.WriteLine("text");
                sender.Text = "00";
               
            }

        }
        private void TimerTextbox_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            // only numbers in textbox
            args.Cancel = args.NewText.Any(c => !char.IsNumber(c)); 

        }


    }
}