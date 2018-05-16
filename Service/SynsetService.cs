using System.Collections.Generic;
using System;
using System.Data.SQLite;
using Farsnet.Schema;
using Farsnet.Database;

namespace Farsnet.Service
{
    public class SynsetService
    {
        public static List<Synset> GetSynsetsByWord(string searchStyle, string searchKeyword)
        {
            List<Synset> results = new List<Synset>();

            string sql = "SELECT id, pos, semanticCategory, example, gloss, nofather, noMapping FROM synset " + "WHERE synset.id IN (SELECT " + "synset.id as synset_id " + "FROM " + "word INNER JOIN sense ON sense.word = word.id " + "INNER JOIN synset ON sense.synset = synset.id " + "LEFT OUTER JOIN value ON value.word = word.id " + "WHERE word.search_value @SearchStyle '@SearchValue' OR (value.search_value) @SearchStyle '@SearchValue') " + " OR synset.id IN (SELECT sense.synset AS synset_id FROM sense INNER JOIN sense_relation ON sense.id = sense_relation.sense INNER JOIN sense AS sense_2 ON sense_2.id = sense_relation.sense2 INNER JOIN word ON sense_2.word = word.id WHERE sense_relation.type =  'Refer-to' AND word.search_value LIKE  '@SearchValue')" + " OR synset.id IN (SELECT sense_2.synset AS synset_id FROM sense INNER JOIN sense_relation ON sense.id = sense_relation.sense INNER JOIN sense AS sense_2 ON sense_2.id = sense_relation.sense2 INNER JOIN word ON sense.word = word.id WHERE sense_relation.type =  'Refer-to' AND word.search_value LIKE  '@SearchValue')";

            searchKeyword = SecureValue(NormalValue(searchKeyword));

            if (searchStyle.Equals("LIKE") || searchStyle.Equals("START") || searchStyle.Equals("END"))
            {

                sql = sql.Replace("@SearchStyle", "LIKE");

                if (searchStyle.Equals("LIKE"))
                {

                    searchKeyword = "%" + searchKeyword + "%";
                }

                if (searchStyle.Equals("START"))
                {

                    searchKeyword = "" + searchKeyword + "%";
                }

                if (searchStyle.Equals("END"))
                {

                    searchKeyword = "%" + searchKeyword + "";
                }
            }
            if (searchStyle.Equals("EXACT"))
            {

                sql = sql.Replace("@SearchStyle", "=");
            }

            sql = sql.Replace("@SearchValue", searchKeyword);

            using (var conn = new SQLiteConnection(SqlLiteDbUtility.SqlLiteConnection))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();

                    cmd.CommandText = sql;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(new Synset(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), Convert.ToString(reader[5]), Convert.ToString(reader[6])));
                        }
                    }
                }
            }

            return results;
        }

        public static List<Synset> AllSynsets
        {
            get
            {
                List<Synset> results = new List<Synset>();

                string sql = "SELECT id, pos, semanticCategory, example, gloss, nofather, noMapping FROM synset ";

                using (var conn = new SQLiteConnection(SqlLiteDbUtility.SqlLiteConnection))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();

                        cmd.CommandText = sql;

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                results.Add(new Synset(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), Convert.ToString(reader[5]), Convert.ToString(reader[6])));
                            }
                        }
                    }
                }

                return results;
            }
        }

        public static Synset GetSynsetById(int synsetId)
        {
            Synset result = null;

            string sql = "SELECT id, pos, semanticCategory, example, gloss, nofather, noMapping FROM synset WHERE id=" + synsetId;

            using (var conn = new SQLiteConnection(SqlLiteDbUtility.SqlLiteConnection))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();

                    cmd.CommandText = sql;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result = new Synset(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), Convert.ToString(reader[5]), Convert.ToString(reader[6]));
                        }
                    }
                }
            }

            return result;
        }

        public static List<SynsetRelation> GetSynsetRelationsById(int synsetId)
        {
            List<SynsetRelation> results = new List<SynsetRelation>();

            string sql = "SELECT id, type, synsetWords1, synsetWords2, synset, synset2, reverse_type FROM synset_relation WHERE synset=" + synsetId + " OR synset2=" + synsetId;

            using (var conn = new SQLiteConnection(SqlLiteDbUtility.SqlLiteConnection))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();

                    cmd.CommandText = sql;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(new SynsetRelation(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetInt32(4), reader.GetInt32(5), reader.GetString(6)));
                        }
                    }
                }
            }

            List<SynsetRelation> resultsArr = new List<SynsetRelation>();

            SynsetRelation temp;

            String type;

            int synsetId2;

            int synsetId1;

            String synsetWords2;

            String synsetWords1;

            String reverseType;

            for (int i = 0; i < results.Count; i++)
            {

                temp = results[i];

                if (temp.SynsetId1 != synsetId)
                {
                    type = temp.Type;

                    synsetId2 = temp.SynsetId2;

                    synsetId1 = temp.SynsetId1;

                    synsetWords2 = temp.SynsetWords2;

                    synsetWords1 = temp.SynsetWords1;

                    reverseType = temp.ReverseType;

                    temp.ReverseType = type;

                    temp.SynsetId1 = synsetId2;

                    temp.SynsetId2 = synsetId1;

                    temp.SynsetWords1 = synsetWords2;

                    temp.SynsetWords2 = synsetWords1;

                    temp.Type = reverseType;
                }

                resultsArr.Add(temp);
            }

            return resultsArr;
        }

        public static List<SynsetRelation> GetSynsetRelationsByType(int synsetId, SynsetRelationType[] types)
        {
            List<SynsetRelation> results = new List<SynsetRelation>();

            String _types = "";

            String _revTypes = "";

            foreach (SynsetRelationType _type in types)
            {
                _types = _types + "'" + RelationValue(_type) + "',";

                _revTypes = _revTypes + "'" + RelationValue(ReverseRelationType(_type)) + "',";
            }

            _types = _types + "'not_type'";

            _revTypes = _revTypes + "'not_type'";

            String sql = "SELECT id, type, synsetWords1, synsetWords2, synset, synset2, reverse_type FROM synset_relation WHERE (synset = " + synsetId + " AND type in (" + _types + ")) OR (synset2 = " + synsetId + " AND type in (" + _revTypes + "))" + " ORDER BY synset";

            using (var conn = new SQLiteConnection(SqlLiteDbUtility.SqlLiteConnection))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();

                    cmd.CommandText = sql;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(new SynsetRelation(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetInt32(4), reader.GetInt32(5), reader.GetString(6)));
                        }
                    }
                }
            }

            return results;
        }

        public static List<WordNetSynset> GetWordNetSynsets(int synsetId)
        {
            List<WordNetSynset> results = new List<WordNetSynset>();

            string sql = "SELECT id, wnPos, wnOffset, example, gloss, synset, type FROM wordnetsynset WHERE synset=" + synsetId;

            using (var conn = new SQLiteConnection(SqlLiteDbUtility.SqlLiteConnection))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();

                    cmd.CommandText = sql;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(new WordNetSynset(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetInt32(5), reader.GetString(6)));
                        }
                    }
                }
            }

            return results;
        }

        public static List<SynsetExample> GetSynsetExamples(int synsetId)
        {
            List<SynsetExample> results = new List<SynsetExample>();

            string sql = "SELECT gloss_and_example.id, content, lexicon.title FROM gloss_and_example INNER JOIN lexicon ON gloss_and_example.lexicon=lexicon.id WHERE type='EXAMPLE' and synset=" + synsetId;

            using (var conn = new SQLiteConnection(SqlLiteDbUtility.SqlLiteConnection))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();

                    cmd.CommandText = sql;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(new SynsetExample(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
                        }
                    }
                }
            }

            return results;
        }

        public static List<SynsetGloss> GetSynsetGlosses(int synsetId)
        {
            List<SynsetGloss> results = new List<SynsetGloss>();

            string sql = "SELECT gloss_and_example.id, content, lexicon.title FROM gloss_and_example INNER JOIN lexicon ON gloss_and_example.lexicon=lexicon.id WHERE type='GLOSS' and synset=" + synsetId;

            using (var conn = new SQLiteConnection(SqlLiteDbUtility.SqlLiteConnection))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();

                    cmd.CommandText = sql;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(new SynsetGloss(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
                        }
                    }
                }
            }

            return results;
        }

        private static string NormalValue(string Value)
        {

            string NormalValue = Value;

            NormalValue = NormalValue.Replace("ی", "ي");

            NormalValue = NormalValue.Replace("ى", "ي");

            NormalValue = NormalValue.Replace("ك", "ک");

            NormalValue = NormalValue.Replace("'", "");

            NormalValue = NormalValue.Replace("\"", "");

            NormalValue = NormalValue.Replace(" ", "");

            NormalValue = NormalValue.Replace("‌", "");

            //همزه

            NormalValue = NormalValue.Replace("‌‌ء", "");

            NormalValue = NormalValue.Replace("‌‌ٔ", "");

            NormalValue = NormalValue.Replace("‌‌ؤ", "و");

            NormalValue = NormalValue.Replace("‌‌ئ", "ي");

            NormalValue = NormalValue.Replace("آ", "ا");

            NormalValue = NormalValue.Replace("‌‌أ", "ا");

            NormalValue = NormalValue.Replace("إ", "ا");

            NormalValue = NormalValue.Replace("ۀ", "ه");

            NormalValue = NormalValue.Replace("ة", "ه");

            //اعراب و تنوین

            NormalValue = NormalValue.Replace("َ", "");

            NormalValue = NormalValue.Replace("ُ", "");

            NormalValue = NormalValue.Replace("ِ", "");

            NormalValue = NormalValue.Replace("ً", "");

            NormalValue = NormalValue.Replace("ٌ", "");

            NormalValue = NormalValue.Replace("ٍ", "");

            //تشدید و سكون

            NormalValue = NormalValue.Replace("ّ", "");

            NormalValue = NormalValue.Replace("ْ", "");

            NormalValue = NormalValue.Replace("ِّ", "");

            NormalValue = NormalValue.Replace("ٍّ", "");

            NormalValue = NormalValue.Replace("َّ", "");

            NormalValue = NormalValue.Replace("ًّ", "");

            NormalValue = NormalValue.Replace("ُّ", "");

            NormalValue = NormalValue.Replace("ٌّ", "");

            NormalValue = NormalValue.Replace("u200D", "%");

            NormalValue = NormalValue.Replace("ء", "");

            NormalValue = NormalValue.Replace("أ", "ا");

            NormalValue = NormalValue.Replace("ئ", "ي");

            return NormalValue;
        }

        private static string SecureValue(string Value)
        {

            if (string.ReferenceEquals(Value, null))
            {
                return "";
            }

            Value = Value.Replace("\0", "");

            Value = Value.Replace("\'", "");

            Value = Value.Replace("\"", "");

            Value = Value.Replace("\b", "");

            Value = Value.Replace("\n", "");

            Value = Value.Replace("\r", "");

            Value = Value.Replace("\t", "");

            Value = Value.Replace("\\", "");

            Value = Value.Replace("/", "");

            Value = Value.Replace("%", "");

            Value = Value.Replace("_", "");

            Value = Value.Replace("ـ", "");

            //Value = Value.replace("-", "");

            Value = Value.Replace("!", "");

            Value = Value.Replace(";", "");

            Value = Value.Replace("?", "");

            Value = Value.Replace("=", "");

            Value = Value.Replace("<", "");

            Value = Value.Replace(">", "");

            Value = Value.Replace("&", "");

            Value = Value.Replace("#", "");

            Value = Value.Replace("@", "");

            Value = Value.Replace("$", "");

            Value = Value.Replace("^", "");

            Value = Value.Replace("*", "");

            Value = Value.Replace("+", "");

            return Value;
        }

        private static String RelationValue(SynsetRelationType type)
        {

            if (type.ToString() == "Related_to" ||
               type.ToString() == "Has-Unit" ||
               type.ToString().Substring(3) == "Is_"
                    )
            {
                return type.ToString().Replace("_", "-");
            }

            if (type.ToString() == "Has_Salient_defining_feature")
            {
                return "Has-Salient defining feature";
            }

            return type.ToString().Replace("_", " ");
        }

        private static SynsetRelationType ReverseRelationType(SynsetRelationType type)
        {

            //Agent...Is_Agent_of
            if (SynsetRelationType.Agent == type)
            {

                return SynsetRelationType.Is_Agent_of;
            }

            if (SynsetRelationType.Is_Agent_of == type)
            {

                return SynsetRelationType.Agent;
            }

            //Hypernym..Hyponym
            if (SynsetRelationType.Hypernym == type)
            {

                return SynsetRelationType.Hyponym;
            }

            if (SynsetRelationType.Hyponym == type)
            {

                return SynsetRelationType.Hypernym;
            }

            //Instance_hypernym..Instance_hyponym
            if (SynsetRelationType.Instance_hyponym == type)
            {

                return SynsetRelationType.Instance_hypernym;
            }

            if (SynsetRelationType.Instance_hypernym == type)
            {

                return SynsetRelationType.Instance_hyponym;
            }

            //Part_holonym..Part_meronym
            if (SynsetRelationType.Part_holonym == type)
            {

                return SynsetRelationType.Part_meronym;
            }

            if (SynsetRelationType.Part_meronym == type)
            {

                return SynsetRelationType.Part_holonym;
            }

            //Member_holonym..Member_meronym
            if (SynsetRelationType.Member_holonym == type)
            {

                return SynsetRelationType.Member_meronym;
            }

            if (SynsetRelationType.Member_meronym == type)
            {

                return SynsetRelationType.Member_holonym;
            }

            //Substance_holonym..Substance_meronym
            if (SynsetRelationType.Substance_holonym == type)
            {

                return SynsetRelationType.Substance_meronym;
            }

            if (SynsetRelationType.Substance_meronym == type)
            {

                return SynsetRelationType.Substance_holonym;
            }

            //Portion_meronym..Portion_holonym
            if (SynsetRelationType.Portion_holonym == type)
            {

                return SynsetRelationType.Portion_meronym;
            }

            if (SynsetRelationType.Portion_meronym == type)
            {

                return SynsetRelationType.Portion_holonym;
            }

            //Is_Domain_of..Domain
            if (SynsetRelationType.Domain == type)
            {

                return SynsetRelationType.Is_Domain_of;
            }

            if (SynsetRelationType.Is_Domain_of == type)
            {

                return SynsetRelationType.Domain;
            }

            //Is_Caused_by..Cause
            if (SynsetRelationType.Cause == type)
            {

                return SynsetRelationType.Is_Caused_by;
            }

            if (SynsetRelationType.Is_Caused_by == type)
            {

                return SynsetRelationType.Cause;
            }

            //Is_Instrument_of..Instrument
            if (SynsetRelationType.Is_Instrument_of == type)
            {

                return SynsetRelationType.Instrument;
            }

            if (SynsetRelationType.Instrument == type)
            {

                return SynsetRelationType.Is_Instrument_of;
            }

            //Is_Entailed_by...Entailment
            if (SynsetRelationType.Is_Entailed_by == type)
            {

                return SynsetRelationType.Entailment;
            }

            if (SynsetRelationType.Entailment == type)
            {

                return SynsetRelationType.Is_Entailed_by;
            }

            //Location...Is_Location_of
            if (SynsetRelationType.Location == type)
            {

                return SynsetRelationType.Is_Location_of;
            }

            if (SynsetRelationType.Is_Location_of == type)
            {

                return SynsetRelationType.Location;
            }

            //Has_Salient_defining_feature..Salient_defining_feature
            if (SynsetRelationType.Has_Salient_defining_feature == type)
            {

                return SynsetRelationType.Salient_defining_feature;
            }

            if (SynsetRelationType.Salient_defining_feature == type)
            {

                return SynsetRelationType.Has_Salient_defining_feature;
            }

            //Is_Attribute_of..Attribute
            if (SynsetRelationType.Is_Attribute_of == type)
            {

                return SynsetRelationType.Attribute;
            }

            if (SynsetRelationType.Attribute == type)
            {

                return SynsetRelationType.Is_Attribute_of;
            }

            //Unit..Attribute
            if (SynsetRelationType.Unit == type)
            {

                return SynsetRelationType.Has_Unit;
            }

            if (SynsetRelationType.Has_Unit == type)
            {

                return SynsetRelationType.Unit;
            }

            //Is_Patient_of..Patient
            if (SynsetRelationType.Is_Patient_of == type)
            {

                return SynsetRelationType.Patient;
            }

            if (SynsetRelationType.Patient == type)
            {

                return SynsetRelationType.Is_Patient_of;
            }

            return type;
        }

    }

}