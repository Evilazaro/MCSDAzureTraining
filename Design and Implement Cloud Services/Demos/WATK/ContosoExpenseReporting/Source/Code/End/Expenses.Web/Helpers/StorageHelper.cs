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

namespace Expenses.Web.Helpers
{
    using System;
    using System.Configuration;
    using System.Security.Principal;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.StorageClient;

    public static class StorageHelper
    {
        public static CloudBlobContainer GetUserContainer(IIdentity user)
        {
            var connection = CloudStorageAccount.Parse(ConfigurationManager.AppSettings.Get("WAZStorageAccount"));
            var client = connection.CreateCloudBlobClient();
            client.RetryPolicy = RetryPolicies.Retry(3, TimeSpan.FromSeconds(5));

            var blobContainer = client.GetContainerReference(ExtractContainerName(user));
            blobContainer.CreateIfNotExist();
            blobContainer.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Container });

            return blobContainer;
        }

        private static string ExtractContainerName(IIdentity user)
        {
            return user.Name.ToLowerInvariant().Replace(" ", "-");
        }
    }
}