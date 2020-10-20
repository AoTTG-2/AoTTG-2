using System;

namespace Assets.Scripts.UI.CommandLine
{
    public class ConsoleCommands
    {
        public static void SwitchDebugLevel(Command command)
        {
            EMCli.SwitchDebugLevel((DebugLevel)command.LastArgAsByte);
            EMCli.RefreshLayout();
        }

        public static void FastLoadAndSpawn(Command command)
        {
            FengGameManagerMKII.instance.StartCoroutine(EMCli.ConnectAndJoinIE(true));
        }

        public static void ClearCommandsHistory(Command command)
        {
            InputLine.ClearInputs();
            "Commands history has been cleaned!".SendProcessing(true);
        }

        public static void ClearCLIMessages(Command command)
        {
            EMCli.ClearMessages();
            EMCli.RefreshLayout();
        }

        public static void Spawn(Command command)
        {
            throw new NotImplementedException();
        }

        public static void TestConnect(Command command)
        {
            FengGameManagerMKII.instance.StartCoroutine(EMCli.ConnectAndJoinIE(false));
        }

        public static void PrintMessage(Command command)
        {
            //You can access all arguments from Command as well as description and some other info
            $"Info: {command.Format} - \n description: {command.Description} \n Args count: {command.LastArgsAsString.Length}".SendProcessing(true);
        }

        public static void Info(Command command)
        {
            string.Concat("Welcome! \n \n" +
                          "1) Press Enter to enter the command you are typing. The window will be automatically unfocused. \n" +
                          "2) Press RightAlt to focus/unfocus the console. You are able to type there right away. \n" +
                          "3) Press F3 to hide/unhide the console. \n" +
                          "4) Press UpArrow/DownArrow to access what you already entered. \n" +
                          "5) Start typing and you will see suggestions on the right. Press RightCtrl to switch to them. Press it again to switch back. \n" +
                          "6) /help for more info").SendProcessing(true);
        }

        public static void GetInfoAboutCommand(Command command)
        {
            string startWith = string.Empty;
            if(command.LastArgsAsString.Length!=0) startWith = command.LastArgsAsString[0];

            string.Empty.SendCli();

            foreach(Command command1 in CommandHandler.Instance.Commands)
            {
                if (command1.Name.Equals(startWith))
                {
                    $"Name: [{command1.Name }] \n Description: [{command1.Description}] \n Format: [{command1.Format}]".SendProcessing(true);
                }
            }
        }
    }
}
