using System.Collections.Generic;
using Farsnet.Service;

namespace Farsnet.Schema
{
    public class Sense
    {
        internal int id;

        internal string seqId;

        internal string value;

        internal Word word;

        //vtansivity
        internal string verbTransitivity;

        //vactivity
        internal string verbActivePassive;

        //vtype
        internal string verbType;

        internal string synset;

        //vpastStem
        internal string verbPastStem;

        //vpresentStem
        internal string verbPresentStem;

        //category
        internal string nounCategory;

        //goupOrMokassar
        internal string nounPluralType;

        //esmeZamir
        internal string pronoun;

        //adad
        internal string nounNumeralType;

        //adverb_type_1
        internal string adverbType1;

        //adverb_type_2
        internal string adverbType2;

        //adj_pishin_vijegi
        internal string preNounAdjectiveType;

        //adj_type
        internal string adjectiveType2;

        //noe_khas
        internal string nounSpecifityType;

        internal string nounType;

        //adj_type_sademorakkab
        internal string adjectiveType1;

        //vIssababi
        internal bool? isCausative;

        //vIsIdiom
        internal bool? isIdiomatic;

        //vGozaraType;
        internal string transitiveType;

        //kootah_nevesht
        internal bool? isAbbreviation;

        //mohaverh
        internal bool? isColloquial;

        public Sense()
        {
        }

        public Sense(int id, string seqId, string pos, string defaultValue, int wordId, string defaultPhonetic, string verbTransitivity, string verbActivePassive, string verbType, string synset, string verbPastStem, string verbPresentStem, string nounCategory, string nounPluralType, string pronoun, string nounNumeralType, string adverbType1, string adverbType2, string preNounAdjectiveType, string adjectiveType2, string nounSpecifityType, string nounType, string adjectiveType1, bool? isCausative, bool? isIdiomatic, string transitiveType, bool? isAbbreviation, bool? isColloquial)
        {

            this.id = id;
            this.isColloquial = isColloquial;
            this.isAbbreviation = isAbbreviation;
            this.transitiveType = transitiveType;
            this.isIdiomatic = isIdiomatic;
            this.isCausative = isCausative;
            this.adjectiveType1 = adjectiveType1;
            this.nounType = nounType;
            this.nounSpecifityType = nounSpecifityType;
            this.adjectiveType2 = adjectiveType2;
            this.preNounAdjectiveType = preNounAdjectiveType;
            this.adverbType1 = adverbType1;
            this.adverbType2 = adverbType2;
            this.nounNumeralType = nounNumeralType;
            this.pronoun = pronoun;
            this.nounPluralType = nounPluralType;
            this.nounCategory = nounCategory;
            this.verbPastStem = verbPastStem;
            this.verbPresentStem = verbPresentStem;
            this.synset = synset;
            this.verbType = verbType;
            this.verbActivePassive = verbActivePassive;
            this.verbTransitivity = verbTransitivity;
            this.id = id;
            this.seqId = seqId;
            this.value = defaultValue;
            this.word = new Word(wordId, pos, defaultPhonetic, defaultValue);
        }

        public int Id
        {
            get
            {
                return id;
            }
        }

        public string SeqId
        {
            get
            {
                return seqId;
            }
        }

        public string Value
        {
            get
            {
                return value;
            }
        }

        public string VerbActivePassive
        {
            get
            {
                return verbActivePassive;
            }
        }

        public string VerbTransitivity
        {
            get
            {
                return verbTransitivity;
            }
        }

        public string VerbType
        {
            get
            {
                return verbType;
            }
        }

        public string VerbPresentStem
        {
            get
            {
                return verbPresentStem;
            }
        }

        public string VerbPastStem
        {
            get
            {
                return verbPastStem;
            }
        }

        public string NounCategory
        {
            get
            {
                return nounCategory;
            }
        }

        public string NounPluralType
        {
            get
            {
                return nounPluralType;
            }
        }

        public string Pronoun
        {
            get
            {
                return pronoun;
            }
        }

        public string NounNumeralType
        {
            get
            {
                return nounNumeralType;
            }
        }

        public string AdverbType1
        {
            get
            {
                return adverbType1;
            }
        }

        public string AdverbType2
        {
            get
            {
                return adverbType2;
            }
        }

        public string PreNounAdjectiveType
        {
            get
            {
                return preNounAdjectiveType;
            }
        }

        public string AdjectiveType2
        {
            get
            {
                return adjectiveType2;
            }
        }

        public string NounSpecifityType
        {
            get
            {
                return nounSpecifityType;
            }
        }

        public string NounType
        {
            get
            {
                return nounType;
            }
        }

        public string AdjectiveType1
        {
            get
            {
                return adjectiveType1;
            }
        }

        public bool? IsCausative
        {
            get
            {
                return isCausative;
            }
        }

        public bool? IsIdiomatic
        {
            get
            {
                return isIdiomatic;
            }
        }

        public string TransitiveType
        {
            get
            {
                return transitiveType;
            }
        }

        public bool? IsAbbreviation
        {
            get
            {
                return isAbbreviation;
            }
        }

        public bool? IsColloquial
        {
            get
            {
                return isColloquial;
            }
        }

        public Word Word
        {
            get
            {
                return word;
            }
        }

        public Synset Synset
        {
            get
            {

                if (!string.ReferenceEquals(this.synset, null) && !this.synset.Equals(""))
                {
                    return SynsetService.GetSynsetById(int.Parse(this.synset));
                }
                else
                {
                    return null;
                }
            }
        }

        public List<SenseRelation> SenseRelations
        {
            get
            {
                return SenseService.GetSenseRelationsById(this.id);
            }
        }

        public List<SenseRelation> GetSenseRelations(SenseRelationType relationType)
        {
            SenseRelationType[] types = new SenseRelationType[1];
            types[0] = relationType;

            return SenseService.GetSenseRelationsByType(this.id, types);
        }

        public List<SenseRelation> GetSenseRelations(SenseRelationType[] relationTypes)
        {
            return SenseService.GetSenseRelationsByType(this.id, relationTypes);
        }
    }

}