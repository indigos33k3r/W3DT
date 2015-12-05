﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using W3DT.CASC;
using W3DT.Events;

namespace W3DT.Runners
{
    class RunnerInitializeCASC : RunnerBase
    {
        public override void Work()
        {
            EventManager.Trigger_CASCLoadStart();

            if (Program.CASCEngine != null)
            {
                Program.CASCEngine.Clear();
                Program.CASCEngine = null;
            }

            try
            {
                CASCEngine engine = Program.Settings.UseRemote ? CASCEngine.OpenOnlineStorage() : CASCEngine.OpenLocalStorage();
                RootHandler handler = engine.RootHandler;

                handler.LoadListFile(Constants.LIST_FILE);
                Program.Root = handler.SetFlags(LocaleFlags.All_WoW, ContentFlags.None);
                handler.MergeInstall(engine.Install);

                Program.CASCEngine = engine;

                EventManager.Trigger_LoadStepDone();
                Done(true);
            }
            catch (Exception ex)
            {
                Program.CASCEngine = null;
                Log.Write("Error prevented CASC engine loading: " + ex.Message);
                Log.Write(ex.StackTrace);

                Done(false);
            }
        }

        private void Done(bool success)
        {
            EventManager.Trigger_CASCLoadDone(new CASCLoadDoneArgs(success));
        }
    }
}
