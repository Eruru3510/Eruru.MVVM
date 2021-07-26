package demo.table;

import com.eruru.mvvm.MVVMBinding;
import com.eruru.mvvm.MVVMBindingRelativeSource;
import com.eruru.mvvm.MVVMBindingUpdateSourceTrigger;
import com.eruru.mvvm.MVVMTableColumn;
import com.eruru.mvvm.swing.*;

import javax.swing.*;
import javax.swing.border.LineBorder;
import java.awt.*;
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;

public class View {

	private static ViewModel viewModel;

	public static void main (String[] args) {
		JFrame frame = new JFrame ();
		frame.setSize (380, 250);
		frame.setLocationRelativeTo (null);
		frame.setDefaultCloseOperation (JFrame.EXIT_ON_CLOSE);
		frame.setLayout (new BorderLayout ());
		JTable table = new JTable ();
		table.setBorder (new LineBorder (Color.BLACK));
		table.addKeyListener (new KeyListener () {
			@Override
			public void keyTyped (KeyEvent e) {

			}

			@Override
			public void keyPressed (KeyEvent e) {
				if (e.getKeyCode () == 127) {
					e.consume ();
					if (table.getSelectedRow () > 0) {
						viewModel.getItems ().removeRange (table.getSelectedRow (), table.getSelectedRowCount ());
					}
				}
			}

			@Override
			public void keyReleased (KeyEvent e) {

			}
		});
		frame.add (new JScrollPane (table), BorderLayout.CENTER);
		JPanel panel = new JPanel (new FlowLayout ());
		frame.add (panel, BorderLayout.SOUTH);
		JTextField idTextField = new JTextField ();
		idTextField.setPreferredSize (new Dimension (100, 25));
		panel.add (idTextField);
		JTextField nameTextField = new JTextField ();
		nameTextField.setPreferredSize (new Dimension (100, 25));
		panel.add (nameTextField);
		JButton addButton = new JButton ("添加");
		addButton.setSize (75, 25);
		panel.add (addButton);
		JLabel countLabel = new JLabel ();
		panel.add (countLabel);
		frame.setVisible (true);

		new MVVMFrame (frame) {{
			setDataContext (new MVVMBinding (viewModel = new ViewModel ()));
			setTitle (new MVVMBinding ("countLabel", "text"));
		}}.add (
				new MVVMTable (table) {{
					setItemsSource (new MVVMBinding ("items"));
				}}.addColumns (
						new MVVMTableColumn ("ID", new MVVMBinding ("id")),
						new MVVMTableColumn ("Name", new MVVMBinding ("name")),
						new MVVMTableColumn ("Test", new MVVMBinding (new MVVMBindingRelativeSource (MVVMTable.class), "dataContext.name"))
				),
				new MVVMTextField (idTextField) {{
					setText (new MVVMBinding ("id"));
				}},
				new MVVMTextField (nameTextField) {{
					setText (new MVVMBinding ("name", MVVMBindingUpdateSourceTrigger.PropertyChanged));
				}},
				new MVVMButton (addButton) {{
					setOnAction (new MVVMBinding ("onAdd"));
				}},
				new MVVMLabel (countLabel) {{
					setName ("countLabel");
					setText (new MVVMBinding ("items.size") {{
						setStringFormat ("总计：%s");
					}});
				}}
		).build ();
	}

}