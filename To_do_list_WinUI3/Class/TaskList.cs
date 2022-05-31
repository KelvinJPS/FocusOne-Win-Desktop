using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using to_do_list_WinUI3.Data_access;
using To_do_list_WinUI3;

namespace To_do_list_WinUI3
{

    public class TaskList
    {
        TasklistSqliteDataAccess tasklistSqlite = new TasklistSqliteDataAccess();
        public ObservableCollection<string> Getlists() => tasklistSqlite.GetListsDB();
        public void AddList(string NameList) => tasklistSqlite.AddList(NameList);

        public string listSelected { get; set; }
    }
}