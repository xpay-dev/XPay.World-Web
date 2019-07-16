using System.Collections.Generic;
using System.IO;
using XPW.Utilities.NoSQL;

namespace XPW.NoSQLDemo.Utilities {
     public static class FileChecker<T> where T : class, new() {
          public static void AutoCreateIfNotExists(string filePath) {
               try {
                    if (!File.Exists(filePath)) {
                         var newFile = File.Create(filePath);
                         newFile.Close();
                         newFile.Dispose();
                         Writer<T>.JsonWriterListAsync(new List<T>(), filePath);
                    }
               } catch {
                    return;
               }
          }
          public static bool IsExists(string filePath) {
               return File.Exists(filePath);
          }
     }
}