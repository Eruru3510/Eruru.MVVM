package demo.usePropertyChangedToNotifyTextfieldUpdateValue;

import com.eruru.mvvm.MVVMBinding;
import com.eruru.mvvm.swing.MVVMButton;
import com.eruru.mvvm.swing.MVVMFrame;
import com.eruru.mvvm.swing.MVVMTextField;

import javax.swing.*;
import java.awt.*;

public class View {

	public static void main (String[] args) {
		JFrame frame = new JFrame ();
		frame.setSize (380, 250);
		frame.setLocationRelativeTo (null);
		frame.setDefaultCloseOperation (JFrame.EXIT_ON_CLOSE);
		frame.setLayout (new FlowLayout ());
		JTextField textField = new JTextField ();
		textField.setPreferredSize (new Dimension (200, 25));
		frame.add (textField);
		JButton button = new JButton ("Click me to set text");
		button.setSize (200, 25);
		frame.add (button);
		frame.setVisible (true);

		new MVVMFrame (frame) {{
			setDataContext (new MVVMBinding (new ViewModel ()));
		}}.add (
				new MVVMTextField (textField) {{
					setText (new MVVMBinding ("text"));
				}},
				new MVVMButton (button) {{
					setOnAction (new MVVMBinding ("onClick"));
				}}
		).build ();
	}

}