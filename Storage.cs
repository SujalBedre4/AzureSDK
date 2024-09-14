using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Storage;
using Azure.ResourceManager.Storage.Models;
using System;
using System.Threading.Tasks;

class Program
{
    private const string subscriptionId = "a7078587-67b9-4f1e-b1b8-d89e4156543d";
    private const string resourceGroupName = "myNewResourceGroup";
    private const string storageAccountName = "mystorageaccount123"; // Must be unique
    private const string location = "East US"; // Specify your preferred location

    static async Task Main(string[] args)
    {
        // Authenticate using DefaultAzureCredential
        var armClient = new ArmClient(new DefaultAzureCredential());

        // Create a new resource group
        var resourceGroupData = new ResourceGroupData(location);
        var resourceGroup = await armClient.GetDefaultSubscription().GetResourceGroups().CreateOrUpdateAsync(resourceGroupName, resourceGroupData);
        Console.WriteLine($"Created Resource Group: {resourceGroup.Value.Data.Name}");

        // Create a new storage account
        var storageAccountData = new StorageAccountData
        {
            Location = location,
            Sku = new Sku
            {
                Name = SkuName.StandardLRS, // Standard Locally Redundant Storage
            },
            Kind = StorageKind.StorageV2 // General-purpose v2
        };

        var storageAccount = await resourceGroup.Value.GetStorageAccounts().CreateOrUpdateAsync(storageAccountName, storageAccountData);
        Console.WriteLine($"Created Storage Account: {storageAccount.Value.Data.Name}");
        Console.WriteLIne("Storage account created successfully.");
    }
}
