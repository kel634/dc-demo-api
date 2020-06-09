public class AssetMetadata
{
  public string Key { get; set; }
  public string Value { get; set; }

  public AssetMetadata(string key, string value)
  {
      this.Key = key;
      this.Value = value;
  }
}