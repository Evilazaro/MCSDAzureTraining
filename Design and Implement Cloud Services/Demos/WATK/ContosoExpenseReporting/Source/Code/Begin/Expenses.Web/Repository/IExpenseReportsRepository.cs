// ----------------------------------------------------------------------------------
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

namespace Expenses.Web.Repository
{
    using System;
    using System.Collections.Generic;
    using Expenses.Web.Models;

    public interface IExpenseReportsRepository
    {
        void Delete(int reportId);

        ExpenseReport GetReport(int reportId);

        IEnumerable<ExpenseReport> GetReports(bool includeDrafts);

        IEnumerable<ExpenseReport> GetReports(bool includeDrafts, int count);

        IEnumerable<ExpenseReport> GetReports(string status);

        IEnumerable<ExpenseReport> GetReports(string status, int count);

        IEnumerable<ExpenseReport> GetEmployeesReports(Guid managerId, string status, int count);

        IEnumerable<ExpenseReport> GetUserReports(Guid userId);

        IEnumerable<ExpenseReport> GetUserReports(Guid userId, int count);

        IEnumerable<ExpenseReport> GetUserReports(Guid userId, string status);

        IEnumerable<ExpenseReport> GetUserReports(Guid userId, string status, int count);

        ExpenseReport Save(ExpenseReport report);
    }
}
