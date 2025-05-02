using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Net.Http;
using Newtonsoft.Json;

namespace file_manager
{
    public partial class GoBack2 : Form
    {

        public GoBack2()
        {
            InitializeComponent();
            InitializeContextMenu();
        }

        public void InitializeContextMenu()
        {

            ContextMenuStrip contextMenu = new ContextMenuStrip();

        }
        public async void GoOver_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Кнопка нажата");

            string language = comboBox1.Text.Trim();
            if (string.IsNullOrWhiteSpace(language))
            {
                MessageBox.Show("Выберите язык программирования.");
                return;
            }

            listViewBooks.Items.Clear();
            if (!int.TryParse(textBox1.Text, out int pageCount) || pageCount <= 0)
            {
                MessageBox.Show("Введите корректное количество страниц.");
                return;
            }

            await SearchBooks(language, pageCount);
        }


        private void GoBack2_Load(object sender, EventArgs e)
        {
            comboBox1.Items.AddRange(new string[]
        {
            "Python", "Java", "C#", "JavaScript", "C++", "Go", "Ruby", "PHP", "Rust"
        });
            comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;

        }
        public async Task SearchBooks(string language, int pageCount)
        {
            string query = Uri.EscapeDataString(language);

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Заголовки, чтобы прикинуться браузером
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36");
                    client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8");
                    client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
                    client.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.5,en;q=0.3");

                    listViewBooks.Items.Clear();
                    Random random = new Random();

                    for (int page = 1; page <= pageCount; page++)
                    {
                        string url = $"https://www.amazon.com/s?k={query}&i=stripbooks-intl-ship&page={page}";

                        await Task.Delay(random.Next(2000, 5000)); // случайная задержка 2-5 сек

                        var response = await client.GetAsync(url);
                        if (!response.IsSuccessStatusCode)
                        {
                            MessageBox.Show($"Ошибка при загрузке страницы {page}: {response.StatusCode}");
                            continue;
                        }

                        string html = await response.Content.ReadAsStringAsync();

                        // Проверка на блокировку или пустую страницу
                        if (string.IsNullOrEmpty(html) || !html.Contains("s-pagination-next"))
                        {
                            MessageBox.Show($"Страница {page} не содержит ожидаемых данных. Возможно, Amazon что-то изменил.");
                            continue;
                        }

                        // Парсинг
                        var titles = Regex.Matches(html, @"<span class=""a-size-medium a-color-base a-text-normal"">(.*?)</span>");
                        var authors = Regex.Matches(html, @"<div class=""a-row a-size-base a-color-secondary"">\s*<span.*?>by\s*(.*?)</span>");

                        for (int i = 0; i < titles.Count; i++)
                        {
                            string title = WebUtility.HtmlDecode(titles[i].Groups[1].Value);
                            string author = authors.Count > i ? WebUtility.HtmlDecode(authors[i].Groups[1].Value) : "Неизвестен";

                            var item = new ListViewItem(new[] { title, author });
                            listViewBooks.Items.Add(item);
                        }
                    }

                    if (listViewBooks.Items.Count == 0)
                    {
                        MessageBox.Show("Не удалось найти книги. Страницы могли измениться.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при получении данных: {ex.Message}");
                }
            }
        }


        class BookInfo
        {
            public string Title { get; set; }
            public string Author { get; set; }
            public string Rating { get; set; }
            public string Year { get; set; }
            public string Url { get; set; }
        }

        private void listViewBooks_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listViewBooks_ItemActivate(object sender, EventArgs e)
        {
            if (listViewBooks.SelectedItems.Count > 0)
            {
                string url = listViewBooks.SelectedItems[0].Tag.ToString();
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
        }

        public class OpenLibraryResponse
        {
            public List<Doc> Docs { get; set; }
        }

        public class Doc
        {
            public string Title { get; set; }
            public List<string> AuthorName { get; set; }
            public int? FirstPublishYear { get; set; }
            public string Key { get; set; }
        }
    }
}

