using AutoUpdaterDotNET;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoUpdateTester
{
    class Updator
    {
        public Updator()
        {
            AutoUpdater.ParseUpdateInfoEvent += AutoUpdaterOnParseUpdateInfoEvent;

            AutoUpdater.Start("https://washiil.github.io/jsonstore/data/autoupdators/elocheck.json");
        }

        void AutoUpdaterOnParseUpdateInfoEvent(ParseUpdateInfoEventArgs args)
        {
            dynamic json = JsonConvert.DeserializeObject(args.RemoteData);
            args.UpdateInfo = new UpdateInfoEventArgs
            {
                CurrentVersion = json["version"],
                ChangelogURL = json["changelog"],
                DownloadURL = json["url"],
                Mandatory = new Mandatory
                {
                    Value = json["mandatory"]["value"],
                    UpdateMode = json["mandatory"]["UpdateMode"],
                    MinimumVersion = json["mandatory"]["MinimumVersion"]
                }
            };

        }
    }
}
