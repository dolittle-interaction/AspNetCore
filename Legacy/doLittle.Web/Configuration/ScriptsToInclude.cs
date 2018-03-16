/*---------------------------------------------------------------------------------------------
 *  Copyright (c) 2008-2017 Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
namespace Dolittle.Web.Configuration
{
    public class ScriptsToInclude
    {
        public bool JQuery { get; set; }
        public bool Knockout { get; set; }
        public bool JQueryHistory { get; set; }
        public bool Require { get; set; }
        public bool Dolittle { get; set; }

        public bool SignalR { get; set; }

        public ScriptsToInclude()
        {
            JQuery = true;
            JQueryHistory = true;
            Knockout = true;
            Require = true;
            Dolittle = true;
            SignalR = true;
        }

        public void ExcludeAllScripts()
        {
            JQuery = false;
            JQueryHistory = false;
            Knockout = false;
            Require = false;
            Dolittle = false;
            SignalR = false;
        }
    }
}
