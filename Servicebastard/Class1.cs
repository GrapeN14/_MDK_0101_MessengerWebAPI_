using ModelsLibrary.Messages;
using Models;
using Newtonsoft.Json;
using System.Text;
using System.Windows;
 


namespace Servicebastard
{
    public class Service
    {
        public List<ModelsLibrary.Messages.MessageResponse> Messages
        { get; private set; }

        public Uri Uri { get; set; }

        public Service()
        {
            Messages = new();
            Uri = new("http://localhost:0/swagger/index.html");
        }


        public Service(Uri uri) : this()
        {
            this.Uri = Uri;
        }
        public Service(string IP, int port) : this()
        {
            SetNewAddress(IP, port);
        }




        public void SetNewAddress(string IP, int port)
        {
            this.Uri = new Uri($"http://{IP}:{port}/api/messages");
        }
        public void SetNewAddress(string IP, string port)
        {
            this.Uri = new Uri($"http://{IP}:{port}/api/messages");
        }
        public void SetNewAddress(Uri uri)
        {
            this.Uri = uri;
        }




        
        public async Task PostAsync(ModelsLibrary.Messages.MessageRequest message, Uri uri)
        {
            HttpClient client = new HttpClient();
            StringContent content = new StringContent
            (
                JsonConvert.SerializeObject(message),
                Encoding.UTF8,
                "application/json"
            );


            try
            {
                HttpResponseMessage response = await client.PostAsync(uri, content);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ResetColor();
                throw;
            }
        }
        public async Task PostAsyncDesktop(ModelsLibrary.Messages.MessageRequest message, Uri uri)
        {
            HttpClient client = new HttpClient();
            StringContent content = new StringContent
            (
                JsonConvert.SerializeObject(message),
                Encoding.UTF8,
                "application/json"
            );


            try
            {
                HttpResponseMessage response = await client.PostAsync(uri, content);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Ошибка сервера\nПопробуйте обратиться к администратору", "Плохой запрос на сервер", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public async Task GetAsyncDesktop(Uri uri)
        {
            await Task.Run(async () =>
            {
                HttpClient client = new HttpClient();
                try
                {
                    HttpResponseMessage response = await client.GetAsync(uri);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    try
                    {
                        Messages = JsonConvert.DeserializeObject<List<MessageResponse>>(responseBody);
                        if (Messages is null || Messages.Count == 0)
                        {
                            MessageBox.Show("Не удалось содать список сообщений", "Ошибка расшифровки", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (HttpRequestException e)
                {
                    MessageBox.Show("Ошибка сервера\nПопробуйте обратиться к администратору", "Плохой запрос на сервер", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }








       
        public async void PostAsync(ModelsLibrary.Messages.MessageRequest message)
        {
            await PostAsync(message, this.Uri);
        }
        public async void PostAsyncDesktop(ModelsLibrary.Messages.MessageRequest message)
        {
            await PostAsyncDesktop(message, this.Uri);
        }



       
        public async void GetAsyncDesktop()
        {
            await GetAsyncDesktop(this.Uri);
        }
    }
}