using System;
using System.Threading.Tasks;
using System.Linq;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Compute;
using Azure.ResourceManager.Resources;
using Microsoft.Extensions.Configuration;
using Services;

class Program
{
    static async Task Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
        string subscriptionId = config["Azure:SubscriptionId"];
                if (string.IsNullOrEmpty(subscriptionId))
        {
            Console.WriteLine("Error: Subscription ID is missing! Please check your appsettings.json or environment variables.");
            return;
        }

        Console.WriteLine($"Loaded Subscription ID: {subscriptionId}");  


        // Authenticate using Azure CLI login (DefaultAzureCredential checks this)
        var credential = new DefaultAzureCredential();
        var armClient = new ArmClient(credential);

        // Connect to your subscription
        var subscription = armClient.GetSubscriptionResource(
            new ResourceIdentifier($"/subscriptions/{subscriptionId}"));

        VmService vmService = new VmService(subscription);
        await vmService.ListVmsAsync();
        
    }
}
