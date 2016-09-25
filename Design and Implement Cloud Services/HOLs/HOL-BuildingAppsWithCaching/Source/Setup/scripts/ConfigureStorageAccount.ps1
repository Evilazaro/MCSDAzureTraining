﻿### PARAMETERS DEFINITION ###

param (
    [Parameter(ParameterSetName='Local', Mandatory = $true)]
    [switch]$UseDevStorage,
    [Parameter(ParameterSetName='Remote', Mandatory = $true)]
    [string]$AccountName = $null,
    [Parameter(ParameterSetName='Remote', Mandatory = $true)]
    [string]$AccountKey = $null
)

### TYPES DEFINITION AND ASSEMBLIES IMPORTING ###

try
{
    $key = 'HKLM:\SOFTWARE\Microsoft\Microsoft SDKs\ServiceHosting'
    $registryProperty = Get-ChildItem $key -ErrorAction Stop | Sort CreationTime -Descending | Select -First 1 | Get-ItemProperty -ErrorAction Stop
    $fullVersion = ($registryProperty | Select FullVersion).FullVersion
    $installPath = ($registryProperty | Select InstallPath).InstallPath

    if ($fullVersion -lt "2.2.0000.0") { throw }
    $ref1 = "${installPath}ref\Microsoft.WindowsAzure.Storage.dll"
    $ref2 = "${installPath}ref\Microsoft.WindowsAzure.StorageClient.dll"
    if (-not(Test-Path $ref1) -or -not(Test-Path $ref2)) { throw }
}
catch
{
    throw "The Windows Azure Storage Client Library could not be found. Please make sure you have installed Windows Azure SDK for .NET 2.2 or later"
}

Add-Type -Path $ref1
Add-Type -Path $ref2
$cpar = New-Object System.CodeDom.Compiler.CompilerParameters
$cpar.ReferencedAssemblies.Add($ref1)
$cpar.ReferencedAssemblies.Add($ref2)
Add-Type @"
    using System;
    using Microsoft.WindowsAzure.Storage.Table;
 
    public class Customer : TableEntity
    {
        public Customer()
        {
        }
        public string Id { get; set; }
        public string Company { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public string Comment { get; set; }
        public DateTime ContractDate { get; set; }
    }
"@ -CompilerParameters $cpar

### TABLE CREATION ###

if ($UseDevStorage -eq $true)
{
    $storageAccount = [Microsoft.WindowsAzure.Storage.CloudStorageAccount]::Parse([Microsoft.WindowsAzure.Storage.CloudStorageAccount]::DevelopmentStorageAccount)
    $tableClient = $storageAccount.CreateCloudTableClient()
}
else
{
    $uri = New-Object System.Uri("http://$AccountName.table.core.windows.net/");
    $credentials = new-object Microsoft.WindowsAzure.Storage.Auth.StorageCredentials($AccountName, $AccountKey)
    $tableClient = New-Object Microsoft.WindowsAzure.Storage.Table.CloudTableClient($uri,$credentials);
}

$table = $tableClient.GetTableReference("customers") # here log is the table name you want
$table.DeleteIfExists()
while ($true)
{
    try
    {
        $table.CreateIfNotExists();
        break;
    }
    catch [Microsoft.WindowsAzure.Storage.StorageException]
    {
        if ($_.Exception.RequestInformation.HttpStatusCode -ne [System.Net.HttpStatusCode]::Conflict)
        {
            throw;
        }
        else
        {
            [System.Threading.Thread]::Sleep(1000);
        }
    }
}

### ENTITIES INSERTION ###

for ($i=0; $i -lt 4; $i++)
{
    $batchOperation = New-Object Microsoft.WindowsAzure.Storage.Table.TableBatchOperation
    for ($j=0; $j -lt 100; $j++)
    {
        $index = [math]::floor($i / 2) * 100 + $j * 2 + ($i % 2) + 1
        $customer = New-Object Customer
        $customer.Company = "Company" + ($i % 2).ToString()
        $customer.PartitionKey = $customer.Company
        $customer.Id = "{0:0000000}" -f $index
        $customer.RowKey = $customer.Id
        $customer.Name = "Customer " + $index.ToString()
        $customer.Value = (Get-Random -minimum 0 -maximum 99999) / 10.0
        $customer.Comment = "This is customer " + $index.ToString()
        $customer.ContractDate = Get-Date

        $batchOperation.InsertOrReplace($customer);
    }

    $table.ExecuteBatch($batchOperation);
}