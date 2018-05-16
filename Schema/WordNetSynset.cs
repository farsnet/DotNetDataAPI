namespace Farsnet.Schema
{
    public class WordNetSynset
    {

        private int id;

        private string wnPos;

        private string wnOffset;

        private string example;

        private string gloss;

        private int synset;

        private string type;

        public WordNetSynset()
        {
        }

        public WordNetSynset(int id, string wnPos, string wnOffset, string example, string gloss, int synset, string type)
        {

            this.id = id;
            this.wnPos = wnPos;
            this.wnOffset = wnOffset;
            this.example = example;
            this.gloss = gloss;
            this.synset = synset;
            this.type = type;

        }

        public int Id
        {
            get
            {
                return id;
            }
        }

        public string WnPos
        {
            get
            {
                return wnPos;
            }
        }

        public string WnOffset
        {
            get
            {
                return wnOffset;
            }
        }

        public string Example
        {
            get
            {
                return example;
            }
        }

        public string Gloss
        {
            get
            {
                return gloss;
            }
        }

        public int Synset
        {
            get
            {
                return synset;
            }
        }

        public string Type
        {
            get
            {
                return type;
            }
        }
    }
}