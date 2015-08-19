using System;
using Gtk;

namespace WetterApp {

    public class App {

        public static void Main(string[] args) {
            Application.Init();

            SQLite database = new SQLite();

            MeteoData weather = new WeatherAPI();

            weather.addLocation("Berlin");
            weather.addLocation("Lisbon");
            weather.addLocation("Havana");
            weather.addLocation("Barcelona");
            weather.addLocation("Rio de Janeiro");
            weather.addLocation("Berlin");
            
            Input userInput = new UserInput(weather);

            Application.Run();
        }
    }
}
