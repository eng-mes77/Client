using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace client
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private async void btnRequest_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {

                    client.Timeout = new TimeSpan(00, 00, 10);
                    string baseAddress = "http://localhost:5000";
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                    Uri url = new Uri(baseAddress + "/Pesron");
                    var ub = new UriBuilder(url);

                    string xml;
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(Phone));
                    using (var textWriter = new Utf8StringWriter())
                    {
                        xmlSerializer.Serialize(textWriter, new Phone() { phoneNumber = txtPhoneNumber.Text.Trim() });
                        xml = textWriter.ToString();
                    }

                    string json = JsonConvert.SerializeObject(new { phoneNumber = "09370863373" }, Formatting.Indented);
                    var content = new StringContent(xml, Encoding.UTF8, "application/xml");


                    HttpResponseMessage response = await client.PostAsync(ub.Uri, content);

                    string result = response.Content.ReadAsStringAsync().Result;

                    var serializer = new XmlSerializer(typeof(Person));
                    Person pers;

                    using (TextReader reader = new StringReader(result))
                    {
                        pers = (Person)serializer.Deserialize(reader);
                    }
                    lblName.Text = pers.Name;
                    lblAddress.Text = pers.Address;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
