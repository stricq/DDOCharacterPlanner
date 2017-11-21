using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace DdoCharacterPlanner.Repository.Loaders {

  public class DataFileLoaderBase {

    #region Protected Methods

    protected static async Task VerifyAndDownloadAsync(string filePath, string urlPath) {
      if (!await Task.Run(() => File.Exists(filePath))) {
        using(StreamWriter writer = new StreamWriter(filePath)) {
          using(HttpClient client = new HttpClient()) {
            Stream reader = await client.GetStreamAsync(new Uri(urlPath));

            await reader.CopyToAsync(writer.BaseStream);
          }
        }
      }
    }

    protected static async Task<List<T>> ReadDataFileAsync<T>(StreamReader stream, Action<T, string, string> parser) where T : new() {
      T item = new T();

      List<T> list = new List<T>();

      string line = await stream.ReadLineAsync();

      while(line != null) {
        if (line.Trim().Length == 0) {
          list.Add(item);

          item = new T();
        }
        else if (!line.StartsWith("//") && !line.StartsWith("[")) {
          string property = line.Substring(0, line.IndexOf(':')).TrimStart();

          string value = line.Substring(line.IndexOf(':') + 2).TrimEnd();

          while(!value.EndsWith(";")) value += " " + (await stream.ReadLineAsync())?.Trim();

          value = value.TrimEnd(';');

          parser(item, property, value);
        }

        line = await stream.ReadLineAsync();
      }

      list.Add(item);

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

      return data.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(value => value.Trim()).ToList();
    }

    #endregion Protected Methods

  }

}
