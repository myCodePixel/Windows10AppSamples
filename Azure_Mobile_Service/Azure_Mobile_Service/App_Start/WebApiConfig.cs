using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Web.Http;
using Azure_Mobile_Service.DataObjects;
using Azure_Mobile_Service.Models;
using Microsoft.WindowsAzure.Mobile.Service;

namespace Azure_Mobile_Service
{
    public static class WebApiConfig
    {
        public static void Register()
        {
            // Use this class to set configuration options for your mobile service
            ConfigOptions options = new ConfigOptions();

            // Use this class to set WebAPI configuration options
            HttpConfiguration config = ServiceConfig.Initialize(new ConfigBuilder(options));

            // To display errors in the browser during development, uncomment the following
            // line. Comment it out again when you deploy your service for production use.
            // config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            Database.SetInitializer(new MobileServiceInitializer());
        }
    }

    public class MobileServiceInitializer : DropCreateDatabaseIfModelChanges<MobileServiceContext>
    {
        protected override void Seed(MobileServiceContext context)
        {
            List<Book> Books = new List<Book>
            {
                new Book { Id = Guid.NewGuid().ToString(), Title="First", Author = "Abd", Complete = false },
                new Book { Id = Guid.NewGuid().ToString(), Title="Second",Author = "Abd", Complete = false },
            };

            foreach (Book Book in Books)
            {
                context.Set<Book>().Add(Book);
            }

            base.Seed(context);
        }
    }
}

