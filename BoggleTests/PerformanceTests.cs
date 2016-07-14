///////////////////////////////////////////////////////////////////////////////
// File: PerformanceTests.cs
// Author: Ramón Zarazúa B. (ramon@ztktech.com)
// Date: Jul/12/2016
// Description: Tests that measure performance of the boggle solver.
///////////////////////////////////////////////////////////////////////////////
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BoggleManaged;

namespace BoggleTests
{
    [TestClass]
    public class PerformanceTests
    {

        const string largeDictionary = "Dictionaries\\EnglishDictionary1.txt";
        static readonly Dictionary<char, uint> testTileInfoLargeSet
            = new Dictionary<char, uint>
            {
                ['a'] = 2,
                ['b'] = 2,
                ['c'] = 2,
                ['d'] = 2,
                ['e'] = 2,
                ['f'] = 2,
                ['g'] = 2,
                ['h'] = 2,
                ['i'] = 2,
                ['j'] = 2,
                ['k'] = 2,
                ['l'] = 2,
                ['m'] = 2,
                ['n'] = 2,
                ['o'] = 2,
                ['p'] = 2,
                ['q'] = 2,
                ['r'] = 2,
                ['s'] = 2,
                ['t'] = 2,
                ['u'] = 2,
                ['v'] = 2,
                ['w'] = 2,
                ['x'] = 2,
                ['y'] = 2,
                ['z'] = 2
            };
        const uint largeDictionaryWordCount = 273193U;

        [TestMethod]
        [TestCategory("Performance")]
        [DeploymentItem(largeDictionary, "Dictionaries")]
        [Timeout(10 * 1000)]
        public void LoadLargeDictionary()
        {
            DictionaryTree d = new DictionaryTree(largeDictionary, testTileInfoLargeSet, 1, false);

            Assert.AreEqual(d.WordCount, largeDictionaryWordCount, "The dictionary did not read the expected ammount of words");
        }

        const string minimalDict = "Dictionaries\\MinimalDict.txt";

        [TestMethod]
        [TestCategory("Performance")]
        [DeploymentItem(minimalDict,"Dictionaries")]
        [Timeout(1000)]
        public void SolveMaxSizeBoardWithSmallDictionary()
        {
            char[] board = {
                't', 'M', 'm', 'm', 'M', 't', 't', 't',
                't', 'm', 'r', 't', 'a', 'r', 'r', 'm',
                'm', 'M', 'a', 't', 'a', 't', 'M', 't',
                'M', 'r', 'r', 'm', 'r', 'a', 't', 'r',
                'M', 'r', 't', 'm', 'M', 'M', 'M', 'm',
                'a', 'a', 't', 'm', 'M', 't', 't', 'm',
                'M', 'm', 'm', 'r', 'M', 't', 'r', 't',
                't', 'm', 'a', 'm', 'M', 'm', 'M', 'm'
            };

            string[] solutions = {
                "am", "arm", "art", "at", "mart", "Mat", "mt", "ra", "raam", "ram", "rat", "tar", "tram"};

            // Maybe run the tree first, then save it in the testcontext, and then solve a large board?
            Boggle b = new Boggle(8, 8, board);
            var testSolutions = from solution in b.Solve(minimalDict, 1, true) orderby solution ascending select solution;

            Helpers.AssertEnumeratorsAreEqual(solutions, testSolutions, "The proper solution for the boggle board was not found.");
        }

