using System;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;

namespace WetterApp {

    public class SQLite {

        bool foundDB = false;
        //string filename = "URI=file:WeatherDB.sqlite";
        string filename = "WeatherDB.sqlite";
        SqliteConnection database;
        SqliteCommand command;

        public SQLite() {
            connectDatabase();
            database.Open();
            command = database.CreateCommand();
            createTables();
            insert("Berlin", "2015-02-06", 3, 0, 5, 12, 233, 12, "NNW", "Fog");
        }

        ~SQLite() {
            database.Close();
        }

        private void connectDatabase() {
            foundDB = File.Exists(filename);
            if (!foundDB)
                SqliteConnection.CreateFile(filename);
            database = new SqliteConnection("Data Source=" + filename);
        }


        private void createTables() {

            if (!foundDB) {
                command.CommandText = "CREATE TABLE Locations (ID INTEGER PRIMARY KEY NOT NULL, Name TEXT NOT NULL);";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE Weather (ID INTEGER PRIMARY KEY NOT NULL," +
                                                            "Temperature INTEGER," +
                                                            "Max INTEGER," +
                                                            "Min INTEGER," +
                                                            "Humidity INTEGER," +
                                                            "Pressure INTEGER," +
                                                            "Windspeed INTEGER," +
                                                            "Winddir TEXT," +
                                                            "Description TEXT);";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE LocalWeather (ID INTEGER PRIMARY KEY NOT NULL," +
                                                                 "DateTime TEXT," +
                                                                 "LocationID INTEGER," +
                                                                 "WeatherID INTEGER," +
                                                                 "FOREIGN KEY (LocationID) REFERENCES Locations(ID)," +
                                                                 "FOREIGN KEY (WeatherID) REFERENCES Weather(ID));"; 
                command.ExecuteNonQuery();
            }
        }

        private void insert(String location, String datetime, int temperature, int max, int min,
                            int humidity, int pressure,
                            int windspeed, string winddir,
                            string description) {
            command.CommandText = "INSERT INTO Locations (Name) VALUES ('" + location + "');";
            command.ExecuteNonQuery();
            command.CommandText = "INSERT INTO Weather(Temperature, Max, Min, Humidity, Pressure, Windspeed, Winddir, Description)" +
                                  "VALUES (" + temperature + "," + max + "," + min
                                             + "," + humidity + "," + pressure
                                             + "," + windspeed
                                             + ", '" + winddir + "'"
                                             + ", '" + description + "');";
            command.ExecuteNonQuery();

            command.CommandText = "INSERT INTO LocalWeather (DateTime, LocationID, WeatherID) VALUES ('" + datetime + "'"
                                  + "," + 3 + "," + 4
                                  + "');";
            command.ExecuteNonQuery();

        }
    }
}
