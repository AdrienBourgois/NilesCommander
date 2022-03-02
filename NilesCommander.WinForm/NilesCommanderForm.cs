using NilesCommander.Core;

namespace NilesCommander.WinForm;

public partial class NilesCommanderForm : Form
{
    public delegate void FormCommandProvided(string _command);

    public event FormCommandProvided OnFormCommandProvided;

    public NilesCommanderForm()
    {
        InitializeComponent();
    }

    public void AddLogToDisplay(Log _log)
    {
        int new_row_index = LogDataGridView.Rows.Add(_log.Time, _log.Severity, _log.Source, _log.Message);

        DataGridViewRow new_row = LogDataGridView.Rows[new_row_index];

        Color severity_color = _log.Severity switch
        {
            Log.LogSeverity.Debug => Color.LightGray,
            Log.LogSeverity.Verbose => Color.Gray,
            Log.LogSeverity.Log => Color.Black,
            Log.LogSeverity.Warning => Color.Yellow,
            Log.LogSeverity.Error => Color.DarkRed,
            _ => Color.DarkSlateGray
        };

        new_row.DefaultCellStyle.ForeColor = severity_color;
    }

    private void CommandTextBox_KeyDown(object _sender, KeyEventArgs _e)
    {
        if (_e.KeyCode == Keys.Enter)
        {
            string command = CommandTextBox.Text;

            CommandTextBox.ResetText();

            _e.SuppressKeyPress = true;

            OnFormCommandProvided?.Invoke(command);
        }
    }
}