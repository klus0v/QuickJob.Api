// using QuickJob.DataModel.Configuration;
// using IConfigurationProvider = Vostok.Configuration.Abstractions.IConfigurationProvider;
//
// namespace QuickJob.Api.DI;
//
// internal class UsersClientFactory
// {
//     private readonly IConfigurationProvider configuration;
//
//     public UsersClientFactory(IConfigurationProvider configuration) => 
//         this.configuration = configuration;
//
//     public IUsersClient GetClient()
//     {
//         var storageSettings = configuration.Get<ServiceSettings>();
//         return new UsersClient()
//         {
//
//         };
//     }
// }