﻿// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

namespace MyTodo.WebUx.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web;

    public class TodoItemContext : DbContext
    {
        //// You can add custom code to this file. Changes will not be overwritten.
        //// 
        //// If you want Entity Framework to drop and regenerate your database
        //// automatically whenever you change your model schema, please use data migrations.
        //// For more information refer to the documentation:
        //// http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public TodoItemContext()
            : base("name=DefaultConnection")
        {
        }

        public System.Data.Entity.DbSet<MyTodo.WebUx.Models.TodoItem> TodoItems { get; set; }

        public System.Data.Entity.DbSet<MyTodo.WebUx.Models.TodoList> TodoLists { get; set; }
    }
}