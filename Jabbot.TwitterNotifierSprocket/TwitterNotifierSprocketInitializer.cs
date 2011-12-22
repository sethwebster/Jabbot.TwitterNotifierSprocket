using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jabbot.Sprockets;
using Jabbot.TwitterNotifierSprocket.Models;
using Jabbot.TwitterNotifierSprocket.Migrations;
using System.Data.Entity.Migrations;
using Jabbot.Sprockets.Core;

namespace Jabbot.TwitterNotifierSprocket
{
    public class TwitterNotifierSprocketInitializer : ISprocketInitializer
    {
        private ITwitterNotifierSprocketRepository _database;
        private const string SqlClient = "System.Data.SqlClient";

        public TwitterNotifierSprocketInitializer()
            : this(new TwitterNotifierSprocketRepository())
        {

        }

        public TwitterNotifierSprocketInitializer(ITwitterNotifierSprocketRepository database)
        {
            this._database = database;
        }


        public void Initialize(Bot bot)
        {
            DoMigrations();

            if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["Application.Gravatar"]))
            {
                bot.Gravatar(System.Configuration.ConfigurationManager.AppSettings["Application.Gravatar"]);
            }
            foreach (var r in this._database.OccupiedRooms)
            {
                bot.Join(r.Name);
            }
        }


        private static void DoMigrations()
        {
            // Get the Jabbr connection string
            //var connectionString = ConfigurationManager.ConnectionStrings["Jabbr"];

            //if (String.IsNullOrEmpty(connectionString.ProviderName) ||
            //    !connectionString.ProviderName.Equals(SqlClient, StringComparison.OrdinalIgnoreCase))
            //{
            //    return;
            //}

            // Only run migrations for SQL server (Sql ce not supported as yet)
            var settings = new Configuration();
            var migrator = new DbMigrator(settings);
            migrator.Update();
        }

    }
}
