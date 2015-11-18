using System;
using Gtk;
using Bla;
using System.Threading;

namespace Bla
{
	public class Prompt
	{
		/*
		public void CreateMyForm()
		{
			// Create a new instance of the form.
			Form form1 = new Form();
			// Create two buttons to use as the accept and cancel buttons.
			Button button1 = new Button ();
			Button button2 = new Button ();

			// Set the text of button1 to "OK".
			button1.Text = "OK";
			// Set the position of the button on the form.
			button1.Location = new Point (10, 10);
			// Set the caption bar text of the form.   
			form1.Text = "My Dialog Box";
			// Display a help button on the form.
			form1.HelpButton = true;

			// Define the border style of the form to a dialog box.
			form1.FormBorderStyle = FormBorderStyle.FixedDialog;
			// Set the MaximizeBox to false to remove the maximize box.
			form1.MaximizeBox = false;
			// Set the MinimizeBox to false to remove the minimize box.
			form1.MinimizeBox = false;
			// Set the accept button of the form to button1.
			form1.AcceptButton = button1;
			// Set the start position of the form to the center of the screen.
			form1.StartPosition = FormStartPosition.CenterScreen;

			// Add button1 to the form.
			form1.Controls.Add(button1);

			button1.Click += sendButtonClicked

			// Display the form as a modal dialog box.
			form1.ShowDialog();
		}
		*/

		public static string getInput() {
			Prompt p = new Prompt ();
			EventWaitHandle _waitHandle = new AutoResetEvent (false);
			return "";
		}
		TextView input;
		Window prompt;

		public Prompt ()//TextView input)
		{
			input = new TextView ();
			//this.input = input;
			openWindowPrompt (input);
		}

		public void openWindowPrompt (TextView input)
		{
			prompt = new Window ("Enter Input");
			VBox vb = new VBox();

			Button sendInput = new Button("Send");

			vb.PackStart(input, false, false, 1);
			vb.PackStart(sendInput, false, false, 1);

			sendInput.Clicked += sendButtonClicked;
			prompt.Add(vb);
			prompt.ShowAll();
		}

		protected void sendButtonClicked(object sender, EventArgs e)
		{
			getInput (input.Buffer.Text);
			prompt.Destroy ();
		}

		protected String getInput(String input)
		{
			return input;
		}

	}
}

