using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

public class StorageClient
{
  BlobServiceClient blobServiceClient;
  public StorageClient(string storageConnectionString)
  {
      blobServiceClient = new BlobServiceClient(storageConnectionString);
  }

  public async Task<BlobClient> StoreFile(string containerName, string fileName, Stream file, string contentType)
  {
    BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
    BlobClient blobClient = containerClient.GetBlobClient(fileName);

    var blobInfo = await blobClient.UploadAsync(file, new BlobHttpHeaders { ContentType = contentType }, conditions: null);

    return blobClient;
  }

  public BlobClient GetFile(string containerName, string fileName)
  {
    BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
    BlobClient blobClient = containerClient.GetBlobClient(fileName);

    return blobClient;
  }

  public async Task StoreMetadata(BlobClient blobClient, Dictionary<string, string> metadata)
  {
    await blobClient.SetMetadataAsync(metadata);
  }
}