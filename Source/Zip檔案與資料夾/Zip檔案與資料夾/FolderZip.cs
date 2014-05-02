﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Zip檔案與資料夾
{
    class FolderZip
    {
        /// <summary>
        /// ZipFolder and its sub folders
        /// </summary>
        /// <param name="tcpSocket"></param>
        async private static Task ZipFolderContents(StorageFolder sourceFolder, ZipArchive archive, string baseDirPath)
        {
            IReadOnlyList<StorageFile> files = await sourceFolder.GetFilesAsync();

            foreach (StorageFile file in files)
            {
                ZipArchiveEntry readmeEntry = archive.CreateEntry(GetCompressedFileName(baseDirPath, file));

                byte[] buffer = WindowsRuntimeBufferExtensions.ToArray(await FileIO.ReadBufferAsync(file));

                // And write the contents to it
                using (Stream entryStream = readmeEntry.Open())
                {
                    await entryStream.WriteAsync(buffer, 0, buffer.Length);
                }
            }

            IReadOnlyList<StorageFolder> subFolders = await sourceFolder.GetFoldersAsync();

            if (subFolders.Count() == 0) return;

            foreach (StorageFolder subfolder in subFolders)
                await ZipFolderContents(subfolder, archive, baseDirPath);
        }

        async public static Task ZipFolder(StorageFolder sourceFolder, StorageFolder destnFolder, string zipFileName)
        {
            StorageFile zipFile = await destnFolder.CreateFileAsync(zipFileName, CreationCollisionOption.ReplaceExisting);

            Stream zipToCreate = await zipFile.OpenStreamForWriteAsync();

            ZipArchive archive = new ZipArchive(zipToCreate, ZipArchiveMode.Update);
            await ZipFolderContents(sourceFolder, archive, sourceFolder.Path);
            archive.Dispose();
        }

        static private string GetCompressedFileName(string baseDirPath, StorageFile file)
        {
            return file.Path.Remove(0, baseDirPath.Length);
        }

        async public static Task UnZipFile(StorageFolder zipFileDirectory, string zipFilename, StorageFolder extractFolder = null)
        {
            if (extractFolder == null) extractFolder = zipFileDirectory;

            var folder = ApplicationData.Current.LocalFolder;

            using (var zipStream = await folder.OpenStreamForReadAsync(zipFilename))
            {
                using (MemoryStream zipMemoryStream = new MemoryStream((int)zipStream.Length))
                {
                    await zipStream.CopyToAsync(zipMemoryStream);

                    using (var archive = new ZipArchive(zipMemoryStream, ZipArchiveMode.Read))
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            if (entry.Name != "")
                            {
                                using (Stream fileData = entry.Open())
                                {
                                    StorageFile outputFile = await extractFolder.CreateFileAsync(entry.FullName, CreationCollisionOption.ReplaceExisting);
                                    using (Stream outputFileStream = await outputFile.OpenStreamForWriteAsync())
                                    {
                                        await fileData.CopyToAsync(outputFileStream);
                                        await outputFileStream.FlushAsync();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
