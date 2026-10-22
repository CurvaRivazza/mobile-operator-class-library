using Microsoft.Extensions.Configuration;
using MobileOperatorClassLibrary.Data;
using MobileOperatorClassLibrary.Domain;
using MobileOperatorClassLibrary.Interfaces;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace MobileOperatorDesktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string AppSettingsPath = "appsettings.json";

        public static string ConnectionString { get; private set; }
        public static IClientRepository ClientRepository { get; private set; }
        public static OfferRepository OfferRepository { get; private set; }
        public static BalancePredictor BalancePredictor { get; private set; }
        public static OfferGenerator OfferGenerator { get; private set; }
        public static IRateCalculator RateCalculator { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(AppSettingsPath, optional: false, reloadOnChange: true);

            IConfiguration configuration = builder.Build();

            ConnectionString = configuration.GetConnectionString("MobileOperatorDb");

            ClientRepository = new ClientRepository(ConnectionString);
            OfferRepository = new OfferRepository(ConnectionString);
            BalancePredictor = new BalancePredictor(ClientRepository);
            OfferGenerator = new OfferGenerator(OfferRepository);
            RateCalculator = new StandardRateCalculator();
        }
    }

}
