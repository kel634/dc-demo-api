using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dc_demo_api.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using Azure.Storage.Blobs;
using System.Text.Json;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Azure.Storage.Blobs.Models;

namespace dc_demo_api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AssetController : ControllerBase
  {
    private readonly IConfiguration _configuration;

    private readonly DCDemoContext _context;

    public AssetController(DCDemoContext context, IConfiguration configuration)
    {
      _context = context;
      _configuration = configuration;
    }

    // GET: api/Asset
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Asset>>> GetAsset()
    {
      return await _context.Asset.ToListAsync();
    }

    // GET: api/Asset/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Asset>> GetAsset(int id)
    {
      var asset = await _context.Asset.Include(asset => asset.AssetVariants).FirstOrDefaultAsync(asset => asset.AssetId == id);

      if (asset == null)
      {
        return NotFound();
      }

      return asset;
    }

    // POST: api/Asset
    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    [HttpPost]
    public async Task<ActionResult<Asset>> PostAsset([FromQuery] int folderId, IFormFile file)
    {
      var storageConnectionString = _configuration["ConnectionStrings:AzureStorageConnectionString"];
      var visionKey = _configuration["VISION_KEY"];
      var visionEndpoint = _configuration["VISION_ENDPOINT"];

      string originalFileName = file.FileName;
      string ext = Path.GetExtension(originalFileName);
      string fileName = "asset_" + Guid.NewGuid().ToString() + ext;

      var storage = new StorageClient(storageConnectionString);
      var visionWorker = new VisionWorker(visionKey, visionEndpoint);


      using var stream = file.OpenReadStream();
      // store the file in azure blob storage
      var blobClient = await storage.StoreFile("assets", fileName, stream, file.ContentType);

      // generate metadata and thumbnail
      await visionWorker.Run(blobClient.Uri.ToString());
      // store metadata
      await storage.StoreMetadata(blobClient, visionWorker.Metadata);
      // store thumbnail
      var thumbnailBlobClient = await storage.StoreFile("thumbs", fileName, visionWorker.ThumbnailStream, "image/png");

      // create the new Asset
      Asset asset = new Asset
      {
        FileName = fileName,
        DisplayName = visionWorker.Description,
        FolderId = folderId,
        PreviewUrl = thumbnailBlobClient.Uri.ToString()
      };
      var originalVariant = new AssetVariant { AssetId = asset.AssetId, Url = blobClient.Uri.ToString(), VariantTypeId = 1 };
      var thumbnailVariant = new AssetVariant { AssetId = asset.AssetId, Url = thumbnailBlobClient.Uri.ToString(), VariantTypeId = 2 };
      asset.AssetVariants.Add(originalVariant);
      asset.AssetVariants.Add(thumbnailVariant);

      _context.Asset.Add(asset);
      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateException)
      {
        if (AssetExists(asset.AssetId))
        {
          return Conflict();
        }
        else
        {
          throw;
        }
      }

      return CreatedAtAction("GetAsset", new { id = asset.AssetId }, asset);
    }

    // DELETE: api/Asset/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<Asset>> DeleteAsset(int id)
    {
      var asset = await _context.Asset.FindAsync(id);
      if (asset == null)
      {
        return NotFound();
      }

      _context.Asset.Remove(asset);
      await _context.SaveChangesAsync();

      return asset;
    }

    private bool AssetExists(int id)
    {
      return _context.Asset.Any(e => e.AssetId == id);
    }
  }
}
