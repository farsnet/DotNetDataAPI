using Farsnet.Service;

namespace Farsnet.Schema
{
    public class SenseRelation
    {
        private int id;

        private int senseId1;

        private int senseId2;

        private string senseWord1;

        private string senseWord2;

        private string type;

        public SenseRelation()
        {
        }

        public SenseRelation(int id, int senseId1, int senseId2, string senseWord1, string senseWord2, string type)
        {

            this.id = id;
            this.senseId1 = senseId1;
            this.senseId2 = senseId2;
            this.senseWord1 = senseWord1;
            this.senseWord2 = senseWord2;
            this.type = type;
        }

        public int Id
        {
            get
            {
                return id;
            }
        }

        public int SenseId1
        {
            get
            {
                return senseId1;
            }

            set
            {
                senseId1 = value;
            }
        }

        public int SenseId2
        {
            get
            {
                return senseId2;
            }

            set
            {
                senseId2 = value;
            }
        }

        public string SenseWord1
        {
            get
            {
                return senseWord1;
            }

            set
            {
                senseWord1 = value;
            }
        }

        public string SenseWord2
        {
            get
            {
                return senseWord2;
            }

            set
            {
                senseWord2 = value;
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

        public Sense Sense1
        {
            get
            {
                return SenseService.GetSenseById(this.senseId1);
            }
        }

        public Sense Sense2
        {
            get
            {
                return SenseService.GetSenseById(this.senseId2);
            }
        }
    }

}