using Farsnet.Service;

namespace Farsnet.Schema
{
	public class SynsetRelation
	{

		private int id;

		private string type;

		private string synsetWords1;

		private string synsetWords2;

		private int synsetId1;

		private int synsetId2;

		private string reverseType;

		public SynsetRelation(int id, string type, string synsetWords1, string synsetWords2, int synsetId1, int synsetId2, string reverseType)
		{

			this.id = id;
			this.type = type;
			this.synsetWords1 = synsetWords1;
			this.synsetWords2 = synsetWords2;
			this.synsetId1 = synsetId1;
			this.synsetId2 = synsetId2;
			this.reverseType = reverseType;
		}

		public SynsetRelation()
		{
			// TODO Auto-generated constructor stub
		}

		public int Id
		{
			get
			{
			  return id;
			}
		}

		public string Type
		{
			get
			{
			  return type;
			}

            set
            {
                type = value;
            }
		}

		public string SynsetWords1
		{
			get
			{
			  return synsetWords1;
			}

            set
            {
                synsetWords1 = value;
            }
		}

		public string SynsetWords2
		{
			get
			{
			  return synsetWords2;
			}

            set
            {
                synsetWords2 = value;
            }
		}

		public int SynsetId1
		{
			get
			{
			  return synsetId1;
			}

            set
            {
                synsetId1 = value;
            }
		}

		public int SynsetId2
		{
			get
			{
			  return synsetId2;
			}

            set
            {
                synsetId2 = value;
            }
		}

        public string ReverseType
        {
            get
            {
                return reverseType;
            }

            set
            {
                reverseType = value;
            }
        }

		public Synset Synset1
		{
			get
			{
				return SynsetService.GetSynsetById(this.synsetId1);
			}
		}

        public Synset Synset2
		{
			get
			{
				return SynsetService.GetSynsetById(this.synsetId2);
			}
		}
	}

}