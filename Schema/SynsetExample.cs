namespace Farsnet.Schema
{
	public class SynsetExample
	{

		private int id;

		private string content;

		private string lexicon;

		public SynsetExample()
		{
		}

		public SynsetExample(int id, string content, string lexicon)
		{

			this.id = id;
			this.content = content;
			this.lexicon = lexicon;
		}

		public int Id
		{
			get
			{
				return id;
			}
		}

		public string Content
		{
			get
			{
				return content;
			}
		}

		public string Lexicon
		{
			get
			{
				return lexicon;
			}
		}
	}

}