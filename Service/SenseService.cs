using System.Collections.Generic;
using System;
using System.Data.SQLite;
using Farsnet.Schema;
using Farsnet.Database;

namespace Farsnet.Service
{
    public class SenseService
    {
        public static List<Sense> GetSensesByWord(string searchStyle, string searchKeyword)
        {
            List<Sense> results = new List<Sense>();

            string sql = "SELECT sense.id, seqId, word.pos, word.defaultValue, word.id as wordId, word.avaInfo, vtansivity, vactivity, vtype, synset, vpastStem, vpresentStem, category, goupOrMokassar, esmeZamir, adad, adverb_type_1, adverb_type_2, adj_pishin_vijegi, adj_type, noe_khas, nounType, adj_type_sademorakkab, vIssababi, vIsIdiom, vGozaraType, kootah_nevesht, mohavere FROM sense INNER JOIN word ON sense.word = word.id WHERE sense.id IN (SELECT sense.id FROM word INNER JOIN sense ON sense.word = word.id LEFT OUTER JOIN value ON value.word = word.id WHERE word.search_value @SearchStyle '@SearchValue' OR value.search_value @SearchStyle '@SearchValue') " + "OR sense.id IN (SELECT sense.id FROM sense INNER JOIN sense_relation ON sense.id = sense_relation.sense INNER JOIN sense AS sense_2 ON sense_2.id = sense_relation.sense2 INNER JOIN word ON sense_2.word = word.id WHERE sense_relation.type =  'Refer-to' AND word.search_value LIKE  '@SearchValue') " + "OR sense.id IN (SELECT sense_2.id FROM sense INNER JOIN sense_relation ON sense.id = sense_relation.sense INNER JOIN sense AS sense_2 ON sense_2.id = sense_relation.sense2 INNER JOIN word ON sense.word = word.id WHERE sense_relation.type =  'Refer-to' AND word.search_value LIKE  '@SearchValue') ";

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
                            results.Add(new Sense(
                                    reader.GetInt32(0),//"id"
                                    Convert.ToString(reader[1]),//"seqId"
                                    reader.GetString(2),//"pos"
                                    reader.GetString(3),//"defaultValue"
                                    reader.GetInt32(4),//"wordId"
                                    reader.GetString(5),//"avaInfo"
                                    getVtansivity(reader.GetString(6)),//"vtansivity"
                                    getVactivity(reader.GetString(7)),//"vactivity"
                                    getVtype(reader.GetString(8)),//"vtype"
                                    getNormalValue(Convert.ToString(reader[9])),//"synset"
                                    getNormalValue(reader.GetString(10)),//"vpastStem"
                                    getNormalValue(reader.GetString(11)),//"vpresentStem"
                                    getCategory(reader.GetString(12)),//"category"
                                    getGoupOrMokassar(reader.GetString(13)),//"goupOrMokassar"
                                    getEsmeZamir(reader.GetString(14)),//"esmeZamir"
                                    getAdad(reader.GetString(15)),//"adad"
                                    getAdverbType1(reader.GetString(16)),//"adverb_type_1"
                                    getAdverbType2(reader.GetString(17)),//"adverb_type_2"
                                    getAdjPishinVijegi(reader.GetString(18)),//"adj_pishin_vijegi"
                                    getAdjType(reader.GetString(19)),//"adj_type"
                                    getNoeKhas(reader.GetString(20)),//"noe_khas"
                                    getNounType(reader.GetString(21)),//"nounType"
                                    getAdjTypeSademorakkab(reader.GetString(22)),//"adj_type_sademorakkab"
                                    Convert.ToString(reader[23]) == "1" ? true : false,//"vIssababi"
                                    Convert.ToString(reader[24]) == "1" ? true : false,//"vIsIdiom"
                                    getVGozaraType(reader.GetString(25)),//"vGozaraType"
                                    Convert.ToString(reader[26]) == "1" ? true : false,//"kootah_nevesht"
                                    Convert.ToString(reader[27]) == "1" ? true : false//"mohavere"
                        ));
                        }
                    }
                }
            }

            return results;
        }

        public static List<Sense> GetSensesBySynset(int synsetId)
        {
            List<Sense> results = new List<Sense>();

            string sql = "SELECT sense.id, seqId, word.pos, word.defaultValue, word.id as wordId, word.avaInfo, vtansivity, vactivity, vtype, synset, vpastStem, vpresentStem, category, goupOrMokassar, esmeZamir, adad, adverb_type_1, adverb_type_2, adj_pishin_vijegi, adj_type, noe_khas, nounType, adj_type_sademorakkab, vIssababi, vIsIdiom, vGozaraType, kootah_nevesht, mohavere FROM sense INNER JOIN word ON sense.word = word.id WHERE sense.synset = " + synsetId;

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
                            results.Add(new Sense(
                                    reader.GetInt32(0),//"id"
                                    Convert.ToString(reader[1]),//"seqId"
                                    reader.GetString(2),//"pos"
                                    reader.GetString(3),//"defaultValue"
                                    reader.GetInt32(4),//"wordId"
                                    reader.GetString(5),//"avaInfo"
                                    getVtansivity(reader.GetString(6)),//"vtansivity"
                                    getVactivity(reader.GetString(7)),//"vactivity"
                                    getVtype(reader.GetString(8)),//"vtype"
                                    getNormalValue(Convert.ToString(reader[9])),//"synset"
                                    getNormalValue(reader.GetString(10)),//"vpastStem"
                                    getNormalValue(reader.GetString(11)),//"vpresentStem"
                                    getCategory(reader.GetString(12)),//"category"
                                    getGoupOrMokassar(reader.GetString(13)),//"goupOrMokassar"
                                    getEsmeZamir(reader.GetString(14)),//"esmeZamir"
                                    getAdad(reader.GetString(15)),//"adad"
                                    getAdverbType1(reader.GetString(16)),//"adverb_type_1"
                                    getAdverbType2(reader.GetString(17)),//"adverb_type_2"
                                    getAdjPishinVijegi(reader.GetString(18)),//"adj_pishin_vijegi"
                                    getAdjType(reader.GetString(19)),//"adj_type"
                                    getNoeKhas(reader.GetString(20)),//"noe_khas"
                                    getNounType(reader.GetString(21)),//"nounType"
                                    getAdjTypeSademorakkab(reader.GetString(22)),//"adj_type_sademorakkab"
                                    Convert.ToString(reader[23]) == "1" ? true : false,//"vIssababi"
                                    Convert.ToString(reader[24]) == "1" ? true : false,//"vIsIdiom"
                                    getVGozaraType(reader.GetString(25)),//"vGozaraType"
                                    Convert.ToString(reader[26]) == "1" ? true : false,//"kootah_nevesht"
                                    Convert.ToString(reader[27]) == "1" ? true : false//"mohavere"
                        ));
                        }
                    }
                }
            }

            return results;
        }

        public static Sense GetSenseById(int senseId)
        {
            Sense result = null;

            string sql = "SELECT sense.id, seqId, word.pos, word.defaultValue, word.id as wordId, word.avaInfo, vtansivity, vactivity, vtype, synset, vpastStem, vpresentStem, category, goupOrMokassar, esmeZamir, adad, adverb_type_1, adverb_type_2, adj_pishin_vijegi, adj_type, noe_khas, nounType, adj_type_sademorakkab, vIssababi, vIsIdiom, vGozaraType, kootah_nevesht, mohavere FROM sense INNER JOIN word ON sense.word = word.id WHERE sense.id = " + senseId;

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
                            result = new Sense(
                                    reader.GetInt32(0),//"id"
                                    Convert.ToString(reader[1]),//"seqId"
                                    reader.GetString(2),//"pos"
                                    reader.GetString(3),//"defaultValue"
                                    reader.GetInt32(4),//"wordId"
                                    reader.GetString(5),//"avaInfo"
                                    getVtansivity(reader.GetString(6)),//"vtansivity"
                                    getVactivity(reader.GetString(7)),//"vactivity"
                                    getVtype(reader.GetString(8)),//"vtype"
                                    getNormalValue(Convert.ToString(reader[9])),//"synset"
                                    getNormalValue(reader.GetString(10)),//"vpastStem"
                                    getNormalValue(reader.GetString(11)),//"vpresentStem"
                                    getCategory(reader.GetString(12)),//"category"
                                    getGoupOrMokassar(reader.GetString(13)),//"goupOrMokassar"
                                    getEsmeZamir(reader.GetString(14)),//"esmeZamir"
                                    getAdad(reader.GetString(15)),//"adad"
                                    getAdverbType1(reader.GetString(16)),//"adverb_type_1"
                                    getAdverbType2(reader.GetString(17)),//"adverb_type_2"
                                    getAdjPishinVijegi(reader.GetString(18)),//"adj_pishin_vijegi"
                                    getAdjType(reader.GetString(19)),//"adj_type"
                                    getNoeKhas(reader.GetString(20)),//"noe_khas"
                                    getNounType(reader.GetString(21)),//"nounType"
                                    getAdjTypeSademorakkab(reader.GetString(22)),//"adj_type_sademorakkab"
                                    Convert.ToString(reader[23]) == "1" ? true : false,//"vIssababi"
                                    Convert.ToString(reader[24]) == "1" ? true : false,//"vIsIdiom"
                                    getVGozaraType(reader.GetString(25)),//"vGozaraType"
                                    Convert.ToString(reader[26]) == "1" ? true : false,//"kootah_nevesht"
                                    Convert.ToString(reader[27]) == "1" ? true : false//"mohavere"
                        );
                        }
                    }
                }
            }

            return result;
        }

        public static List<Sense> AllSenses
        {
            get
            {
                List<Sense> results = new List<Sense>();

                string sql = "SELECT sense.id, seqId, word.pos, word.defaultValue, word.id as wordId, word.avaInfo, vtansivity, vactivity, vtype, synset, vpastStem, vpresentStem, category, goupOrMokassar, esmeZamir, adad, adverb_type_1, adverb_type_2, adj_pishin_vijegi, adj_type, noe_khas, nounType, adj_type_sademorakkab, vIssababi, vIsIdiom, vGozaraType, kootah_nevesht, mohavere FROM sense INNER JOIN word ON sense.word = word.id";

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
                                results.Add(new Sense(
                                    reader.GetInt32(0),//"id"
                                    Convert.ToString(reader[1]),//"seqId"
                                    reader.GetString(2),//"pos"
                                    reader.GetString(3),//"defaultValue"
                                    reader.GetInt32(4),//"wordId"
                                    reader.GetString(5),//"avaInfo"
                                    getVtansivity(reader.GetString(6)),//"vtansivity"
                                    getVactivity(reader.GetString(7)),//"vactivity"
                                    getVtype(reader.GetString(8)),//"vtype"
                                    getNormalValue(Convert.ToString(reader[9])),//"synset"
                                    getNormalValue(reader.GetString(10)),//"vpastStem"
                                    getNormalValue(reader.GetString(11)),//"vpresentStem"
                                    getCategory(reader.GetString(12)),//"category"
                                    getGoupOrMokassar(reader.GetString(13)),//"goupOrMokassar"
                                    getEsmeZamir(reader.GetString(14)),//"esmeZamir"
                                    getAdad(reader.GetString(15)),//"adad"
                                    getAdverbType1(reader.GetString(16)),//"adverb_type_1"
                                    getAdverbType2(reader.GetString(17)),//"adverb_type_2"
                                    getAdjPishinVijegi(reader.GetString(18)),//"adj_pishin_vijegi"
                                    getAdjType(reader.GetString(19)),//"adj_type"
                                    getNoeKhas(reader.GetString(20)),//"noe_khas"
                                    getNounType(reader.GetString(21)),//"nounType"
                                    getAdjTypeSademorakkab(reader.GetString(22)),//"adj_type_sademorakkab"
                                    Convert.ToString(reader[23]) == "1" ? true : false,//"vIssababi"
                                    Convert.ToString(reader[24]) == "1" ? true : false,//"vIsIdiom"
                                    getVGozaraType(reader.GetString(25)),//"vGozaraType"
                                    Convert.ToString(reader[26]) == "1" ? true : false,//"kootah_nevesht"
                                    Convert.ToString(reader[27]) == "1" ? true : false//"mohavere"
                            ));
                            }
                        }
                    }
                }

                return results;
            }
        }

        public static List<SenseRelation> GetSenseRelationsById(int senseId)
        {
            List<SenseRelation> results = new List<SenseRelation>();

            string sql = "SELECT id, type, sense, sense2, senseWord1, senseWord2 FROM sense_relation WHERE sense = " + senseId + " OR sense2 = " + senseId;

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
                            results.Add(new SenseRelation(
                                reader.GetInt32(0),//"id"
                                reader.GetInt32(2),//"sense"
                                reader.GetInt32(3),//"sense2"
                                reader.GetString(4),//"senseWord1"
                                reader.GetString(5),//"senseWord2"
                                reader.GetString(1)//"type"
                                ));
                        }
                    }
                }
            }

            List<SenseRelation> resultsArr = new List<SenseRelation>();

            SenseRelation temp;

            String type;

            int senseId2;

            int senseId1;

            String senseWord2;

            String senseWord1;

            for (int i = 0; i < results.Count; i++)
            {
                temp = results[i];

                if (temp.SenseId1 != senseId)
                {
                    type = temp.Type;

                    senseId2 = temp.SenseId2;

                    senseId1 = temp.SenseId1;

                    senseWord2 = temp.SenseWord2;

                    senseWord1 = temp.SenseWord1;

                    temp.Type = ReverseSRelationType(type);

                    temp.SenseId1 = senseId2;

                    temp.SenseId2 = senseId1;

                    temp.SenseWord1 = senseWord2;

                    temp.SenseWord2 = senseWord1;
                }

                resultsArr.Add(temp);
            }

            return resultsArr;
        }

        public static List<SenseRelation> GetSenseRelationsByType(int senseId, SenseRelationType[] types)
        {
            List<SenseRelation> results = new List<SenseRelation>();

            String _types = "";

            String _revTypes = "";

            foreach (SenseRelationType _type in types)
            {
                _types = _types + "'" + RelationValue(_type) + "',";

                _revTypes = _revTypes + "'" + RelationValue(ReverseRelationType(_type)) + "',";
            }

            _types = _types + "'not_type'";

            _revTypes = _revTypes + "'not_type'"; ;

            String sql = "SELECT id, type, sense, sense2, senseWord1, senseWord2 FROM sense_relation WHERE (sense = " + senseId + " AND type in (" + _types + ")) OR (sense2 = " + senseId + " AND type in (" + _revTypes + "))" + " ORDER BY sense";

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
                            results.Add(new SenseRelation(
                                reader.GetInt32(0),//"id"
                                reader.GetInt32(2),//"sense"
                                reader.GetInt32(3),//"sense2"
                                reader.GetString(4),//"senseWord1"
                                reader.GetString(5),//"senseWord2"
                                reader.GetString(1)//"type"
                                ));
                        }
                    }
                }
            }

            List<SenseRelation> resultsArr = new List<SenseRelation>();

            SenseRelation temp;

            String type;

            int senseId2;

            int senseId1;

            String senseWord2;

            String senseWord1;

            for (int i = 0; i < results.Count; i++)
            {
                temp = results[i];

                if (temp.SenseId1 != senseId)
                {
                    type = temp.Type;

                    senseId2 = temp.SenseId2;

                    senseId1 = temp.SenseId1;

                    senseWord2 = temp.SenseWord2;

                    senseWord1 = temp.SenseWord1;

                    temp.Type = ReverseSRelationType(type);

                    temp.SenseId1 = senseId2;

                    temp.SenseId2 = senseId1;

                    temp.SenseWord1 = senseWord2;

                    temp.SenseWord2 = senseWord1;
                }

                resultsArr.Add(temp);
            }

            return resultsArr;
        }

        public static List<PhoneticForm> GetPhoneticFormsByWord(int wordId)
        {
            List<PhoneticForm> results = new List<PhoneticForm>();

            string sql = "SELECT id, value FROM speech WHERE word = " + wordId;

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
                            results.Add(new PhoneticForm(
                                reader.GetInt32(0),//"id"
                                reader.GetString(1)//"value"
                                ));
                        }
                    }
                }
            }

            return results;
        }

        public static List<WrittenForm> GetWrittenFormsByWord(int wordId)
        {
            List<WrittenForm> results = new List<WrittenForm>();

            string sql = "SELECT id, value FROM value WHERE word = " + wordId;

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
                            results.Add(new WrittenForm(
                                reader.GetInt32(0),//"id"
                                reader.GetString(1)//"value"
                                ));
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

        private static string getVtansivity(string value)
        {

            if (string.ReferenceEquals(value, null) || value.Equals("") || value.Equals("Nothing"))
            {
                return "";
            }

            switch (value)
            {

                case "dovajhi":
                    return "Causative/Anticausative";

                case "inTransitive":
                    return "Intransitive";

                case "transitive":
                    return "Transitive";

            }

            return value;
        }

        private static string getVactivity(string value)
        {

            if (string.ReferenceEquals(value, null) || value.Equals("") || value.Equals("Nothing"))
            {
                return "";
            }

            switch (value)
            {

                case "active":
                    return "Active";

                case "passive":
                    return "Passive";

            }

            return value;
        }

        private static string getVtype(string value)
        {

            if (string.ReferenceEquals(value, null) || value.Equals("") || value.Equals("Nothing"))
            {
                return "";
            }

            switch (value)
            {

                case "auxiliaryVerb":
                    return "Auxiliary";

                case "compoundVerb":
                    return "Complex";

                case "copulaVerb":
                    return "Copula";

                case "pishvandiVerb":
                    return "Phrasal";

                case "simpleVerb":
                    return "Simple";

            }

            return value;
        }

        private static string getCategory(string value)
        {

            if (string.ReferenceEquals(value, null) || value.Equals("") || value.Equals("Nothing"))
            {
                return "";
            }

            switch (value)
            {

                case "category_adad":
                    return "Numeral";

                case "category_Am":
                    return "General";

                case "category_khAs":
                    return "Specific";

                case "category_masdari":
                    return "Infinitival";

                case "category_esmZamir":
                    return "Pronoun";

            }

            return value;
        }

        private static string getGoupOrMokassar(string value)
        {

            if (string.ReferenceEquals(value, null) || value.Equals("") || value.Equals("Nothing"))
            {
                return "";
            }

            switch (value)
            {

                case "am_khas_esmejam":
                    return "MassNoun";

                case "am_khas_jam":
                    return "Regular";

                case "am_khas_mokassar":
                    return "Irregular";

            }

            return value;
        }

        private static string getEsmeZamir(string value)
        {

            if (string.ReferenceEquals(value, null) || value.Equals("") || value.Equals("Nothing"))
            {
                return "";
            }

            switch (value)
            {

                case "moakkad":
                    return "Emphatic";

                case "gheir_moshakhas":
                    return "Indefinite";

                case "motaghabel":
                    return "Reciprocal";

                case "noun_type_morakab":
                    return "";

            }

            return value;
        }

        private static string getAdad(string value)
        {

            if (string.ReferenceEquals(value, null) || value.Equals("") || value.Equals("Nothing"))
            {
                return "";
            }

            switch (value)
            {

                case "asli":
                    return "Cardinal";

                case "tartibi":
                    return "Ordinal";

            }

            return value;
        }

        private static string getAdverbType1(string value)
        {

            if (string.ReferenceEquals(value, null) || value.Equals("") || value.Equals("Nothing"))
            {
                return "";
            }

            switch (value)
            {

                case "morakkab":
                    return "Compound";

                case "moshtagh":
                    return "Derivative";

                case "moshtagh_morakab":
                    return "DerivationalCompound";

                case "saade":
                    return "Simple";

            }

            return value;
        }

        private static string getNormalValue(string value)
        {

            if (string.ReferenceEquals(value, null) || value.Equals("") || value.Equals("Nothing"))
            {
                return "";
            }

            return value;
        }

        private static string getAdverbType2(string value)
        {

            if (string.ReferenceEquals(value, null) || value.Equals("") || value.Equals("Nothing"))
            {
                return "";
            }

            string res = " ";

            switch (value[0])
            {

                case '1':
                    res += "AdjectiveModifying" + ",";
                    break;

                case '0':
                    res += "";
                    break;

                default:
                    res += value[0] + ",";
                    break;
            }

            switch (value[1])
            {

                case '1':
                    res += "AdverbModifying" + ",";
                    break;

                case '0':
                    res += "";
                    break;

                default:
                    res += value[1] + ",";
                    break;
            }

            switch (value[2])
            {

                case '1':
                    res += "VerbModifying" + ",";
                    break;

                case '0':
                    res += "";
                    break;

                default:
                    res += value[2] + ",";
                    break;
            }

            switch (value[3])
            {

                case '1':
                    res += "SentenceModifying" + ",";
                    break;

                case '0':
                    res += "";
                    break;

                default:
                    res += value[3] + ",";
                    break;
            }

            return res.Substring(0, res.Length - 1);
        }

        private static string getAdjPishinVijegi(string value)
        {

            if (string.ReferenceEquals(value, null) || value.Equals("") || value.Equals("Nothing") || value.Equals("No"))
            {
                return "";
            }

            switch (value)
            {

                case "Yes_mobham":
                    return "Indefinite";

                case "Yes_taajobi":
                    return "Exclamatory";

                case "Yes_eshare":
                    return "Demonstrative";

                case "Yes_Nothing":
                    return "Simple";

            }

            return value;
        }

        private static string getAdjType(string value)
        {

            if (string.ReferenceEquals(value, null) || value.Equals("") || value.Equals("Nothing") || string.ReferenceEquals(value, "No"))
            {
                return "";
            }

            switch (value)
            {

                case "bartarin":
                    return "Superlative";

                case "motlagh":
                    return "Absolute";

                case "bartar":
                    return "Comparative";

            }

            return value;
        }

        private static string getNoeKhas(string value)
        {

            if (string.ReferenceEquals(value, null) || value.Equals("") || value.Equals("Nothing") || string.ReferenceEquals(value, "No"))
            {
                return "";
            }

            switch (value)
            {

                case "noe_khas_ensan":
                    return "Human";

                case "noe_khas_heyvan":
                    return "Animal";

                case "noe_khas_makan":
                    return "Place";

                case "noe_khas_zaman":
                    return "Time";

            }

            return value;
        }

        private static string getAdjTypeSademorakkab(string value)
        {

            if (string.ReferenceEquals(value, null) || value.Equals("") || value.Equals("Nothing") || string.ReferenceEquals(value, "No"))
            {
                return "";
            }

            switch (value)
            {

                case "adj_type_morakab":
                    return "Compound";

                case "adj_type_moshtagh":
                    return "Derivative";

                case "adj_type_moshtagh_morakab":
                    return "DerivatinalCompound";

                case "adj_type_saade":
                    return "Simple";

            }

            return value;
        }

        private static string getVGozaraType(string value)
        {

            if (string.ReferenceEquals(value, null) || value.Equals("") || value.Equals("Nothing"))
            {
                return "";
            }

            string res = " ";

            switch (value[0])
            {

                case '1':
                    res += "WithComplement" + ",";
                    break;

                case '0':
                    res += "";
                    break;

                default:
                    res += value[0] + ",";
                    break;
            }

            switch (value[1])
            {

                case '1':
                    res += "WithObject" + ",";
                    break;

                case '0':
                    res += "";
                    break;

                default:
                    res += value[1] + ",";
                    break;
            }

            switch (value[2])
            {

                case '1':
                    res += "WithPredicate" + ",";
                    break;

                case '0':
                    res += "";
                    break;

                default:
                    res += value[2] + ",";
                    break;
            }

            return res.Substring(0, res.Length - 1);
        }

        private static string getNounType(string value)
        {

            if (string.ReferenceEquals(value, null) || value.Equals("") || value.Equals("Nothing") || string.ReferenceEquals(value, "No"))
            {
                return "";
            }

            switch (value)
            {

                case "noun_type_morakab":
                    return "Compound";

                case "noun_type_moshtagh":
                    return "Derivative";

                case "noun_type_moshtagh_morakab":
                    return "DerivatinalCompound";

                case "noun_type_saade":
                    return "Simple";

                case "noun_type_ebarat":
                    return "Phrasal";

            }

            return value;
        }

        private static String RelationValue(SenseRelationType type)
        {

            if (type.ToString() == "Derivationally_related_form")
            {

                return "Derivationally related form";
            }

            return type.ToString().Replace("_", "-");
        }

        private static SenseRelationType ReverseRelationType(SenseRelationType type)
        {

            //Refer_to...Is_Referred_by
            if (SenseRelationType.Refer_to == type)
            {

                return SenseRelationType.Is_Referred_by;
            }

            if (SenseRelationType.Is_Referred_by == type)
            {

                return SenseRelationType.Refer_to;
            }

            //Verbal_Part..Is_Verbal_Part_of
            if (SenseRelationType.Verbal_Part == type)
            {

                return SenseRelationType.Is_Verbal_Part_of;
            }

            if (SenseRelationType.Is_Verbal_Part_of == type)
            {

                return SenseRelationType.Verbal_Part;
            }

            //Is_Non_Verbal_Part_of..Non_Verbal_Part
            if (SenseRelationType.Is_Non_Verbal_Part_of == type)
            {

                return SenseRelationType.Non_Verbal_Part;
            }

            if (SenseRelationType.Non_Verbal_Part == type)
            {

                return SenseRelationType.Is_Non_Verbal_Part_of;
            }

            return type;
        }

        private static String ReverseSRelationType(String type)
        {

            //Refer_to...Is_Referred_by
            if (type.Equals("Refer-to"))
            {

                return "Is-Referred-by";
            }

            if (type.Equals("Is-Referred-by"))
            {

                return "Refer-to";
            }

            //Verbal_Part..Is_Verbal_Part_of
            if (type.Equals("Verbal-Part"))
            {

                return "Is-Verbal-Part-of";
            }

            if (type.Equals("Is-Verbal-Part-of"))
            {

                return "Verbal-Part";
            }

            //Is_Non_Verbal_Part_of..Non_Verbal_Part
            if (type.Equals("Non-Verbal-Part"))
            {

                return "Is-Non-Verbal-Part-of";
            }

            if (type.Equals("Is-Non-Verbal-Part-of"))
            {

                return "Non-Verbal-Part";
            }

            return type;
        }

    }
}