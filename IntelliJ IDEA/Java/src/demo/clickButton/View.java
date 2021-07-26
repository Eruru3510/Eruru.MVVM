package demo.clickButton;

import com.eruru.mvvm.MVVMBinding;
import com.eruru.mvvm.swing.MVVMButton;

import javax.swing.*;
import java.awt.*;

public class View {

	public static void main (String[] args) {
		JFrame frame = new JFrame ();
		frame.setSize (380, 250);
		frame.setLocationRelativeTo (null);
		frame.setDefaultCloseOperation (JFrame.EXIT_ON_CLOSE);
		frame.setLayout (new FlowLayout ());
		JButton button = new JButton ("Click me");
		button.setSize (75, 25);
		frame.add (button);
		frame.setVisible (true);

		new MVVMButton (button) {{
			setDataContext (new MVVMBinding (new ViewModel ()));
			setOnAction (new MVVMBinding ("onClick"));
		}}.build ();
	}

}