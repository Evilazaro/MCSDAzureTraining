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

namespace Expenses.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Expenses.Web.Repository;
    using Expenses.Web.ViewModels;

    public class HomeController : Controller
    {
        private readonly IExpenseReportsRepository expenseReportRepository;
        private readonly IUserProfileRepository userProfileRepository;

        public HomeController()
            : this(new ExpenseReportsRepository(), new UserProfileRepository())
        {
        }

        public HomeController(IExpenseReportsRepository expenseReportRepository, IUserProfileRepository userProfileRepository)
        {
            this.expenseReportRepository = expenseReportRepository;
            this.userProfileRepository = userProfileRepository;            
        }

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {                
                return RedirectToAction("Dashboard");
            }
            
            return View();
        }

        [Authorize]
        public ActionResult Dashboard()
        {            
            var userId = this.GetUserId();

            int pending;
            int approved;
            int rejected;

            // TODO: Create repository method to retrieve the count
            if (User.IsInRole("manager"))
            {
                pending = this.expenseReportRepository.GetEmployeesReports(userId, "pending", 0).ToList().Count();
                approved = this.expenseReportRepository.GetEmployeesReports(userId, "approved", 0).ToList().Count();
                rejected = this.expenseReportRepository.GetEmployeesReports(userId, "rejected", 0).ToList().Count();
            }
            else
            {
                pending = this.expenseReportRepository.GetUserReports(userId, "pending").ToList().Count();
                approved = this.expenseReportRepository.GetUserReports(userId, "approved").ToList().Count();
                rejected = this.expenseReportRepository.GetUserReports(userId, "rejected").ToList().Count();
            }

            return View(new DashboardItems { Pending = pending, Approved = approved, Rejected = rejected });
        }
        
        public ActionResult Support()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Policies()
        {
            return View();
        }

        private Guid GetUserId()
        {
            return this.userProfileRepository.GetProfile(User.Identity.Name).UserId;
        }
    }
}
