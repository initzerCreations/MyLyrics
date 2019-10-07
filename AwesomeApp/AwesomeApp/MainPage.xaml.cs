using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using Xamarin.Essentials;

namespace AwesomeApp
{

    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        
        public MainPage()
        {
            InitializeComponent();
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            getLyrics();
        }
        void Button_Clicked(object sender, System.EventArgs e)
        {
            Label l = (Label)sl.Children[3];
            string s = l.Text; //get the lyrics text
            _ = ShareText(s); //using discard operation
        }
        public void getLyrics()
        {
            string sURL;
            sURL = "https://api.lyrics.ovh/v1/";
            //Get the request parameters
            Entry artistName = (Entry)sl.Children[5];
            Entry songName = (Entry)sl.Children[6];
            sURL = sURL + artistName.Text + "/" + songName.Text;
            WebRequest wrGETURL = WebRequest.Create(sURL);
            Stream objStream = null;
            Label lyLabel = (Label)sl.Children[3];

            try
            {
                objStream = wrGETURL.GetResponse().GetResponseStream();
            } catch(WebException e)
            {
                lyLabel.Text = "Sorry, could not find the lyrics :( \n" + "Check the names are correctly written";
                Console.WriteLine(e);
            }
            string lyrics = @"";
            if (objStream != null)
            {
                using (StreamReader objReader = new StreamReader(objStream))
                {
                    //Get the lyrics
                    string sLine = "";

                    while (sLine != null)
                    {
                        sLine = objReader.ReadLine();
                        if (sLine != null)
                            lyrics += sLine;
                    }

                    //Parse json
                    var lyricsParsed = JObject.Parse(lyrics);

                    
                    //Write lyrics
                    lyLabel.Text = (string)lyricsParsed["lyrics"];
                }
            }
        }
        public async Task ShareText(string text)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = text,
                Title = "Share this beautiful lyrics :)"
            });
        }
    }
}
