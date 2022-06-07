using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                    Debug.WriteLine(p.ProcessName);
                }
            }
            return processwithwindow;

        }
    }
}