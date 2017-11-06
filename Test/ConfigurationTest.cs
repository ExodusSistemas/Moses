using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Moses.Extensions;
using Xunit;
using Moses.Test.SampleApp;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json.Linq;

namespace Moses.Test
{
    public class ConfigurationTest
    {

        private IWebHostBuilder GetHostBuilder()
        {
            return WebHost.CreateDefaultBuilder(new string[] { })
                .UseStartup<Startup>();

        }

        [Fact]
        public void DefaultConfigurationTest()
        {
            var iwh = WebHost.CreateDefaultBuilder(new string[] { })
                .UseShutdownTimeout(TimeSpan.FromSeconds(10))
               .UseStartup<Startup>();

        }

        [Fact]
        public async void FileConfigurationTest()
        {
            using (var server = new TestServer(GetHostBuilder()))
            using (var client = server.CreateClient())
            {
                var q = await client.GetAsync("/Sample/Configuration").ConfigureAwait(false);

                JObject jobj = Newtonsoft.Json.JsonConvert.DeserializeObject(q.Content.ReadAsStringAsync().Result) as JObject;
                Assert.Equal("dev@exodus.eti.br", jobj["emails"]["systemEmail"]["address"].Value<string>());
                client.CancelPendingRequests();
            }

        }

        [Fact]
        public async void SimpleControllerTestAsync()
        {
            using (var server = new TestServer(GetHostBuilder()))
            using (var client = server.CreateClient())
            {
                var q = await client.GetAsync("/Sample/SendTestEmail?nome=Teste&email=fulanodetal@exodus.eti.br&fone=55-5555-5555&mensagem=TESTE&noneField=").ConfigureAwait(false);
                
                JObject jobj = Newtonsoft.Json.JsonConvert.DeserializeObject(q.Content.ReadAsStringAsync().Result) as JObject;
                Assert.True(jobj["success"].Value<bool>());
                client.CancelPendingRequests();
            }
            
        }

        [Fact]
        public async void SimpleHeaderTestAsync()
        {
            using (var server = new TestServer(GetHostBuilder()))
            using (var client = server.CreateClient())
            {
                var q = await client.GetAsync("/Sample/SendTestEmail?nome=Teste&email=fulanodetal@exodus.eti.br&fone=55-5555-5555&mensagem=TESTE&noneField=").ConfigureAwait(false);

                Assert.True(q.Headers.Any(s => s.Key.Contains("Moses")));
                client.CancelPendingRequests();
            }

        }
    }
}
