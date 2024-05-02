using MinorShift.Emuera.GameProc.PluginSystem;
using System.Windows;

//Plugin must be a part of Emuera solution to be able to reference it
namespace EmueraPluginExample
{
    //class MUST be named PluginManifest
    public class PluginManifest : PluginManifestAbstract
    {
        public PluginManifest() { 
            //Add all Methods
            methods.Add(new HelloWorldMethod());
            methods.Add(new ParametersAndReferencesMethod());
            methods.Add(new ERBExecutionExampleMethod());
        }

        public override string PluginName => "Example Plugin";

        public override string PluginDescription => @"
Example Emuera Plugin
ERB code for test purposes:

#DIMS OUT_VAR_TEST = """"
CALLSHARP HelloWorld()
CALLSHARP ParametersAndReferences(""This line was passed from ERB!"", OUT_VAR_TEST)
PRINTFORML %OUT_VAR_TEST%
CALLSHARP ERBExecutionExample()

        ";

        public override string PluginVersion => "1.0";

        public override string PluginAuthor => "Neo_Kesha";
    }

    public class HelloWorldMethod : IPluginMethod
    {
        public string Name => "HelloWorld";

        public string Description => "Shows simple Hello World message";

        public void Execute(PluginMethodParameter[] args)
        {
            MessageBox.Show("Hello, The World!");
        }
    }

    public class ParametersAndReferencesMethod : IPluginMethod
    {
        public string Name => "ParametersAndReferences";

        public string Description => "Shows how to get args from ERB and how to send something back";

        public void Execute(PluginMethodParameter[] args)
        {
            MessageBox.Show(args[0].value);
            args[1].value = "This string is from Sharp Plugin";
        }
    }

    public class ERBExecutionExampleMethod : IPluginMethod
    {
        public string Name => "ERBExecutionExample";

        public string Description => "Rudimentary ERB command execution from Sharp. This is to use only when direct API call at PluginManager is not implemented";

        public void Execute(PluginMethodParameter[] args)
        {
            PluginManager.GetInstance().ExecuteLine("PRINTL this was printed from Sharp plugin!");
        }
    }
}
