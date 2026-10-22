using MobileOperatorClassLibrary.Entities;
using MobileOperatorClassLibrary.EventArguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MobileOperatorDesktop.Views
{
    public partial class ClientsPage : UserControl
    {
        private List<Client> _clients = new();

        public ClientsPage()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadClients();
        }

        private void LoadClients()
        {
            try
            {
                foreach (var c in _clients)
                    Unsubscribe(c);

                _clients = App.ClientRepository.GetAll().ToList();

                foreach (var c in _clients)
                    Subscribe(c);

                ClientsGrid.ItemsSource = _clients;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки клиентов: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Subscribe(Client c)
        {
            c.OnBalanceLow += OnClientBalanceLow;
            c.OnBalanceZero += OnClientBalanceZero;
            c.OnBalanceChanged += OnClientBalanceChanged;
        }

        private void Unsubscribe(Client c)
        {
            c.OnBalanceLow -= OnClientBalanceLow;
            c.OnBalanceZero -= OnClientBalanceZero;
            c.OnBalanceChanged -= OnClientBalanceChanged;
        }

        private void OnClientBalanceLow(object sender, BalanceEventArgs e)
        {
            var client = (Client)sender;
            App.OfferGenerator.OnClientBalanceLow(sender, e);

            LogEvent($"[LOW] {client.FullName}: {e.Message}. Баланс: {e.CurrentBalance:C}");
            MessageBox.Show(
                $"Клиент {client.FullName}\n{e.Message}\nБаланс: {e.CurrentBalance:C}",
                "Низкий баланс", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void OnClientBalanceZero(object sender, BalanceEventArgs e)
        {
            var client = (Client)sender;
            App.OfferGenerator.OnClientBalanceZero(sender, e);

            LogEvent($"[ZERO] {client.FullName}: {e.Message}. Баланс: {e.CurrentBalance:C}");
            MessageBox.Show(
                $"Клиент {client.FullName}\n{e.Message}\nБаланс: {e.CurrentBalance:C}",
                "Баланс обнулён", MessageBoxButton.OK, MessageBoxImage.Stop);
        }

        private void OnClientBalanceChanged(object sender, BalanceEventArgs e)
        {
            var client = (Client)sender;
            LogEvent($"[CHANGED] {client.FullName}: баланс = {e.CurrentBalance:C}");
        }

        private void LogEvent(string text)
        {
            EventsLog.Items.Insert(0, $"{DateTime.Now:HH:mm:ss} — {text}");
        }

        private void Refresh_Click(object sender, RoutedEventArgs e) => LoadClients();

        private void TopUp_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsGrid.SelectedItem is not Client client)
            {
                MessageBox.Show("Выберите клиента.");
                return;
            }

            var input = Microsoft.VisualBasic.Interaction.InputBox(
                "Введите сумму пополнения:", "Пополнение", "500");

            if (!decimal.TryParse(input, out var amount) || amount <= 0)
            {
                MessageBox.Show("Некорректная сумма.");
                return;
            }

            try
            {
                client.TopUp(amount); 
                App.ClientRepository.Save(client);
                ClientsGrid.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void Debit_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsGrid.SelectedItem is not Client client)
            {
                MessageBox.Show("Выберите клиента.");
                return;
            }

            var input = Microsoft.VisualBasic.Interaction.InputBox(
                "Введите сумму списания:", "Списание", "100");

            if (!decimal.TryParse(input, out var amount) || amount <= 0)
            {
                MessageBox.Show("Некорректная сумма.");
                return;
            }

            try
            {
                client.Debit(amount);
                App.ClientRepository.Save(client);
                ClientsGrid.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void Predict_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsGrid.SelectedItem is not Client client)
            {
                MessageBox.Show("Выберите клиента.");
                return;
            }

            var date = App.BalancePredictor.PredictZeroDate(client, 7);
            MessageBox.Show(
                $"Прогноз обнуления баланса для {client.FullName}:\n{date:dd.MM.yyyy}",
                "Прогноз");
        }

        private void ShowOffers_Click(object sender, RoutedEventArgs e)
        {
            var offers = App.OfferRepository.GetAll().ToList();

            if (!offers.Any())
            {
                MessageBox.Show("Предложений пока нет.");
                return;
            }

            var text = string.Join("\n\n", offers.Select(o =>
                $"[{o.CreatedAt:dd.MM HH:mm}] Клиент #{o.ClientId} ({o.Type})\n{o.Text}"));

            MessageBox.Show(text, "Сгенерированные предложения");
        }

        public void ShowOffersByClient_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsGrid.SelectedItem is not Client client)
            {
                MessageBox.Show("Выберите клиента.");
                return;
            }

            var offers = App.OfferRepository.GetByClient(client.Id).ToList();

            if (!offers.Any())
            {
                MessageBox.Show($"Предложений для клиента {client.Id} пока нет.");
                return;
            }

            var text = string.Join("\n\n", offers.Select(o =>
                $"[{o.CreatedAt:dd.MM HH:mm}] Клиент #{o.ClientId} ({o.Type})\n{o.Text}"));

            MessageBox.Show(text, "Сгенерированные предложения");
        }
    }
}
