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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using to_do_list_WinUI3;
using Windows.Foundation;
using Windows.Foundation.Collections;

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
        ObservableCollection<TaskTodo> SubTasks = new ObservableCollection<TaskTodo>();
        public Task_screen()
        {
            this.InitializeComponent();
            
            
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

        private void Play_task_Click(object sender, RoutedEventArgs e)
        {
            var f_window = new Focus_Task();
            f_window.Activate();
            this.Close();
        }
    }
}
