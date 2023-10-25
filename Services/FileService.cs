using DynamicDataGrid.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicDataGrid.Services
{
    internal class FileService : IFileService
    {
        public async Task<IEnumerable<DataViewModel>> CreateItemList(string root)
        {
            var list = new List<DataViewModel>();
            await ConvertToList(root, list);
            return list;
        }
        public async Task ConvertToList(string root, IList<DataViewModel> fileList)
        {
            var rootDirectory = new DirectoryInfo(root);
            foreach (var dir in rootDirectory.EnumerateDirectories())
            {
                fileList.Add(new DataViewModel(dir.Name, dir.FullName));
                await ConvertToList(dir.FullName, fileList);
            }

            fileList.AddItems(rootDirectory.EnumerateFiles().Select(f => new DataViewModel(f.Name, f.FullName)).ToList<DataViewModel>());
        }
    }
}
