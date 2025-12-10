using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace Dolgozat
{
    public class DolgozatAdat
    {
        public string Nev { get; set; } = string.Empty;
        public int Eletkor { get; set; }
        public int Pontszam { get; set; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<DolgozatAdat> Dolgozatok { get; } = new();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            BetoltDolgozatok();
        }

        private void BetoltDolgozatok()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dolgozatok.txt");
            if (!File.Exists(filePath))
            {
                return;
            }

            var lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                // CSV Header
                if (i == 0 && lines[i].Contains(';', StringComparison.Ordinal))
                {
                    continue;
                }

                var line = lines[i];
                var parts = line.Split(';', StringSplitOptions.TrimEntries);
                if (parts.Length < 3)
                {
                    continue;
                }

                if (!int.TryParse(parts[1], out var eletkor))
                {
                    continue;
                }

                if (!int.TryParse(parts[2], out var pontszam))
                {
                    continue;
                }

                Dolgozatok.Add(new DolgozatAdat
                {
                    Nev = parts[0],
                    Eletkor = eletkor,
                    Pontszam = pontszam
                });
            }
        }

        private void Hozzaadas_Click(object sender, RoutedEventArgs e)
        {
            var nev = NevTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(nev))
            {
                return;
            }

            if (!int.TryParse(EletkorTextBox.Text, out var eletkor))
            {
                return;
            }

            if (!int.TryParse(PontszamTextBox.Text, out var pontszam))
            {
                return;
            }

            Dolgozatok.Add(new DolgozatAdat
            {
                Nev = nev,
                Eletkor = eletkor,
                Pontszam = pontszam
            });

            NevTextBox.Clear();
            EletkorTextBox.Clear();
            PontszamTextBox.Clear();
        }
    }
}