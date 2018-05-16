using System.Collections.Generic;
using Farsnet.Service;

namespace Farsnet.Schema
{
    public class Synset
    {

        private int id;

        private string pos;

        private string semanticCategory;

        //private String example;

        //private String gloss;

        private string nofather;

        private string noMapping;

        public Synset()
        {
        }

        public Synset(int id, string pos, string semanticCategory, string example, string gloss, string nofather, string noMapping)
        {

            this.id = id;
            this.semanticCategory = semanticCategory;
            //this.example = example;
            //this.gloss = gloss;
            this.nofather = nofather;
            this.noMapping = noMapping;
            this.pos = pos;
        }

        public int Id
        {
            get
            {
                return id;
            }
        }

        public string SemanticCategory
        {
            get
            {
                return semanticCategory;
            }
        }

        public string NoMapping
        {
            get
            {
                return noMapping;
            }
        }

        public string Nofather
        {
            get
            {
                return nofather;
            }
        }

        public string Pos
        {
            get
            {
                return pos;
            }
        }

        public List<SynsetExample> Examples
        {
            get
            {
                return SynsetService.GetSynsetExamples(this.id);
            }
        }

        public List<SynsetGloss> Glosses
        {
            get
            {
                return SynsetService.GetSynsetGlosses(this.id);
            }
        }

        public List<Sense> Senses
        {
            get
            {

                return SenseService.GetSensesBySynset(this.id);
            }
        }

        public List<WordNetSynset> WordNetSynsets
        {
            get
            {

                return SynsetService.GetWordNetSynsets(this.id);
            }
        }

        public List<SynsetRelation> SynsetRelations
        {
            get
            {

                return SynsetService.GetSynsetRelationsById(this.id);
            }
        }

        public List<SynsetRelation> getSynsetRelations(SynsetRelationType relationType)
        {

            SynsetRelationType[] types = new SynsetRelationType[1];
            types[0] = relationType;

            return SynsetService.GetSynsetRelationsByType(this.id, types);
        }

        public List<SynsetRelation> getSynsetRelations(SynsetRelationType[] relationTypes)
        {

            return SynsetService.GetSynsetRelationsByType(this.id, relationTypes);
        }
    }

}