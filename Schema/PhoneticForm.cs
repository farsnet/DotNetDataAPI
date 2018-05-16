namespace Farsnet.Schema
{
	public class PhoneticForm
	{

		private int id;

		private string value;

		public PhoneticForm()
		{
		}

		public PhoneticForm(int id, string value)
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