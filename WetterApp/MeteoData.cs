using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Xml;

namespace WetterApp {

    public interface MeteoData {
        void addObserver(WeatherObserver observer);
        void removeObserver(WeatherObserver observer);
        void addLocation(string location);
        void setLocation(int location);
        int getLocation();
        string locationName(int location);
        Weather weather();
        //void updateMeteoData();
    }

    public class WeatherAPI : MeteoData {
        
        private string key = "7e3nxzef2tncaqk2vqu2tqgw";
        private string url = "http://api.worldweatheronline.com/free/v1/weather.ashx?format=xml&";

        List<WeatherObserver> observerList = new List<WeatherObserver>();
        List<XmlNodeList> weatherList = new List<XmlNodeList>();
        int currentLocation = 0;
        int number_of_days;

        public void addObserver(WeatherObserver observer) {
            observerList.Add(observer);
        }

        public void removeObserver(WeatherObserver observer) {
            observerList.Remove(observer);
        }

        private void updateObserver() {
            for (int i = 0; i < observerList.Count; i++) {
                WeatherObserver observer = observerList[i];
                observer.updateWeatherReport();
            }
        }

        public void addLocation(string location) {
            XmlNodeList weatherReport = fetchWeather(location, 5).SelectNodes("data");

            if (weatherList.Count <= 0) {
                weatherList.Add(weatherReport);
                return;
            }

            string localWeather = weatherReport.Item(0).ChildNodes[0].ChildNodes[1].InnerText;
            for (int i = 0; i < weatherList.Count; i++) {
                string weatherInList = weatherList[i].Item(0).ChildNodes[0].ChildNodes[1].InnerText;
                if (localWeather == weatherInList) {
                    return;
                }
                else {
                    weatherList.Add(weatherReport);
                    return;
                }
            }
        }

        public void setLocation(int location) {
            currentLocation = location;
            updateObserver();
        }

        public string locationName(int location) {
            if (location < weatherList.Count)
                return weatherList[location].Item(0).ChildNodes[0].ChildNodes[1].InnerText;
            else
                return null;
        }

        public int getLocation() {
            return currentLocation;
        }
/*
        public void updateMeteoData() {
            for (int i = 0; i < locations.Count; i++) {
                XmlNodeList weather = fetchWeather(locations[i].ToString(), 5).SelectNodes("data");
                weatherReports.Add(weather);
                locations[i] = weather.Item(0).ChildNodes[0].ChildNodes[1].InnerText;
            }
        }
*/

        private XmlDocument fetchWeather(string location, int days) {
            HttpWebRequest request;
            HttpWebResponse response = null;
            XmlDocument document = null;
            try {
                request = (HttpWebRequest)WebRequest.Create(string.Format(url +
                                                                          "q=" + location +
                                                                          "&num_of_days=" + days +
                                                                          "&key=" + key));
                request.UserAgent = @"Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.8.1.4) Gecko/20070515 Firefox/2.0.0.4";
                response = (HttpWebResponse)request.GetResponse();
                document = new XmlDocument();
                document.Load(response.GetResponseStream());
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            response.Close();
            number_of_days = days;
            return document;
        }

        public Weather weather() {
            Weather currentWeather = new Weather();
            currentWeather.datetime = weatherList[currentLocation].Item(0).ChildNodes[2].ChildNodes[0].InnerText + ", " +
                                      weatherList[currentLocation].Item(0).ChildNodes[1].ChildNodes[0].InnerText;
            currentWeather.description = weatherList[currentLocation].Item(0).ChildNodes[1].ChildNodes[5].InnerText;
            currentWeather.temperature = weatherList[currentLocation].Item(0).ChildNodes[1].ChildNodes[1].InnerText + " °C";
            currentWeather.humidity = "Humidity: " + weatherList[currentLocation].Item(0).ChildNodes[1].ChildNodes[11].InnerText + " %";
            currentWeather.pressure = "Pressure: " + weatherList[currentLocation].Item(0).ChildNodes[1].ChildNodes[13].InnerText + " mb";
            currentWeather.windspeed = "Wind speed: " + weatherList[currentLocation].Item(0).ChildNodes[1].ChildNodes[7].InnerText + " km/h";
            currentWeather.winddir =  "Wind direction: " + weatherList[currentLocation].Item(0).ChildNodes[1].ChildNodes[9].InnerText;
            currentWeather.icon = weatherList[currentLocation].Item(0).ChildNodes[1].ChildNodes[4].InnerText;

            for (int i = 0; i < number_of_days-1; i++) {
                currentWeather.days[i] = weatherList[currentLocation].Item(0).ChildNodes[i+3].ChildNodes[0].InnerText;
                currentWeather.temperatures[i] = weatherList[currentLocation].Item(0).ChildNodes[i+3].ChildNodes[1].InnerText + "° | " +
                                                 weatherList[currentLocation].Item(0).ChildNodes[i+3].ChildNodes[3].InnerText + "°";
                currentWeather.icons[i] = weatherList[currentLocation].Item(0).ChildNodes[i+3].ChildNodes[11].InnerText;
            }
            return currentWeather;
        }

    }

    public class Weather {
        public string location;
        public string datetime;
        public string temperature;
        public string description;
        public string humidity;
        public string pressure;
        public string windspeed;
        public string winddir;
        public string icon;
        public string[] days = new string[4];
        public string[] temperatures = new string[4];
        public string[] icons = new string[4];
    }

}
