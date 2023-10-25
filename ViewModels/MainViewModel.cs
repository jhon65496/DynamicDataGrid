using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using DynamicDataGrid.Base;
using DynamicDataGrid.Services;
using MathCore.WPF.Commands;
using MathCore.WPF.ViewModels;
using Ookii.Dialogs.Wpf;

namespace DynamicDataGrid.ViewModels;

[MarkupExtensionReturnType(typeof(MainViewModel))]
public class MainViewModel : TitledViewModel
{
    private readonly IFileService _fileService = new FileService();



    #region Files : AwesomeObservableCollection<DataViewModel> - list of files

    ///<summary>list of files</summary>
    private AwesomeObservableCollection<DataViewModel> _Files = new();

    ///<summary>list of files</summary>
    public AwesomeObservableCollection<DataViewModel> Files
    {
        get => _Files;
        set
        {
            if (Set(ref _Files, value))
            {
                _FilesCollectionViewSource = new CollectionViewSource()
                {
                    Source = value
                };
                _FilesCollectionViewSource.Filter += _FilesCollectionViewSource_Filter;
                _FilesCollectionViewSource.View?.Refresh();
                 // OnPropertyChanged(nameof(Files));
                    OnPropertyChanged(nameof(FilesCollectionView));
                 // RaisePropertyChanged(nameof(FilesCollectionView));
            }

        }
    }

    private void _FilesCollectionViewSource_Filter(object sender, FilterEventArgs e)
    {
        // if (!(e.Item is IndexProviderView indexProviderView)) return;
        var item = (DataViewModel)e.Item;
        e.Accepted = true;
        
        Debug.WriteLine($"_FilesCollectionViewSource_Filter  --  item.Title -- {item.Title}");
    }

    private CollectionViewSource _FilesCollectionViewSource;
    public ICollectionView FilesCollectionView => _FilesCollectionViewSource?.View;

    #endregion


    #region property SelectedFile : DataViewModel - Выбранный файл

    /// <Summary>Выбранный файл</Summary>
    private DataViewModel? _SelectedFile;

    /// <Summary>Выбранный файл</Summary>
    public DataViewModel? SelectedFile { get => _SelectedFile; set => Set(ref _SelectedFile, value); }

    #endregion

    private Command? _RemoveCommand;

    public ICommand RemoveCommand => _RemoveCommand ??= Command.New<DataViewModel>(v => _Files?.Remove(v!), v => _Files?.Contains(v!) ?? false);


    #region property NewFileName : string - Имя нового файла

    /// <Summary>Имя нового файла</Summary>
    private string _NewFileName = "New file 0";

    /// <Summary>Имя нового файла</Summary>
    public string NewFileName { get => _NewFileName; set => Set(ref _NewFileName!, value); }

    #endregion

    #region property NewFilePath : string - Путь нового файла

    /// <Summary>Путь нового файла</Summary>
    private string _NewFilePath = "c:\\123\\321.txt";

    /// <Summary>Путь нового файла</Summary>
    public string NewFilePath { get => _NewFilePath; set => Set(ref _NewFilePath!, value); }

    #endregion

    private Command? _AddCommand;

    public ICommand AddCommand => _AddCommand ??= Command.New(() => _Files?.Add(new(_NewFileName, _NewFilePath)));

    private Command? _FillGridCommand;

    public ICommand FillGridCommand => _FillGridCommand ??= Command.New(() => FillGrid());

    private void FillGrid()
    {
        ObservableCollection<DataViewModel> filesToAdd = new ObservableCollection<DataViewModel> {new("File 1", @"c:\data\data_file1.txt"),
            new("File 2", @"c:\data\d1\data_file2.txt"),
            new("File 3", @"c:\data\d2\d3\data_file3.txt"),
            new("File 4", @"c:\data\d3\d5\d7\d8\data_file4.txt") };

        _Files.AddItemsRange(filesToAdd);

        OnPropertyChanged(nameof(Files));

    }


    private Command? _ChangeRootFolderCommand;

    public ICommand ChangeRootFolderCommand => _ChangeRootFolderCommand ??= Command.New(() => ChangeRootFolder());

    private async void ChangeRootFolder()
    {
        var dialog = new VistaFolderBrowserDialog { Multiselect = false };
        if (!dialog.ShowDialog() == true)
            return;
        // Files = Files ?? new();
        Files = new AwesomeObservableCollection<DataViewModel>();
        // Files = new ObservableCollection<DataViewModel>();
        // 

        Files.Clear();

        Files.AddItemsRange(await _fileService.CreateItemList(dialog.SelectedPath));

        //  OnPropertyChanged(nameof(Files));
        // FilesCollectionView?.Refresh();
        // FilesCollectionView.View.Refresh();
        // _FilesCollectionViewSource.View.Refresh();
    }

    public MainViewModel() : base("The title")
    {
        //FillGrid();
    }
}
