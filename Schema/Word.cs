using System.Collections.Generic;
using Farsnet.Service;

namespace Farsnet.Schema
{
    public class Word
    {

        private int id;

        private string pos;

        private string defaultPhonetic;

        private string defaultValue;

        public Word()
        {
        }

        public Word(int id, string pos, string defaultPhonetic, string defaultValue)
        {

            this.id = id;
            this.defaultPhonetic = defaultPhonetic;
            this.defaultValue = defaultValue;
            this.pos = pos;
        }

        public int Id
        {
            get
            {
                return id;
            }
        }

        public string Pos
        {
            get
            {
                return pos;
            }
        }

        public string DefaultPhonetic
        {
            get
            {
                return defaultPhonetic;
            }
        }

        public string DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }

        public List<WrittenForm> WrittenForms
        {
            get
            {
                return SenseService.GetWrittenFormsByWord(this.id);
            }
        }

        public List<PhoneticForm> PhoneticForms
        {
            get
            {
                return SenseService.GetPhoneticFormsByWord(this.id);
            }
        }
    }

}