using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace FibonachiSequence
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, что введенное значение числа в корректном диапазоне
            if (int.TryParse(NumberTextBox.Text, out int number) && number >= 1 && number <= 10)
            {
                int[] fibSequence = new int[number];
                if (number == 1)
                {
                    FibonacciTextBox.Text = string.Join(", ", fibSequence);
                    SumTextBox.Text = fibSequence.Sum().ToString();
                    FibonacciTextBox.IsReadOnly = true;
                    SumTextBox.IsReadOnly = true;
                    return;
                }
                // Вычисляем последовательность Фибоначчи

                fibSequence[0] = 0;
                fibSequence[1] = 1;
                for (int i = 2; i < number; i++)
                {
                    fibSequence[i] = fibSequence[i - 1] + fibSequence[i - 2];
                }

                // Заполняем поля результатами вычислений и блокируем их для редактирования
                FibonacciTextBox.Text = string.Join(", ", fibSequence);
                SumTextBox.Text = fibSequence.Sum().ToString();
                FibonacciTextBox.IsReadOnly = true;
                SumTextBox.IsReadOnly = true;
            }
            else
            {
                MessageBox.Show("Ошибка! Введите число от 1 до 10.");
            }
        }


        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем текст из первого текстового поля
            string inputText = InputTextBox.Text;

            if (CheckText.IsChecked == true)
            {
                // Определяем длину слов, которые необходимо фильтровать
                if (!int.TryParse(LengthTextBox.Text, out int filterLength))
                {
                    MessageBox.Show("Ошибка! Некорректное значение длины слова.");
                    return;
                }
                string[] str = ФильтрацияТекста.ОтобратьСлова(inputText, filterLength);
                // Заполняем второе текстовое поле отфильтрованными словами
                FilteredTextBox.Text = string.Join(", ", str);
            }
            if (CheckEmail.IsChecked == true)
            {
                string[] str = ФильтрацияТекста.ОтобратьEmail(inputText);
                FilteredTextBox.Text = string.Join("; ", str);
            }
        }
        async private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (FilteredTextBox.Text != "")
            {
                string str = FilteredTextBox.Text;

                await ФильтрацияТекста.SaveToFile("123.txt", str);
            }
        }
    }
    public static class ФильтрацияТекста
    {
        // Используем регулярное выражение для удаления знаков препинания и пробелов
        private const string ЗнакиПрипинания = "\\p{P}|\\s+";
        public static string[] ОтобратьСлова(string inputText, int filterLength)
        {
            string[] words = Regex.Split(inputText, ЗнакиПрипинания);

            // Фильтруем слова, длина которых превышает заданное значение
            var filteredWords = words.Where(x => x.Length >= filterLength).ToArray();

            return filteredWords;
        }
        private const string Email = @"(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                  + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                  + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                  + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})";

        public static string[] ОтобратьEmail(string inputText)
        {
            string[] arr = Regex.Matches(inputText, Email).Cast<Match>().Select(x => x.Value).ToArray();
            var str = arr.Select(x => x.ToString()).ToArray();
            return arr;
        }
        async public static Task SaveToFile(string path, string text)
        {
            using StreamWriter writer = new(path, false);
            await writer.WriteLineAsync(text);
        }
    }
}