using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvesAuto.ApiTest.ApiService
{
    public class GeneralInfraApiDTO
    {
    }
    public class NewsapiOutputDto
    {
        public List<Article> Articles { get; set; }

    }
    public class Article
    {
        public string title { get; set; }
        public string description { get; set; }
        public string content { get; set; }
        public string publishedAt { get; set; }
    }
}
