using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Xml;
using Gtk;
using Glade;

namespace WetterApp {

    public interface WeatherObserver{
        void updateWeatherReport();
    }

    public class WeatherReport : WeatherObserver {
        
        MeteoData meteoData;
        Input userInput;

        [Widget]
        ComboBox location = ComboBox.NewText();

        [Widget]
        Label datetime = null, description = null, temperature = null,
              humidity = null, pressure = null, windspeed = null, winddir = null,
              day1 = null, day2 = null, day3 = null, day4 = null,
              temperature1 = null, temperature2 = null, temperature3 = null, temperature4 = null;

        [Widget]
        Image icon = null, icon1 = null, icon2 = null, icon3 = null, icon4 = null;

        public WeatherReport(Input userInput, MeteoData meteoData) {
            this.userInput = userInput;
            this.meteoData = meteoData;
            this.meteoData.addObserver(this);
            showWeatherReport();
        } 

        private void showWeatherReport() {
            createGUI();
            meteoData.setLocation(0);
            location.Active = meteoData.getLocation();
        }

        private void createGUI() {
            Glade.XML gui = new Glade.XML(null, "WeatherReport.xml", "Weather Report", null);
            gui.Autoconnect(this);

            int i = 0;
            while (meteoData.locationName(i) != null) {
                location.AppendText(meteoData.locationName(i));
                i++;
            }
        }

        public void updateWeatherReport() {
            Weather weather = meteoData.weather();
            datetime.Text = weather.datetime;
            description.Text = weather.description;
            temperature.Text = weather.temperature;
            humidity.Text = weather.humidity;
            pressure.Text = weather.pressure;
            windspeed.Text = weather.windspeed;
            winddir.Text = weather.winddir;

            WebRequest request = WebRequest.Create(weather.icon);
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();            
            icon.Pixbuf = new Gdk.Pixbuf(stream);

            day1.Text = weather.days[0];
            day2.Text = weather.days[1];
            day3.Text = weather.days[2];
            day4.Text = weather.days[3];

            temperature1.Text = weather.temperatures[0];            
            temperature2.Text = weather.temperatures[1];            
            temperature3.Text = weather.temperatures[2];            
            temperature4.Text = weather.temperatures[3];            

            request = WebRequest.Create(weather.icons[0]);
            response = request.GetResponse();
            stream = response.GetResponseStream();
            icon1.Pixbuf = new Gdk.Pixbuf(stream);

            request = WebRequest.Create(weather.icons[1]);
            response = request.GetResponse();
            stream = response.GetResponseStream();
            icon2.Pixbuf = new Gdk.Pixbuf(stream);

            request = WebRequest.Create(weather.icons[2]);
            response = request.GetResponse();
            stream = response.GetResponseStream();
            icon3.Pixbuf = new Gdk.Pixbuf(stream);

            request = WebRequest.Create(weather.icons[3]);
            response = request.GetResponse();
            stream = response.GetResponseStream();
            icon4.Pixbuf = new Gdk.Pixbuf(stream);
        }
        
        void on_location_changed(object o, EventArgs args) {
            userInput.setLocation(location.Active);
            //updateWeather();
        }
    } 
}
