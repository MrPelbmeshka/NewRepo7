using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Model;

namespace ToDoList
{
    public class DbContact: DbContext
    {
        public DbContact() : base("ContactString")
        {

        }

        public DbSet<DoList> DoLists { get; set; }
    }
}
