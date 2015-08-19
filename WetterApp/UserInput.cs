using System;
using System.Collections;

namespace WetterApp {

    public interface Input {
        void setLocation(int location);
        //void updateWeather();
    }

    public class UserInput : Input {

        MeteoData meteoData;
        WeatherReport weatherReport;

        public UserInput(MeteoData meteoData) {
            this.meteoData = meteoData;
            weatherReport = new WeatherReport(this, meteoData);
        }

        public void setLocation(int location) {
            meteoData.setLocation(location);
        }
    
        //public void updateWeather() {}
    }

}
