using System;
using System.Windows.Forms;
using Microsoft.Win32;

class NvidiaOptimusBypassTool : Form
{
    private Button applyButton;
    private Button revertButton;
    private TextBox adminWarningTextBox;

    public NvidiaOptimusBypassTool()
    {
        // Set form properties
        this.Text = "NVIDIA Optimus Bypass Tool";
        this.Size = new System.Drawing.Size(400, 250);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;

        // Initialize buttons with updated text and improved layout
        applyButton = new Button
        {
            Text = "Disable Optimus",
            Left = 50,
            Width = 150,
            Top = 60,
            Height = 40,
            Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold),
            BackColor = System.Drawing.Color.LightSkyBlue,
            FlatStyle = FlatStyle.Flat
        };
        revertButton = new Button
        {
            Text = "Enable Optimus",
            Left = 50,
            Width = 150,
            Top = 120,
            Height = 40,
            Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold),
            BackColor = System.Drawing.Color.LightGreen,
            FlatStyle = FlatStyle.Flat
        };

        // Attach button click events
        applyButton.Click += (sender, e) => ModifyRegistry(5);  // Disable Optimus
        revertButton.Click += (sender, e) => ModifyRegistry(0);  // Enable Optimus

        // Initialize admin warning TextBox
        adminWarningTextBox = new TextBox
        {
            Text = "It won't work without administrator!",
            ReadOnly = true,
            BackColor = System.Drawing.Color.LightYellow,
            BorderStyle = BorderStyle.None,
            Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold),
            Size = new System.Drawing.Size(300, 30),
            Location = new System.Drawing.Point(50, 20),
            TextAlign = HorizontalAlignment.Center
        };

        // Add controls to the form
        Controls.Add(applyButton);
        Controls.Add(revertButton);
        Controls.Add(adminWarningTextBox);
    }

    private void ModifyRegistry(int value)
    {
        try
        {
            string[] possiblePaths =
            {
                "SOFTWARE\\Microsoft\\Windows\\DWM",
                "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\DWM"
            };
            string valueName = "OverlayTestMode";
            bool success = false;

            foreach (string keyPath in possiblePaths)
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyPath, true))
                {
                    if (key != null)
                    {
                        key.SetValue(valueName, value, RegistryValueKind.DWord);
                        MessageBox.Show($"Registry updated successfully at {keyPath}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        success = true;
                        break;
                    }
                }
            }

            if (!success)
            {
                MessageBox.Show("Failed to find a valid registry path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error: " + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        MessageBox.Show("Make sure to run this program as an administrator!", "Administrator Privileges Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        Application.Run(new NvidiaOptimusBypassTool());
    }
}
