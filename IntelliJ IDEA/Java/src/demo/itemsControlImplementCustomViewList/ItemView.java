package demo.itemsControlImplementCustomViewList;

import com.eruru.mvvm.MVVMAction;
import com.eruru.mvvm.MVVMBinding;
import com.eruru.mvvm.MVVMBindingRelativeSource;
import com.eruru.mvvm.swing.MVVMButton;
import com.eruru.mvvm.swing.MVVMControl;
import com.eruru.mvvm.swing.MVVMItemsControl;
import com.eruru.mvvm.swing.MVVMLabel;

import javax.swing.*;
import java.awt.*;

public class ItemView {

	private MVVMControl control;
	private JPanel panel;
	private JLabel label;
	private JButton setNewButton;
	private JButton moveButton;
	private JButton deleteButton;
	private JButton insertButton;

	public ItemView (MVVMAction onValid) {
		panel = new JPanel (new FlowLayout ());
		panel.setBorder (BorderFactory.createLineBorder (Color.black));
		label = new JLabel ();
		label.setSize (75, 25);
		panel.add (label);
		setNewButton = new JButton ("赋予新项");
		setNewButton.setSize (100, 25);
		setNewButton.addActionListener (e -> onValid.invoke ());
		panel.add (setNewButton);
		moveButton = new JButton ("移动");
		moveButton.setSize (75, 25);
		moveButton.addActionListener (e -> onValid.invoke ());
		panel.add (moveButton);
		insertButton = new JButton ("插入新项");
		insertButton.setSize (75, 25);
		insertButton.addActionListener (e -> onValid.invoke ());
		panel.add (insertButton);
		deleteButton = new JButton ("删除");
		deleteButton.setSize (75, 25);
		deleteButton.addActionListener (e -> onValid.invoke ());
		panel.add (deleteButton);

		control = new MVVMControl (panel) {{

		}}.add (
				new MVVMLabel (label) {{
					setText (new MVVMBinding ("id"));
				}},
				new MVVMButton (setNewButton) {{
					setOnAction (new MVVMBinding (new MVVMBindingRelativeSource (MVVMItemsControl.class), "dataContext.onSetNew"));
					setOnActionParameter (new MVVMBinding ());
				}},
				new MVVMButton (moveButton) {{
					setOnAction (new MVVMBinding (new MVVMBindingRelativeSource (MVVMItemsControl.class), "dataContext.onMove"));
					setOnActionParameter (new MVVMBinding ());
				}},
				new MVVMButton (deleteButton) {{
					setOnAction (new MVVMBinding (new MVVMBindingRelativeSource (MVVMItemsControl.class), "dataContext.onDelete"));
					setOnActionParameter (new MVVMBinding ());
				}},
				new MVVMButton (insertButton) {{
					setOnAction (new MVVMBinding (new MVVMBindingRelativeSource (MVVMItemsControl.class), "dataContext.onInsert"));
					setOnActionParameter (new MVVMBinding ());
				}}
		);
	}

	public MVVMControl getControl () {
		return control;
	}

	public void setControl (MVVMControl control) {
		this.control = control;
	}

}