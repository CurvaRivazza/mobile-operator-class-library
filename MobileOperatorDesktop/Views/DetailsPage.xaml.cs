using MobileOperatorClassLibrary.Entities;
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
    public partial class DetailsPage : UserControl
    {
        private List<Client> _clients = new();

        public DetailsPage()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _clients = App.ClientRepository.GetAll().ToList();
            ClientCombo.ItemsSource = _clients;

            FromDate.SelectedDate = DateTime.Now.AddDays(-7);
            ToDate.SelectedDate = DateTime.Now;
        }

        private void ClientCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClientCombo.SelectedItem is not Client client) return;

            var full = App.ClientRepository.GetById(client.Id);
            ServicesList.ItemsSource = full?.SubscribedServices;

            ShowDetails();
        }

        private void Show_Click(object sender, RoutedEventArgs e) => ShowDetails();

        private void ShowDetails()
        {
            if (ClientCombo.SelectedItem is not Client client) return;

            var from = FromDate.SelectedDate ?? DateTime.Now.AddDays(-7);
            var to = (ToDate.SelectedDate ?? DateTime.Now).AddDays(1).AddSeconds(-1);

            try
            {
                var details = App.ClientRepository
                    .GetDetailsByClient(client.Id, from, to)
                    .ToList();

                DetailsGrid.ItemsSource = details;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки детализации: {ex.Message}");
            }
        }

        private void AddCall_Click(object sender, RoutedEventArgs e)
        {
            if (ClientCombo.SelectedItem is not Client client)
            {
                MessageBox.Show("Выберите клиента.");
                return;
            }

            if (!int.TryParse(DurationBox.Text, out var seconds) || seconds <= 0)
            {
                MessageBox.Show("Некорректная длительность.");
                return;
            }

            var number = DestNumberBox.Text?.Trim();
            if (string.IsNullOrWhiteSpace(number))
            {
                MessageBox.Show("Введите номер.");
                return;
            }

            try
            {
                decimal cost = App.RateCalculator.CalculateCallCost(seconds, client.CurrentTariff);

                var detail = new CallDetail
                {
                    DateTime = DateTime.Now,
                    DurationSeconds = seconds,
                    DestinationNumber = number,
                    Cost = cost,
                    ClientOwner = client,
                    UsedTariff = client.CurrentTariff
                };

                App.ClientRepository.AddCallDetail(detail);

                client.Debit(cost);
                App.ClientRepository.Save(client);

                MessageBox.Show(
                    $"Звонок записан.\nСтоимость: {cost:C}\nНовый баланс: {client.Balance:C}",
                    "Готово");

                ShowDetails();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}
