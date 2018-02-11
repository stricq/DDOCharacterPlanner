using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace DdoCharacterPlanner.Repository.Loaders {

  public class DataFileLoaderBase {

    #region Protected Methods

    protected static async Task<bool> VerifyAndDownloadAsync(HttpClient client, string filePath, string urlPath, bool downloadFilesFromWeb) {
      string dirPath = Path.GetDirectoryName(filePath);

      if (!await Task.Run(() => Directory.Exists(dirPath))) await Task.Run(() => Directory.CreateDirectory(dirPath));

      if (downloadFilesFromWeb || !await Task.Run(() => File.Exists(filePath))) {
        HttpResponseMessage response = await client.GetAsync(new Uri(urlPath));

        if (!response.IsSuccessStatusCode) {
          Console.WriteLine($"{urlPath} returned code {response.StatusCode}.");

          return false;
        }

        using(StreamWriter writer = new StreamWriter(filePath)) {
          await response.Content.CopyToAsync(writer.BaseStream);
        }
      }

      return true;
    }

    protected static async Task<List<T>> ReadDataFileAsync<T>(StreamReader stream, Action<T, string, string> parser, bool skip = false) where T : new() {
      T item = default;

      List<T> list = new List<T>();

      string line = await stream.ReadLineAsync();

      while(line != null) {
        if (line.StartsWith("[")) {
          string header = line.Remove(line.IndexOf("]", StringComparison.InvariantCulture)).Replace("[", null);

          parser(default, header, null);

          skip = false;

          line = await stream.ReadLineAsync();

          continue;
        }

        if (skip || line.StartsWith("//") || line.StartsWith(@"\\")) {
          //
          // Do nothing, just consume lines...
          //
        }
        else if (line.Trim().Length == 0) {
          if (item != null) list.Add(item);

          item = default;
        }
        else {
          string property = line.Substring(0, line.IndexOf(':')).TrimStart();

          string value = line.Substring(line.IndexOf(':') + 2).TrimEnd();

          while(!value.EndsWith(";")) value += " " + (await stream.ReadLineAsync())?.Trim();

          value = value.TrimEnd(';');

          if (item == null) item = new T();

          parser(item, property, value);
        }

        line = await stream.ReadLineAsync();
      }

      if (item != null) list.Add(item);

      return list;
    }

    protected static string ToNullString(string data) {
      if (data == "" || data.ToLower() == "tbd") return null;

      return data;
    }

    protected static int[] ToIntArray(string data) {
      return data.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToArray();
    }

    protected static List<string> ToStringList(string data) {
      if (data == "" || data.ToLower() == "tbd") return null;

      return data.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Where(value => !String.IsNullOrWhiteSpace(value)).Select(value => value.Trim()).ToList();
    }

    #endregion Protected Methods

  }

}
