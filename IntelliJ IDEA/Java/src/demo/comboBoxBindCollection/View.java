package demo.comboBoxBindCollection;

import com.eruru.mvvm.MVVMBinding;
import com.eruru.mvvm.swing.MVVMComboBox;

import javax.swing.*;
import java.awt.*;

public class View {

	public static void main (String[] args) {
		JFrame frame = new JFrame ();
		frame.setSize (380, 250);
		frame.setLocationRelativeTo (null);
		frame.setDefaultCloseOperation (JFrame.EXIT_ON_CLOSE);
		frame.setLayout (new FlowLayout ());
		JComboBox comboBox = new JComboBox ();
		comboBox.setPreferredSize (new Dimension (200, 25));
		frame.add (comboBox);
		frame.setVisible (true);

		new MVVMComboBox (comboBox) {{
			setDataContext (new MVVMBinding (new ViewModel ()));
			setItemsSource (new MVVMBinding ("items"));
		}}.build ();
	}

}