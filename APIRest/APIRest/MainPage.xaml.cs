using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace APIRest
{
    public partial class MainPage : ContentPage
    {
        private string Url = "https://jsonplaceholder.typicode.com/posts";
        ObservableCollection<Post> posts;
        //1 - Criar o client
        private HttpClient client = new HttpClient();

        public MainPage()
        {    
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            
            //2 - Obter o conteúdo em forma de string
            var conteudo = await client.GetStringAsync(Url);
            //3 - Deserializar a string
            var listaPosts = JsonConvert.DeserializeObject<List<Post>>(conteudo);
            posts = new ObservableCollection<Post>(listaPosts);

            ListaPosts.ItemsSource = posts;

            base.OnAppearing();
        }

        private async void Add(object sender, EventArgs e)
        {
            var post = new Post() { Title = "Idílio", Body = "Exemplo de Post" + DateTime.Now.Ticks };
            var json_post = JsonConvert.SerializeObject(post);
            await client.PostAsync(Url, new StringContent(json_post));

            posts.Insert(0, post);
        }

        private async void Update(object sender, EventArgs e)
        {
            var post = posts[0];
            post.Title += "Idílio Casimiro";
            var json_post = JsonConvert.SerializeObject(post);
            await client.PutAsync(Url + "/" + post.ID, new StringContent(json_post));
        }

        private async void Delete(object sender, EventArgs e)
        {
            var post = posts[0];
            await client.DeleteAsync(Url + "/" + post.ID);
            posts.Remove(post);
        }
    }

    public class Post
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
