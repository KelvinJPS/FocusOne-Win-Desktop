using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using to_do_list_WinUI3;
using To_do_list_WinUI3.Class;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Timer = System.Timers.Timer;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace To_do_list_WinUI3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Task_screen : Window
    {
        TaskTodo TaskSelected = (App.Current as App).TaskSelected;
        TimeSpan blockTime = (App.Current as App).blockTime;
        ObservableCollection<TaskTodo> SubTasks = new ObservableCollection<TaskTodo>();
        List<Process> UsefulApps = (App.Current as App).UsefulApps;
        DispatcherTimer dispatcherTimer;



        public Task_screen()
        {
            this.InitializeComponent();
            SubTasks = TaskSelected.GetSubtasks();
            SetDistapcherTimer();
            

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



        private void return_button_Click(object sender, RoutedEventArgs e)
        {
            var m_window = new MainWindow();
            m_window.Activate();
            this.Close();
        }

        private void Circle_Checked(object sender, RoutedEventArgs e)
        {
            var button = (RadioButton)sender;
            var item = (TaskTodo)button.DataContext;
            item.Done = "True";
            item.UpdateTask();

        }

        private void descriptions_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            TaskSelected.Description = textbox.Text;
            TaskSelected.UpdateTask();


        }

        private void Add_Subtask_textbox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                TaskTodo SubTask = new TaskTodo();
                SubTask.NameTask = Add_Subtask_textbox.Text;
                SubTask.ParentTask = TaskSelected.TaskId; //the id of the task selected
                SubTask.AddTask(SubTask);
                SubTasks.Add(SubTask);
                Add_Subtask_textbox.Text = String.Empty;

            }
        }

        private void BlockApps()
        {
            List<Process> processwithwindow = new List<Process>();                    
            processwithwindow = GetProcessesWithWindow();          
            foreach (Process p in processwithwindow)
                {
                    if (UsefulApps.Any(program => program.Id == p.Id) == false)
                    {
                        p.Kill();
                    }
                }           
        }

        

        private  void Window_Activated(object sender, WindowActivatedEventArgs args)
        {         
            startOrRestartDispatchTimer();
           
        }
        private void SetDistapcherTimer()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Start();
        }
        private void startOrRestartDispatchTimer()
        {
            dispatcherTimer.Stop(); // If already running  
            Countdown_TexBlock.Text = blockTime.ToString();
            dispatcherTimer.Start();
        }


        private void dispatcherTimer_Tick(object sender, object e)
        {

            if (blockTime != TimeSpan.Zero)
            {
                blockTime = blockTime.Subtract(TimeSpan.FromSeconds(1));
                Countdown_TexBlock.Text = blockTime.ToString();
                BlockApps();
           
            }
           
            else
            {
                dispatcherTimer.Stop();

            }

          
    
        }
      
    }

}



