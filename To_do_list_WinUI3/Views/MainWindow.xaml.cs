using Windows.UI.WindowManagement;
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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using to_do_list_WinUI3;
using To_do_list_WinUI3.Views;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Windowing;
using AppWindow = Microsoft.UI.Windowing.AppWindow;
using Microsoft.Toolkit.Uwp.Notifications;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace To_do_list_WinUI3
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        ObservableCollection<TaskTodo> TasksTodo = new ObservableCollection<TaskTodo>();
        ObservableCollection<TaskTodo> SubTasks = new ObservableCollection<TaskTodo>();
        ObservableCollection<string> TasksLists = new ObservableCollection<string>();
        TaskList Tasklist = new TaskList();
        TaskTodo task = new TaskTodo();
        Func<DateTimeOffset, string> SetDate = (date) => string.Format("{0}-{1}-{2}", date.Month, date.Day, date.Year);

        public MainWindow()
        {
            this.InitializeComponent();

            //Select the list by default
            ListView_defaultlists.SelectedItem = Today;
            //Get the list
            TasksLists = Tasklist.Getlists();
            ListView_tasklists.ItemsSource = TasksLists;
            Lists_listbox.ItemsSource = TasksLists;

            //Get the tasks 
            TasksTodo = task.GetTodayTasks();
            task_list.ItemsSource = TasksTodo;
            task_list.SelectedValuePath = "TaskId";

            //Get the subtasks          
            subtask_list.ItemsSource = SubTasks;
            number_repeat.MaxLength = 3;


           

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
        private string GetListSelected()
        {

            if (ListView_defaultlists.SelectedItem != null)
            {
                return "Tasks";


            }

            if (ListView_tasklists.SelectedItem != null)
            {
                return ListView_tasklists.SelectedItem.ToString(); ;
            }

            return string.Empty;
        }


        private void add_Task_textbox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {

                TaskTodo Task = new TaskTodo();

                Task.NameTask = add_Task_textbox.Text;
                Task.ListName = GetListSelected();

                //Add due date to today or tomorrow 
                if (Task.ListName == "Today")
                {
                    Task.Date = DateTimeOffset.Now;
                }

                else if (Task.ListName == "Tomorrow")
                {
                    Task.Date = DateTimeOffset.Now.AddDays(1);
                }

                //add task to de database and the observable collection
                Task.AddTask(Task);
                TasksTodo.Add(Task);

                //update the observable collection 
                TasksTodo = task.GetTasks(GetListSelected());
                task_list.ItemsSource = TasksTodo;
                //select the new task 
                task_list.SelectedItem = Task;
                //clean the texbox and focus
                add_Task_textbox.Text = String.Empty;
                add_Task_textbox.Focus(FocusState.Keyboard);

            }

        }
        private void add_Task_textbox_LostFocus(object sender, RoutedEventArgs e) => add_Task_textbox.Text = string.Empty;

        private void calendar_button(object sender, RoutedEventArgs e)
        {
            calendar_popup.IsOpen = true;
            if ((task_list.SelectedItem as TaskTodo).Date == default)
            {
                calendar_date.SelectedDates.Add(DateTimeOffset.Now);
            }
            else
                calendar_date.SelectedDates.Add((task_list.SelectedItem as TaskTodo).Date);
        }

        private void quit_TaskForm_Click(object sender, RoutedEventArgs e) => TaskForm.Visibility = Visibility.Collapsed;

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            calendar_popup.IsOpen = false;
            calendar_date.SelectedDates.Clear();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            calendar_popup.IsOpen = false;
            (task_list.SelectedItem as TaskTodo).Date = calendar_date.SelectedDates[0];
            (task_list.SelectedItem as TaskTodo).UpdateTask();
            calendar_date.SelectedDates.Clear();

        }
        private void reminder_cancel_Click(object sender, RoutedEventArgs e)
        {
            reminder_popup.IsOpen = false;
            reminder_calendar.SelectedDates.Clear();
            reminder_time_picker.SelectedTime = null;
        }


        private void reminder_save_Click(object sender, RoutedEventArgs e)
        {
            reminder_popup.IsOpen = false;
            DateTimeOffset Date = reminder_calendar.SelectedDates[0];

            //get the Time selected 
            if (reminder_time_picker.SelectedTime.HasValue == true)
            {
                var Time = reminder_time_picker.SelectedTime;
                Date = Date.Date.Add((TimeSpan)Time);     //Date D/M/Y + timer selected 
                
            }

            SheduleNotification(Date);
            (task_list.SelectedItem as TaskTodo).Reminder = Date;
            //clear values
            reminder_calendar.SelectedDates.Clear();
            reminder_time_picker.SelectedTime = null;
        }

        private void priority_checkbox_click(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            task.Priority = radioButton.Name;

        }

        private void reminder_button_Click(object sender, RoutedEventArgs e)
        {
            reminder_popup.IsOpen = true;
            reminder_calendar.SelectedDates.Add(DateTimeOffset.Now);

        }

        private void SheduleNotification(DateTimeOffset Date)
        {
            // sheduled notification 
            try
            {
                  ToastContentBuilder N = new ToastContentBuilder();
                  N.AddText(String.Format("you have to do {0}", task.NameTask));
                  N.AddText(String.Format("{0}", task.NameTask));
                  N.Schedule(Date.DateTime.AddSeconds(10));
            }
            catch (Exception ex) { ex.Message.ToString(); }

        }



        private void repeat_button_Click(object sender, RoutedEventArgs e)
        {
            if (repeat_options_popup.IsOpen == true)
                repeat_options_popup.IsOpen = false;

            else
                repeat_options_popup.IsOpen = true;

        }

        private void number_repeat_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            // only numbers in textbox
            args.Cancel = args.NewText.Any(c => !char.IsNumber(c));
        }

        private void cancel_repeat_Click(object sender, RoutedEventArgs e) => custom_repeat_popup.IsOpen = false;

        private void save_repeat_Click(object sender, RoutedEventArgs e)
        {
            custom_repeat_popup.IsOpen = false;


        }

        private void custom_repeat_combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox ComboBox = sender as ComboBox;
            ComboBoxItem ComboBoxSelected = ComboBox.SelectedItem as ComboBoxItem;

            if (ComboBoxSelected != null)
            {
                custom_month_calendar.Visibility = Visibility.Collapsed;
                week_calendar.Visibility = Visibility.Collapsed;

                switch (ComboBoxSelected.Name)
                {
                    case "MonthsComboBox":

                        custom_month_calendar.Visibility = Visibility.Visible;

                        break;

                    case "DaysComboBox":

                        break;

                    case "WeeksComboBox":

                        week_calendar.Visibility = Visibility.Visible;

                        break;
                }
            }
        }
        private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SubTask_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                TaskTodo SubTask = new TaskTodo();
                SubTask.NameTask = SubTask_TexBox.Text;
                SubTask.ParentTask = (task_list.SelectedItem as TaskTodo).TaskId; //the id of the task selected
                SubTask.AddTask(SubTask);
                SubTasks.Add(SubTask);
                SubTask_TexBox.Text = String.Empty;

            }
        }

        public bool CheckTaskRepeat(DateTimeOffset date) => date == DateTime.Today;


        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TextBlock TextBlockSelected = Repeat_options.SelectedItem as TextBlock;

            if (TextBlockSelected != null)
            {
                switch (TextBlockSelected.Name)
                {
                    case "Custom":

                        repeat_options_popup.IsOpen = false;
                        custom_repeat_popup.IsOpen = true;

                        break;

                    case "Daily":

                        task.NextRep = DateTime.Today.AddDays(1);

                        break;


                    case "Weekly":

                        task.NextRep = DateTime.Today.AddDays(7);

                        break;


                    case "Montly":

                        task.NextRep = DateTime.Today.AddMonths(1);


                        break;

                    case "Yearly":

                        task.NextRep = DateTime.Today.AddYears(1);


                        break;
                }
            }
        }

        private void list_name_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            if ((task_list.SelectedItem as TaskTodo) != null)
            {
                (task_list.SelectedItem as TaskTodo).ListName = textbox.Text;
                (task_list.SelectedItem as TaskTodo).UpdateTask();


            }

        }

        private void CheckPriority()
        {
            if ((task_list.SelectedItem as TaskTodo) != null)

            {
                low.IsChecked = false;
                medium.IsChecked = false;
                high.IsChecked = false;

                switch ((task_list.SelectedItem as TaskTodo).Priority)
                {
                    case "low":
                        low.IsChecked = true;
                        break;

                    case "medium":
                        medium.IsChecked = true;
                        break;

                    case "high":
                        high.IsChecked = true;
                        break;

                    default:
                        low.IsChecked = false;
                        medium.IsChecked = false;
                        high.IsChecked = false;

                        break;
                }

            }

        }

        private void descriptions_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            if ((task_list.SelectedItem as TaskTodo) != null)
            {
                (task_list.SelectedItem as TaskTodo).Description = textbox.Text;
                (task_list.SelectedItem as TaskTodo).UpdateTask();
            }

        }

        private void NameTaskForm_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((task_list.SelectedItem as TaskTodo) != null)
            {
                (task_list.SelectedItem as TaskTodo).NameTask = NameTaskForm.Text;
                (task_list.SelectedItem as TaskTodo).UpdateTask();
            }
        }


        private void Proritycheckbox_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton RadioB = sender as RadioButton;
            (task_list.SelectedItem as TaskTodo).Priority = RadioB.Name;
            (task_list.SelectedItem as TaskTodo).UpdateTask();
        }

        private void Add_list_texbox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                TasksLists.Add(Add_list_texbox.Text);

                Tasklist.AddList(Add_list_texbox.Text);

            }
        }

        private void task_list_ItemClick(object sender, ItemClickEventArgs e)
        {
            TaskForm.Visibility = Visibility.Visible;
        }


        private void ListView_tasklists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TasksTodo = task.GetTasks(GetListSelected());
            task_list.ItemsSource = TasksTodo;
            task_list.Header = GetListSelected();


        }
        private void ListView_defaultlists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ListViewItem ListItem = ListView_defaultlists.SelectedItem as ListViewItem;

            if (ListItem != null)
            {


                switch (ListItem.Name)
                {
                    case "Today":

                        TasksTodo = task.GetTodayTasks();
                        task_list.ItemsSource = TasksTodo;
                        task_list.Header = "Today";

                        break;

                    case "Tomorrow":
                        TasksTodo = task.GetTomorrowTasks();
                        task_list.ItemsSource = TasksTodo;
                        task_list.Header = "Tomorrow";

                        break;

                    case "Planned":
                        TasksTodo = task.GetPlannedTasks();
                        task_list.ItemsSource = TasksTodo;
                        task_list.Header = "Planned";

                        break;

                    default:
                        TasksTodo = task.GetTasks(GetListSelected());
                        task_list.ItemsSource = TasksTodo;
                        task_list.Header = "Tasks";

                        break;
                }

            }

        }

        private void ListView_defaultlists_ItemClick(object sender, ItemClickEventArgs e)
        {
            ListView_tasklists.SelectedItem = null;
            TaskForm.Visibility = Visibility.Collapsed;

        }

        private void ListView_tasklists_ItemClick(object sender, ItemClickEventArgs e)
        {
            ListView_defaultlists.SelectedItem = null;
            TaskForm.Visibility = Visibility.Collapsed;
        }

        private void Play_task_Click(object sender, RoutedEventArgs e)
        {
            //Select the task from the button data context
            var button = sender as Button;
            var item = (TaskTodo)button.DataContext;

           task_list.SelectedItem = item;

            //Set the task selected to the selected in task list listview
            (App.Current as App).TaskSelected = task_list.SelectedItem as TaskTodo;

            //Open the new window
            var f_window = new Focus_Task();
            f_window.Activate();

            //Close the window
            this.Close();
        }
        private void Circle_Checked(object sender, RoutedEventArgs e)
        {
            var button = (RadioButton)sender;
            var item = (TaskTodo)button.DataContext;
            item.Done = "True";
            item.UpdateTask();
            TasksTodo.Remove(item);
        }

        private void Name_list_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {

            if (sender.FocusState != FocusState.Unfocused)
            {
                //Show the lists with the name most similar to the less.

                Lists_listbox.ItemsSource = TasksLists.OrderBy(sender.Text.CompareTo).Take(4);
                Lists_listbox.Visibility = Visibility.Visible;

            }

        }

        private void Lists_listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //change the text of the listname textbox with the list selected 
            if (Lists_listbox.SelectedItem != null)
            {
                string ListSelected = Lists_listbox.SelectedItem as string;
                Name_list.Text = ListSelected;

            }

        }

        private void Name_list_LostFocus(object sender, RoutedEventArgs e)
        {
            //Close the listbox with the lists
            Lists_listbox.Visibility = Visibility.Collapsed;
            string ListName = string.Empty;

            if (task_list.SelectedItem != null)
            {
                ListName = (task_list.SelectedItem as TaskTodo).ListName;
            }


            //Create a new list if it's not made already and the list name it's not empty
            if (TasksLists.Contains(ListName) == false && ListName != String.Empty)
            {
                TasksLists.Add((task_list.SelectedItem as TaskTodo).ListName);
                Tasklist.AddList((task_list.SelectedItem as TaskTodo).ListName);
            }
        }

        private void Name_list_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            //Close the listbox with the lists
            Lists_listbox.Visibility = Visibility.Collapsed;
            String ListName = (task_list.SelectedItem as TaskTodo).ListName;
            //Create a new list if it's not made already and the list name it's not empty

            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                if (TasksLists.Contains(ListName) == false && ListName != String.Empty)
                {
                    TasksLists.Add((task_list.SelectedItem as TaskTodo).ListName);
                    Tasklist.AddList((task_list.SelectedItem as TaskTodo).ListName);
                }
            }
        }

        private void task_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckPriority();
            
            if (task_list.SelectedItem != null)
            {
                task = (task_list.SelectedItem as TaskTodo);
                SubTasks = task.GetSubtasks();
                subtask_list.ItemsSource = SubTasks;

            }

        }

        private void subtaskdone_radiobutton_Checked(object sender, RoutedEventArgs e)
        {
            var button = (RadioButton)sender;
            var item = (TaskTodo)button.DataContext;
            item.Done = "True";
            item.UpdateTask();
            SubTasks.Remove(item);
        }

    }


    
}
