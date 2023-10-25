using DynamicDataGrid.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamicDataGrid.Services
{
    internal interface IFileService
    {
        Task<IEnumerable<DataViewModel>> CreateItemList(string root);
    }
}