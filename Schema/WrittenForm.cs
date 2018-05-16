namespace Farsnet.Schema
{
	public class WrittenForm
	{

		private int id;

		private string value;

		public WrittenForm()
		{
		}

		public WrittenForm(int id, string value)
		{

			this.id = id;
			this.value = value;
		}

		public int Id
		{
			get
			{
				return id;
			}
		}

		public string Value
		{
			get
			{
				return value;
			}
		}
	}
}