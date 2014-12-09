using System;

namespace QStudio {
    public partial class App
    {
        public App()
        {
            AppDomain.CurrentDomain.AppendPrivatePath(@"core");
            AppDomain.CurrentDomain.AppendPrivatePath(@"vendors");
        }
    }
}
