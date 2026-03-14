using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Linq;
using Umre.Web.Data;
using Umre.Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Umre.Web.Services
{
    public class ExchangeRateBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        
        private readonly TimeSpan _period = TimeSpan.FromHours(1);

        public ExchangeRateBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            using (var timer = new PeriodicTimer(_period))
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    try
                    {
                        await UpdateRatesAsync();
                    }
                    catch (Exception)
                    {
                        
                    }
                }
            }
        }

        private async Task UpdateRatesAsync()
        {
             using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var autoUpdateSetting = await context.SiteSettings.FirstOrDefaultAsync(s => s.Key == "Exchange_AutoUpdate");

                if (autoUpdateSetting != null && autoUpdateSetting.Value == "true")
                {
                    await FetchAndSaveRates(context);
                }
            }
        }

        private async Task FetchAndSaveRates(AppDbContext context)
        {
            try
            {
                string tcmbUrl = "https://www.tcmb.gov.tr/kurlar/today.xml";
                using (var client = new HttpClient())
                {
                    var xmlStr = await client.GetStringAsync(tcmbUrl);
                    var xDoc = XDocument.Parse(xmlStr);

                    var usd = xDoc.Descendants("Currency").FirstOrDefault(x => x.Attribute("CurrencyCode")?.Value == "USD")?.Element("ForexSelling")?.Value;
                    var eur = xDoc.Descendants("Currency").FirstOrDefault(x => x.Attribute("CurrencyCode")?.Value == "EUR")?.Element("ForexSelling")?.Value;
                    var sar = xDoc.Descendants("Currency").FirstOrDefault(x => x.Attribute("CurrencyCode")?.Value == "SAR")?.Element("ForexSelling")?.Value;

                    if (!string.IsNullOrEmpty(usd) && !string.IsNullOrEmpty(eur) && !string.IsNullOrEmpty(sar))
                    {
                        var newInfo = $"USD: {usd} ₺  |  EUR: {eur} ₺  |  SAR: {sar} ₺";

                        var sInfo = await context.SiteSettings.FirstOrDefaultAsync(s => s.Key == "Exchange_Info");
                        if (sInfo == null) { sInfo = new SiteSetting { Key = "Exchange_Info" }; context.SiteSettings.Add(sInfo); }
                        sInfo.Value = newInfo;

                        var sLast = await context.SiteSettings.FirstOrDefaultAsync(s => s.Key == "Exchange_LastUpdate");
                        if (sLast == null) { sLast = new SiteSetting { Key = "Exchange_LastUpdate" }; context.SiteSettings.Add(sLast); }
                        sLast.Value = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception)
            {
                
            }
        }
    }
}
