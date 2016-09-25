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
    using System.Data;
    using System.Linq;
    using Expenses.Web.Models;

    public class UserProfileRepository : IUserProfileRepository
    {
        public UserProfile GetProfile(string username)
        {
            using (var ctx = this.GetContext())
            {
                return ctx.UserProfiles.SingleOrDefault(u => u.UserName.Equals(username));
            }
        }

        public UserProfile Save(UserProfile profile)
        {
            using (var ctx = this.GetContext())
            {
                ctx.Entry(profile).State = this.GetProfile(profile.UserName) != null ? EntityState.Modified : EntityState.Added;
                ctx.SaveChanges();
                return profile;
            }            
        }

        private ReportsContext GetContext()
        {
            return new ReportsContext();
        }
    }
}