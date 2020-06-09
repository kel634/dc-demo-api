using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

public class VisionWorker
{
  public Dictionary<string, string> Metadata { get; set; }
  public Stream ThumbnailStream { get; set; }
  public string Description { get; set; }

  ComputerVisionClient vision;
  VisualFeatureTypes[] features = new VisualFeatureTypes[] {
          VisualFeatureTypes.Description,
          VisualFeatureTypes.Categories,
          VisualFeatureTypes.ImageType,
          VisualFeatureTypes.Tags
        };

  public VisionWorker(string visionKey, string visionEndpoint)
  {
    vision = new ComputerVisionClient(
            new ApiKeyServiceClientCredentials(visionKey),
            new System.Net.Http.DelegatingHandler[] { });
    vision.Endpoint = visionEndpoint;
    Metadata = new Dictionary<string, string>();
  }

  public async Task Run(string url)
  {
    var result = await vision.AnalyzeImageAsync(url, features);

    // Record the image description and tags in blob metadata
    Metadata.Add("Caption", result.Description.Captions[0].Text);
    Description = result.Description.Captions[0].Text;

    for (int i = 0; i < result.Description.Tags.Count; i++)
    {
      string key = String.Format("Tag{0}", i);
      Metadata.Add(key, result.Description.Tags[i]);
    }

    // generate thumbnail
    ThumbnailStream = await vision.GenerateThumbnailAsync(200, 200, url, true);
  }
}