        [TestMethod]
        [TestCategory("Performance")]
        [DeploymentItem(largeDictionary,"Dictionaries")]
        [Timeout(1000)]
        public void SolveMaxSizeBoardWithLargeDictionary()
        {
            char[] board = {
                'k', 'i', 'r', 'o', 'e', 'a', 'd', 'e',
                'a', 's', 'y', 'i', 'y', 's', 'n', 'i',
                'g', 'o', 'q', 'o', 'i', 'j', 'i', 'g',
                'n', 'm', 'i', 'a', 'b', 'l', 'a', 'i',
                'w', 'e', 's', 'e', 'b', 'h', 'e', 'f',
                'y', 'a', 't', 'h', 'w', 'i', 'm', 'u',
                'i', 'k', 'a', 'p', 'e', 'k', 'c', 'a',
                'q', 'k', 'o', 's', 'z', 'i', 'q', 'o',
            };

            string[] solutions = {
            #region Solutions
                "a", "aa", "aah", "aahs", "aas", "ab", "abb", "abbe",
                "abbes", "abbest", "abet", "abets", "abime", "abl", "able", "ablins",
            "abo", "aboil", "ac", "acies", "ack", "acme", "ad", "ade",
            "aden", "adenia", "ads", "ae", "aes", "aet", "af", "ag",
            "agile", "agin", "agnosy", "ago", "agon", "agone", "agones", "agos",
            "ah", "ahem", "ahi", "ahs", "ai", "aiblins", "aiel", "aik",
            "ail", "aile", "ailing", "aim", "aims", "ain", "aine", "ains",
            "aion", "air", "airs", "airsome", "airy", "ais", "ak", "aka",
            "akasa", "akia", "ako", "al", "alb", "alba", "albas", "albe",
            "ale", "alef", "alem", "alia", "alias", "align", "aligned", "aligns",
            "alii", "alin", "aline", "alined", "aliya", "aliyas", "am", "ame",
            "amel", "amelia", "ami", "amic", "amici", "amie", "amies", "amu",
            "amuck", "an", "and", "ands", "ane", "anga", "angia", "angili",
            "ani", "anil", "anile", "anis", "anise", "anisil", "ans", "ansi",
            "aob", "ap", "ape", "apes", "aph", "apheta", "apse", "apt",
            "apts", "aq", "as", "asak", "ase", "asea", "asem", "asemia",
            "asg", "ash", "ashes", "ashet", "asia", "asialia", "asimen", "ask",
            "asok", "asoka", "asop", "asp", "ast", "asta", "astay", "at",
            "atap", "ate", "ates", "atheism", "auf", "aum", "aw", "awe",
            "awes", "awest", "awm", "awn", "ay", "aye", "ayen", "ayes",
            "ays", "b", "ba", "bab", "babe", "babes", "babies", "babis",
            "bablah", "bable", "bae", "bai", "bail", "baile", "bailing", "bas",
            "basat", "base", "bash", "bashes", "basion", "bast", "basta", "baste",
            "basten", "bb", "bbl", "be", "beast", "bebaste", "beblain", "behap",
            "behew", "beisa", "bes", "besa", "besaile", "beset", "besew", "best",
            "bestay", "bet", "beta", "betas", "bete", "betes", "beth", "beths",
            "bets", "bewept", "bhakta", "bhaktas", "bhat", "bhp", "bi", "bias",
            "bick", "bija", "bike", "bikes", "bikie", "bilbi", "bilbie", "bilbies",
            "bile", "bim", "bima", "bio", "bis", "bise", "bl", "blae",
            "blah", "blain", "blains", "blea", "bleu", "blibe", "blin", "blind",
            "blinds", "blini", "bo", "boa", "boas", "boast", "boil", "boiling",
            "boise", "boist", "boy", "boyang", "boyo", "boyos", "boys", "c",
            "ca", "cam", "came", "camel", "camelia", "camelias", "cameline", "cauf",
            "caum", "cie", "cima", "cimelia", "cize", "ck", "ckw", "cm",
            "co", "cq", "cu", "cue", "cum", "cumhal", "d", "da",
            "dae", "dan", "dane", "dang", "dansy", "das", "dase", "dasi",
            "day", "days", "de", "dei", "deign", "deigns", "den", "denay",
            "denial", "dens", "dense", "di", "die", "dig", "digne", "din",
            "dine", "ding", "dins", "dn", "ds", "e", "ea", "ead",
            "eadi", "ean", "ease", "easing", "east", "easy", "eat", "eath",
            "eats", "ebb", "eblis", "ed", "eds", "ef", "eh", "eigne",
            "eir", "eiry", "el", "ela", "elain", "elaine", "elains", "elb",
            "elhi", "eli", "elijah", "em", "emf", "emic", "emong", "ems",
            "emu", "en", "end", "ends", "eng", "engs", "enjail", "enmist",
            "ens", "ense", "ensile", "eo", "ep", "epa", "epha", "ephas",
            "ephebi", "ephebic", "epheboi", "ephetae", "ephete", "epos", "es", "esd",
            "ese", "esne", "esp", "est", "et", "eta", "etape", "etapes",
            "etas", "eten", "eth", "eths", "eu", "ew", "ewe", "ewes",
            "ewest", "ey", "eyas", "f", "fa", "fae", "fag", "fagin",
            "fagine", "fagins", "fail", "fain", "fains", "faisan", "fe", "feal",
            "feh", "fei", "feign", "feigned", "feigns", "feline", "felis", "fem",
            "femic", "feu", "fi", "fie", "fiel", "fig", "fm", "fu",
            "fuci", "fuck", "fuel", "fueling", "fum", "fume", "g", "ga",
            "gae", "gael", "gail", "gain", "gaine", "gained", "gains", "gainsay",
            "gair", "gaj", "gal", "galbe", "gale", "gali", "gas", "gi",
            "gid", "gids", "gie", "gied", "gien", "gif", "gila", "gile",
            "gilia", "gin", "gins", "gis", "gise", "gm", "gn", "gnide",
            "gnome", "gnomes", "gnomish", "gnomist", "gns", "go", "goa", "goas",
            "goi", "gois", "gome", "gon", "gone", "goney", "gos", "goy",
            "goys", "gs", "h", "ha", "haak", "hae", "haem", "haemic",
            "haf", "hag", "hagi", "hail", "hain", "haine", "hained", "haj",
            "haji", "hajib", "hajis", "hak", "hako", "hale", "haling", "hao",
            "hap", "haps", "hapten", "has", "hasp", "hat", "hate", "hateable",
            "hates", "hats", "hb", "he", "heaf", "heal", "healing", "hei",
            "heii", "heist", "hel", "helbeh", "heliast", "heling", "helio", "hem",
            "hemic", "hep", "hes", "hest", "het", "hete", "hew", "hewe",
            "hi", "hibla", "hic", "hick", "hie", "hies", "hike", "hikes",
            "him", "hl", "hm", "hp", "hs", "hsien", "ht", "hts",
            "hw", "i", "ia", "iago", "iao", "ib", "iba", "ic",
            "id", "ide", "ids", "ie", "if", "ife", "ign", "ignis",
            "ii", "iiasa", "iii", "ik", "ikat", "ike", "il", "ile",
            "ilea", "ileum", "ilia", "iliahi", "im", "imu", "in", "ind",
            "inde", "ing", "inia", "inial", "ins", "insea", "io", "ion",
            "ios", "iq", "iqs", "ir", "iris", "irs", "is", "isagon",
            "isdn", "ise", "ish", "ising", "ism", "isn", "isnad", "iso",
            "ist", "isth", "iw", "iyo", "j", "ja", "jag", "jah",
            "jai", "jail", "jain", "jiao", "jib", "jibb", "jibba", "jibbeh",
            "jibe", "jibes", "jiboa", "jiboya", "jig", "jin", "jina", "jing",
            "jingal", "jins", "jnd", "js", "k", "ka", "kaas", "kae",
            "kaes", "kago", "kagos", "kai", "kaik", "kaka", "kakas", "kaki",
            "kaon", "kaph", "kaphs", "kapok", "kas", "kasa", "kasha", "kashas",
            "kasm", "kat", "kath", "katha", "kathak", "kats", "kaw", "kay",
            "kc", "kep", "keps", "kept", "ki", "kiaat", "kibbeh", "kibble",
            "kibbling", "kibe", "kibei", "kibes", "kibla", "kiblah", "kie", "kief",
            "kiel", "kielbasa", "kielbasi", "kim", "kiri", "kiyas", "kiyi", "km",
            "kmel", "ko", "koa", "koas", "kokia", "kop", "kopek", "koph",
            "kophs", "kops", "kos", "ksi", "kt", "kw", "ky", "kyat",
            "kyats", "kye", "l", "la", "lag", "lagna", "lah", "lai",
            "lain", "laine", "lb", "lbw", "le", "lea", "leaf", "lei",
            "leu", "leuco", "leuma", "lh", "lhb", "li", "liaise", "lias",
            "lib", "libbet", "lig", "ligne", "liin", "lija", "lin", "lina",
            "lind", "linda", "line", "lined", "ling", "linga", "linie", "linja",
            "lins", "linsey", "lis", "m", "ma", "mac", "macies", "mack",
            "maco", "mao", "mau", "mc", "me", "mea", "meak", "meal",
            "mealing", "meas", "mease", "meat", "meath", "meathe", "meats", "meaw",
            "meio", "mel", "mela", "melba", "meline", "melis", "men", "meng",
            "meno", "mesa", "mesail", "mese", "mesh", "meshes", "mesion", "mest",
            "met", "meta", "metaph", "metas", "mete", "metes", "meth", "meths",
            "mets", "meu", "mew", "mf", "mg", "mh", "mi", "mia",
            "miae", "mias", "mib", "mica", "mick", "mickies", "mico", "mien",
            "mike", "mikes", "mikie", "mis", "misate", "mise", "miseat", "mishap",
            "mishaps", "mist", "mk", "mn", "mo", "moa", "moas", "mog",
            "mogs", "moi", "moiest", "moio", "moise", "moist", "moisten", "mon",
            "mone", "monesia", "monest", "moneth", "money", "mong", "mos", "mosk",
            "moy", "moyo", "ms", "mu", "muck", "mw", "n", "na",
            "nad", "nae", "nasi", "nasial", "nay", "nays", "nd", "ne",
            "nea", "neat", "neath", "neats", "nei", "neist", "nemo", "nemos",
            "nese", "nesh", "nest", "net", "nete", "neth", "nets", "new",
            "ng", "ngai", "ni", "nid", "nide", "nig", "nil", "nile",
            "nis", "nisei", "nisi", "nj", "nm", "no", "noa", "nog",
            "nogs", "noise", "nom", "nome", "nomes", "noms", "nos", "nosy",
            "noy", "ns", "o", "oak", "oaks", "oaky", "oam", "oast",
            "oat", "oaten", "oath", "oaths", "oats", "ob", "oba", "obb",
            "obe", "obeish", "obeism", "obes", "obese", "obi", "obia", "obias",
            "obis", "obj", "obl", "obli", "oc", "oca", "ock", "oe",
            "oes", "og", "oie", "oii", "oil", "oiling", "oime", "oisin",
            "ok", "oka", "okas", "okay", "oki", "okia", "okta", "om",
            "omen", "omnes", "oms", "on", "one", "oneism", "ones", "onethe",
            "op", "opa", "opah", "opahs", "ope", "opes", "ops", "opt",
            "opts", "or", "orison", "oriya", "ors", "ory", "os", "osaka",
            "ose", "osi", "oy", "oyes", "p", "pa", "paas", "pah",
            "pas", "pase", "paso", "pat", "pata", "patas", "pate", "paten",
            "pates", "patesi", "path", "paths", "pats", "pe", "peh", "pes",
            "pesa", "peso", "pew", "ph", "phase", "phat", "phew", "pht",
            "po", "poa", "poky", "pos", "pose", "ps", "pt", "pta",
            "pte", "pts", "q", "qiyas", "qm", "qs", "qy", "r",
            "ria", "rie", "ries", "rio", "risk", "ro", "roe", "roes",
            "roey", "roi", "rs", "s", "sa", "saa", "sab", "sabe",
            "sabicu", "sable", "sad", "sade", "sadi", "sae", "saeta", "sag",
            "sago", "sah", "sai", "saibling", "sail", "sailing", "saim", "sair",
            "sairy", "sakai", "saki", "sakkos", "san", "sand", "sane", "saned",
            "sang", "sanga", "sangah", "sangil", "sao", "sap", "sapek", "sapo",
            "sat", "sata", "satai", "sate", "satem", "sates", "saw", "sawmon",
            "sawn", "sawney", "say", "sd", "sdeign", "se", "sea", "seak",
            "seat", "seathe", "sei", "sem", "semi", "sen", "seor", "sep",
            "sept", "septa", "septaemia", "septemia", "septs", "set", "seta", "setae",
            "seth", "sew", "sewn", "sey", "sg", "sh", "sha", "shako",
            "shakos", "shakta", "shaky", "shap", "shape", "shapes", "shaps", "shat",
            "she", "shea", "sheik", "shes", "shew", "shp", "shpt", "sht",
            "shwebo", "si", "siafu", "siak", "sial", "sib", "sibb", "sibling",
            "sie", "sig", "sign", "signa", "signed", "sika", "sil", "sile",
            "sim", "sime", "simon", "sin", "sina", "sind", "sine", "sing",
            "sion", "sir", "sk", "skag", "skair", "ski", "sm", "smeath",
            "smeth", "smethe", "smew", "smog", "smogs", "sn", "sned", "snide",
            "snig", "so", "soak", "soaky", "soap", "sog", "sok", "soka",
            "some", "somet", "someway", "somne", "son", "sone", "sones", "song",
            "sop", "sope", "soph", "sophs", "sophta", "soy", "sp", "spa",
            "spak", "spat", "spate", "spates", "spath", "spathe", "spathes", "spats",
            "spew", "spoky", "spt", "sq", "sr", "sri", "st", "sta",
            "stap", "stapes", "staph", "staw", "stawn", "stay", "steak", "stem",
            "sten", "steng", "steno", "stenog", "stenos", "stew", "stewy", "stey",
            "stk", "sye", "syr", "syria", "t", "ta", "taa", "tae",
            "taen", "tai", "taka", "takahe", "takahes", "taky", "tao", "taos",
            "tap", "tape", "tapes", "taps", "tas", "tash", "taw", "tawn",
            "tawney", "tay", "te", "tea", "teaboy", "teaish", "teaism", "teak",
            "teas", "teasable", "tease", "teaseable", "teise", "tem", "temse", "ten",
            "teng", "tew", "th", "tha", "thak", "thapes", "the", "theb",
            "thebaism", "theism", "these", "thew", "tk", "tp", "tph", "tps",
            "ts", "tsi", "tsia", "u", "uc", "um", "ume", "w",
            "wa", "wae", "waes", "waik", "waka", "wakas", "waky", "was",
            "wasabi", "wase", "wash", "washes", "wast", "waste", "wat", "watap",
            "watape", "watapeh", "watapes", "wataps", "wath", "wats", "way", "wb",
            "we", "weak", "weaky", "web", "weism", "weki", "wem", "wen",
            "wept", "wese", "west", "weste", "wet", "weta", "wets", "wey",
            "wh", "wha", "whale", "whaling", "whap", "whaps", "whase", "what",
            "whata", "whats", "wheal", "whealing", "wheki", "whesten", "whet", "whets",
            "whiba", "whick", "whim", "whs", "whse", "wi", "wibble", "wick",
            "wim", "wime", "wk", "wm", "wy", "wye", "wyes", "y",
            "ya", "yad", "yade", "yak", "yakka", "yan", "yang", "yas",
            "yat", "yate", "yaw", "yawn", "ye", "yea", "yean", "yeaned",
            "yeans", "yeas", "yeast", "yeat", "yen", "yeo", "yes", "yese",
            "yest", "yet", "yeta", "yeth", "yew", "yi", "yis", "yo",
            "yob", "yobi", "yoe", "yoga", "yogas", "yoi", "yom", "yon",
            "yor", "yoy", "yoyo", "yr", "yrs", "ys", "z", "zek",
            "zep", "zs"
            };
            #endregion

            // Maybe run the tree first, then save it in the testcontext, and then solve a large board?
            Boggle b = new Boggle(8, 8, board);
            var testSolutions = from solution in b.Solve(largeDictionary, 1, true) orderby solution ascending select solution;

            Helpers.AssertEnumeratorsAreEqual(solutions, testSolutions, "The proper solution for the boggle board was not found.");
        }
    }
}
