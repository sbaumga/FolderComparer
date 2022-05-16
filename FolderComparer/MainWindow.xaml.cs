using FolderComparer.Business.Data;
using FolderComparer.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

namespace FolderComparer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IFileDataPersister FileDataPersister { get; }
        private ILocalFileLister FileLister { get; }
        private ISavedFileListSyncChecker SyncChecker { get; }
        private ISyncStatusCompiler SyncStatusCompiler { get; }

        public MainWindow(IFileDataPersister fileDataPersister, ILocalFileLister localFileLister, ISavedFileListSyncChecker syncChecker, ISyncStatusCompiler syncStatusCompiler)
        {
            FileDataPersister = fileDataPersister;
            FileLister = localFileLister;
            SyncChecker = syncChecker;
            SyncStatusCompiler = syncStatusCompiler;

            InitializeComponent();
        }

        private void ComparisonFolderDialogClick(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                var result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK || result == System.Windows.Forms.DialogResult.Yes)
                {
                    FolderTextBox.Text = dialog.SelectedPath;
                }
            }
        }

        private void SelectComparisonFileClick(object sender, RoutedEventArgs e)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.FileName = "Folder Contents.txt";
                dialog.AddExtension = true;
                dialog.Filter = "Text files (*.txt)|*.txt";
                var result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK || result == System.Windows.Forms.DialogResult.Yes)
                { 
                    ComparisonFileTextBox.Text = dialog.FileName;
                }
            }
        }

        private void GenerateComparisonFileClick(object sender, RoutedEventArgs e)
        {
            ComparisonFileGenerationSpinner.Visibility = Visibility.Visible;

            var folderPath = FolderTextBox.Text;
            if (!Directory.Exists(folderPath))
            {
                System.Windows.MessageBox.Show("The selected folder doesn't exist.", "Folder Does Not Exist", MessageBoxButton.OK);
                return;
            }

            var saveFilePath = ComparisonFileTextBox.Text;

            try
            {
                var files = FileLister.GetFileDataForFolder(folderPath);
                var fileData = new FilePersistanceData 
                {
                    BasePath = folderPath,
                    Files = files,
                };
                FileDataPersister.Save(saveFilePath, fileData);

                ShowOkMessageBox($"Folder contents saved to {saveFilePath}", "File Saved");
            } catch (Exception ex){
                DisplayException(ex);
            }
            finally
            {
                ComparisonFileGenerationSpinner.Visibility = Visibility.Collapsed;
            }

            return;
        }

        private void ShowOkMessageBox(string text, string title)
        {
            System.Windows.MessageBox.Show(text, title, MessageBoxButton.OK);
        }

        private void DisplayException(Exception ex)
        {
            System.Windows.MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK);
        }

        private void SelectOutputFileClick(object sender, RoutedEventArgs e)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.FileName = "Folder Comparison Result.txt";
                dialog.AddExtension = true;
                dialog.Filter = "Text files (*.txt)|*.txt";
                var result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK || result == System.Windows.Forms.DialogResult.Yes)
                {
                    OutputFileTextBox.Text = dialog.FileName;
                }
            }
        }

        private void GenerateOutputClicked(object sender, RoutedEventArgs e)
        {
            OutputFileGenerationSpinner.Visibility = Visibility.Visible;

            var folderPath = FolderTextBox.Text;
            if (!Directory.Exists(folderPath))
            {
                ShowOkMessageBox("The selected folder doesn't exist.", "Folder Does Not Exist");
                return;
            }

            var comparisonFilePath = ComparisonFileTextBox.Text;
            if (!File.Exists(comparisonFilePath))
            {
                ShowOkMessageBox("The selected comparison file doesn't exist.", "File Does Not Exist");
                return;
            }

            try
            {
                var syncStatus = SyncChecker.GetSynchronizationStatusForFiles(folderPath, comparisonFilePath);
                var syncOutputStrings = SyncStatusCompiler.CompileOutput(syncStatus);

                if (syncOutputStrings.Any())
                {
                    var outputFilePath = OutputFileTextBox.Text;
                    var combinedText = string.Join("\n", syncOutputStrings);
                    File.WriteAllText(outputFilePath, combinedText);

                    ShowOkMessageBox($"{syncOutputStrings.Count()} discrepancies found. Please check the output file for more details: {outputFilePath}", "Discrepancies Found");
                }
                else
                {
                    ShowOkMessageBox("No discrepancies found", "Success!");
                }
            } catch (Exception ex)
            {
                DisplayException(ex);
            }
            finally
            {
                OutputFileGenerationSpinner.Visibility = Visibility.Collapsed;
            }
        }
    }
}