
namespace Siftan.WinForms.AcceptanceTests
{
  using System;
  using Jabberwocky.Toolkit.Object;
  using TestStack.White.UIItems;
  using TestStack.White.UIItems.WindowItems;

  /// <summary>
  /// Used to set the values of the controls in a Teststack.White window.
  /// </summary>
  public class WindowSetter
  {
    private readonly Window window;

    /// <summary>
    /// Initializes a new instance of the WindowSetter class.
    /// </summary>
    /// <param name="window">Reference to the window containing the controls to set.</param>
    public WindowSetter(Window window)
    {
      window.VerifyThatObjectIsNotNull("Parameter 'window' is null.");
      this.window = window;
    }

    /// <summary>
    /// Sets the value of the text box control.
    /// </summary>
    /// <param name="id">Id of the text box control.</param>
    /// <param name="value">Value to set in the text box.</param>
    /// <returns>Reference to allow chaining.</returns>
    public WindowSetter SetTextBoxValue(String id, String value)
    {
      var textBox = this.window.Get<TextBox>(id);
      this.window.WaitWhileBusy();
      textBox.Text = value;
      return this;
    }

    /// <summary>
    /// Sets the value of the spinner control.
    /// </summary>
    /// <param name="id">Id of the spinner control.</param>
    /// <param name="value">Value to set in the spinner control.</param>
    /// <returns>Reference to allow chaining.</returns>
    public WindowSetter SetSpinnerValue(String id, Double value)
    {
      var spinner = this.window.Get<Spinner>(id);
      this.window.WaitWhileBusy();
      spinner.Value = value;
      return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id">Id of the check box.</param>
    /// <param name="value">True to check the control box; false otherwise.</param>
    /// <returns>Reference to allow chaining.</returns>
    public WindowSetter SetCheckBoxChecked(String id, Boolean value)
    {
      var checkBox = this.window.Get<CheckBox>(id);
      this.window.WaitWhileBusy();
      checkBox.Checked = value;
      return this;
    }

    public WindowSetter ClickButton(String id)
    {
      var button = this.window.Get<Button>(id);
      this.window.WaitWhileBusy();
      button.Click();
      return this;
    }
  }
}
