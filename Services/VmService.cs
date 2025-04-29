using Azure;
using Azure.ResourceManager;
using Azure.ResourceManager.Compute;
using Azure.ResourceManager.Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services {
    public class VmService
    {
        private readonly SubscriptionResource _subscription;

        public VmService(SubscriptionResource subscription)
        {
            _subscription = subscription;
        }

        public async Task ListVmsAsync()
        {
            Console.WriteLine("Checking for VMs...");

            bool hasVm = false;

            await foreach (var vm in _subscription.GetVirtualMachinesAsync())
            {
                if (!hasVm)
                {
                    Console.WriteLine("Listing VMs:");
                    hasVm = true;
                }
                Console.WriteLine($"- {vm.Data.Name}");
            }

            if (!hasVm)
            {
                Console.WriteLine("No VMs found in your subscription.");
            }
        }

        public async Task DeleteVmAsync(string vmName)
        {
            Console.WriteLine($"Attempting to delete VM: {vmName}");
            await foreach (var vm in _subscription.GetVirtualMachinesAsync())
            {
                if (vm.Data.Name.Equals(vmName, StringComparison.OrdinalIgnoreCase))
                {
                    ArmOperation deleteOp = await vm.DeleteAsync(WaitUntil.Completed); // wait until it's fully deleted
                    Console.WriteLine($"Deleted VM: {vmName}");
                    return;
                }
            }
            Console.WriteLine($"VM '{vmName}' not found.");
        }
    }
}