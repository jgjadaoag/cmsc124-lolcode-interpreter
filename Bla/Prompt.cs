using System;
using Gtk;
using Bla;

namespace Bla
{
	public class Prompt
	{
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

