using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SIIILab2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIIILab2.Tests
{
    internal class DependencyInjection
    {
        public static ServiceCollection InitilizeServices()
        {
            var services = new ServiceCollection();
            var options = new DbContextOptionsBuilder<ApplicationContext>().UseInMemoryDatabase("SIIIdb").Options;
            services.AddScoped(_ => new ApplicationContext(options));
            return services;
        }
    }
}
