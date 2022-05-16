using FolderComparer.Business.Implementations;
using FolderComparer.Business.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FolderComparer.Business.TypeMapping
{
    public class BusinessModule
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<ISerializer, JsonSerializerImp>();
            services.AddTransient<IFileDataCreator, FileDataCreatorImp>();
            services.AddTransient<ILocalFileLister, LocalFileListerImp>();
            services.AddTransient<IFileDataPersister, FileDataPersisterImp>();
            services.AddTransient<ISavedFileListSyncChecker, SavedFileListSyncCheckerImp>();
            services.AddTransient<ISyncStatusCompiler, SyncStatusCompilerImp>();
        }
    }
}