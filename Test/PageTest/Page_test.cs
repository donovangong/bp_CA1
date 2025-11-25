using BPCalculator;
using BPCalculator.Pages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.AspNetCore.Hosting;

namespace PageTest
{
    [TestClass]
    public sealed class Page_test
    {
        [TestMethod]
        public void TestMethod1()
        {
            var indexPage = new BloodPressureModel();
            indexPage.OnGet();
            indexPage.BP.Systolic = 120;
            indexPage.BP.Diastolic = 70;
            indexPage.OnPost();

            // Privacy page
            var privacyPage = new PrivacyModel(NullLogger<PrivacyModel>.Instance);
            privacyPage.OnGet();

            // Error page
            var error = new ErrorModel(new LoggerFactory().CreateLogger<ErrorModel>());
            error.PageContext = new PageContext { HttpContext = new DefaultHttpContext() };
            error.OnGet();

            // Covers invalid logic branch
            indexPage.BP.Systolic = 80;
            indexPage.BP.Diastolic = 90;
            indexPage.OnPost();
            Assert.IsFalse(indexPage.ModelState.IsValid);

            //Program test
            var hostBuilder = Program.CreateHostBuilder(Array.Empty<string>());
            using var host = hostBuilder.Build();
            host.Start();
            host.StopAsync().Wait();

            //Startup test
            using var nhost = Host.CreateDefaultBuilder().ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); }).Build();
            nhost.Start();
            nhost.StopAsync().Wait();

        }
    }
}
