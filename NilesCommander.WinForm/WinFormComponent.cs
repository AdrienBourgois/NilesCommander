using NilesCommander.Core;
using NilesCommander.Core.Components;

namespace NilesCommander.WinForm;

public class WinFormComponent : IInputComponent, IOutputComponent
{
    public ComponentConfiguration Configuration { get; set; }

    private NilesCommanderForm Form { get; set; }

    private Thread FormThread { get; set; }

    public event Core.NilesCommander.CommandProvidedDelegate CommandProvided;

    public void SetComponentConfiguration(ComponentConfiguration _configuration)
    {
        Configuration = _configuration;
    }

    public bool Initialize()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        CreateAndOpenForm();

        return true;
    }

    void IOutputComponent.LogProvided(Log _log)
    {
        if (Form == null)
            return;

        if (Form.InvokeRequired)
            Form.Invoke(() => Form.AddLogToDisplay(_log));
        else
            Form.AddLogToDisplay(_log);
    }

    private void CreateAndOpenForm()
    {
        if (FormThread?.IsAlive == true || Form?.IsDisposed == false)
            return;

        Form = new NilesCommanderForm();
        Form.Closed += (_, _) => CleanupForm();
        Form.OnFormCommandProvided += (_command) => CommandProvided?.Invoke(_command);

        FormThread = new Thread(RunForm);
        FormThread.SetApartmentState(ApartmentState.STA);

        FormThread.Start();
    }

    public bool Close()
    {
        CleanupForm();

        if (Form is { IsHandleCreated: false })
        {
            if (Form.InvokeRequired)
                Form.Invoke(Form.Close);
            else
                Form.Close();
        }

        return true;
    }

    public void RunForm()
    {
        Application.Run(Form);
    }

    private void CleanupForm()
    {
        if (!Form.IsDisposed)
        {
            if (Form.InvokeRequired)
                Form.Invoke(Form.Dispose);
            else
                Form.Dispose();
        }

        Form = null;
        FormThread = null;
    }
}