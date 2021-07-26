package demo.itemsControlImplementCustomViewList;

import com.eruru.mvvm.MVVMBinding;
import com.eruru.mvvm.MVVMDataTemplate;
import com.eruru.mvvm.swing.*;

import javax.swing.*;
import java.awt.*;

public class View {

	private static MVVMControl control;
	private static MVVMItemsControl itemsControl;
	private static ViewModel viewModel;

	public static void main (String[] args) {
		JFrame frame = new JFrame ();
		frame.setSize (380, 250);
		frame.setLocationRelativeTo (null);
		frame.setDefaultCloseOperation (JFrame.EXIT_ON_CLOSE);
		frame.setLayout (new BorderLayout ());
		JPanel itemsPanel = new JPanel (new FlowLayout ());
		itemsPanel.setBorder (BorderFactory.createLineBorder (Color.BLACK));
		frame.add (itemsPanel, BorderLayout.CENTER);
		JPanel bottomPanel = new JPanel (new FlowLayout ());
		frame.add (bottomPanel, BorderLayout.SOUTH);
		JButton addButton = new JButton ("添加");
		addButton.setSize (200, 25);
		addButton.addActionListener (e -> valid ());
		bottomPanel.add (addButton);
		JButton clearButton = new JButton ("清空");
		clearButton.setSize (200, 25);
		clearButton.addActionListener (e -> valid ());
		bottomPanel.add (clearButton);
		JButton rebindButton = new JButton ("重构");
		rebindButton.setSize (200, 25);
		rebindButton.addActionListener (e -> valid ());
		rebindButton.addActionListener (e -> control.build ());
		bottomPanel.add (rebindButton);
		frame.setVisible (true);

		control = new MVVMFrame (frame) {{
			setDataContext (new MVVMBinding (viewModel = new ViewModel ()));
		}}.add (
				itemsControl = new MVVMItemsControl (itemsPanel) {{
					setItemsSource (new MVVMBinding ("items"));
					setItemTemplate (new MVVMBinding (new MVVMDataTemplate (() -> new ItemView (View::valid).getControl ())));
				}},
				new MVVMButton (addButton) {{
					setOnAction (new MVVMBinding ("onAdd"));
				}},
				new MVVMButton (clearButton) {{
					setOnAction (new MVVMBinding ("onClear"));
				}}
		).build ();
	}

	private static String valid () {
		StringBuilder stringBuilder = new StringBuilder ();
		stringBuilder.append ("操作结果：\n");
		boolean isValid = true;
		for (int i = 0; i < viewModel.getItems ().size (); i++) {
			JLabel label = (JLabel) ((JPanel) itemsControl.getControl ().getComponent (i)).getComponent (0);
			MVVMLabel mvvmLabel = (MVVMLabel) itemsControl.get (i).get (0);
			int model = viewModel.getItems ().get (i).getId ();
			if (Integer.parseInt (label.getText ()) != model) {
				isValid = false;
			}
			if (label != mvvmLabel.getControl ()) {
				isValid = false;
			}
			stringBuilder.append (String.format (
					"显示：%s 数据:%s 控件树对应：%s\n",
					label.getText (),
					model,
					label == mvvmLabel.getControl ()
			));
		}
		System.out.println (stringBuilder);
		if (!isValid) {
			JOptionPane.showMessageDialog (null, "验证未通过");
		}
		return stringBuilder.toString ();
	}

}