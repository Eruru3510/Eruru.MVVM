package demo.itemsControlImplementCustomViewList;

public class Item {

	int id;

	public Item (int id) {
		this.id = id;
	}

	public int getId () {
		System.out.println ("get id: " + id);
		return id;
	}

	public void setId (int id) {
		this.id = id;
	}

}