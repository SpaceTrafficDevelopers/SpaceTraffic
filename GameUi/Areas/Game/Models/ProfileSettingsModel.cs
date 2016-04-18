/**
Copyright 2010 FAV ZCU

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

**/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace SpaceTraffic.GameUi.Areas.Game.Models
{
    public class ProfileSettingsModel
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Zůstat přihlášen:")]
        public bool RememberMe { get; set; }

        [Display(Name = "Zasílat info ze hry:")]
        public bool SendGameInfo { get; set; }

        [Display(Name = "Zasílat novinky:")]
        public bool SendNews { get; set; }

        [Display(Name = "Počet zobrazených událostí:")]
        public int CountOfEvents{ get; set; }

        [Display(Name = "Počet zobrazených zpráv:")]
        public int CountOfMessages { get; set; }
    }
}