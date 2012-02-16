/* Author: Pratik Pramanik
 * Written for SCVMC LIS
 * contact: pratikpramanik at gmail dot com */
using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

//stuff i added
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public partial class Admin_Indexer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    /* Author: Pratik Pramanik
     * Written for SCVMC LIS
     * contact: pratikpramanik at gmail dot com
     *
     * Indexes the items on a database for broader, less specific 
     * searching of the website.
     */
    protected void IndexDocs(object sender, EventArgs e)
    {
        //status prompt
        IndexStatus.Text = "Indexing..." + System.Environment.NewLine;

        //acess DB with SELECT -> Gives DataView -> Convert View to Table
        DataView dvSql  = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
        DataTable dtSql = dvSql.ToTable();

        //delimination symbols
        char[] delimCh =  { ' ', ',', '.', ':', '\t', '_', '\n', 
                            '!','@','#','$','%','^','&','*','(',
                            ')','[',']',';','\'','{','}','\"',
                            '<','>','\\','/','+','-','`','\r',
                            '~'};

        //hashtable format <String "Keyword", List<Forms> that contain "Keyword">
        Hashtable wordmap    = new Hashtable();
        int dbSize           = dvSql.Count;
        const int DEF_ARR_SZ = 32;
        const int TITLESCORE = 3;
        const int SYNYMSCORE = 2;
        const int NUM_FIELDS = 8;

        IndexStatus.Text += "Processing DB [Active Size:"+ dbSize +"]..." + System.Environment.NewLine;
        

        #region DB Processing
        //LOGIC: for every entry in the database, add that entry's value for ID into the list/array
        foreach (DataRow r in dtSql.Rows)
        {
            FullForm temp = new FullForm();
            String[] fields = new String[NUM_FIELDS];
            int currentscore;
            
            //Store current DATAROW in a Struct for easy access
            temp.ID    = (int)r["ID"];
            fields[0] = temp.Title = (String)r["Title"];
            fields[1] = temp.Code = (String)r["Code"];
            fields[2] = temp.Desc = (String)r["Description"];
            fields[3] = temp.Syn1 = (String)r["Syn1"];
            fields[4] = temp.Syn2 = (String)r["Syn2"];
            fields[5] = temp.Syn3 = (String)r["Syn3"];
            fields[6] = temp.Syn4 = (String)r["Syn4"];
            fields[7] = temp.Syn5 = (String)r["Syn5"];

            IndexStatus.Text += "*DB Row Processing: " + temp.Title + "...";

            //parse through text fields
            for (int f = 0; f < NUM_FIELDS; f++)
            {
                String[] text;
                if (f != 1)                               //split the field, if not code
                { text = fields[f].Split(delimCh); }
                else
                { text = new String[1]; text[0] = fields[f]; }
                int n = 0;                                //set a counter

                //assign special scores
                if (f == 0)      { currentscore = TITLESCORE; }
                else if (f >= 3) { currentscore = SYNYMSCORE; }
                else             { currentscore = 1; }

                #region Word Processing
                //check each word in each field
                foreach (String s in text)
                {
                    string newS = s.ToLower();

                    //ignore stopwords
                    if(s.Equals("") || isStopword(newS))
                      continue;

                    //Hashtable key check, has keyword already
                    if (wordmap.ContainsKey(newS))
                    {
                        int x;
                        string[] oldArr = (string[])wordmap[newS];

                        //checks keyword already found on particular form
                        string tempName = temp.FormatName();
                        string tempID   = temp.ID.ToString();
                        
                        //check keyword's current DB
                        for (x = 0; x < oldArr.Length; x++)
                        {
                            //end of data in array
                            if (oldArr[x] == null)
                                goto EMPTYSPACEARRAY;
                            if (oldArr[x].Equals(""))
                                goto EMPTYSPACEARRAY;

                            int oldScore = Convert.ToInt32(oldArr[x].Split(':')[0]);
                            string oldID    = oldArr[x].Split(':')[1];
                            string oldName  = oldArr[x].Split(':')[2];
                            //string oldNext = oldArr[x].Split(':')[3]; //vestigal

                            //ID shared, repeated word on page, increase relevance rating
                            if (tempID.Equals(oldArr[x].Split(':')[1]))
                            {
                                oldScore += currentscore;
                                wordmap.Remove(newS);
                                oldArr[x] = FormatForm(oldScore,
                                                       (int)Convert.ToInt32(oldID),
                                                       oldName);
                                wordmap.Add(newS, oldArr);
                                goto ENDFOREACH;
                            }
                        }
                    ARRAYTOOSMALL: //**reached the end of the array size, needs to be bigger
                        Array.Resize<string>(ref oldArr, oldArr.Length + 1);
                        
                        //x should already be the index of the last space
                    EMPTYSPACEARRAY: //**insert form data at next space in array
                        oldArr[x] = FormatForm(currentscore, temp.ID, temp.FormatName());
                        wordmap.Remove(newS);
                        wordmap.Add(newS, oldArr);
                    }
                    //does not contain key, add new 
                    else
                    {
                        string[] formArr = new string[DEF_ARR_SZ]; //set back to dbSize if too small
                        //assign score, place in array
                        formArr[0] = FormatForm(currentscore, temp.ID, temp.FormatName());
                        wordmap.Add(newS, formArr);
                    }
                ENDFOREACH: //**skipped to from for statment
                    n++;
                }
                #endregion
            }
            IndexStatus.Text += " Complete!" + System.Environment.NewLine;
        }
            #endregion

        //sort every array! for quicker lookup process

        #region Write Data File
        IndexStatus.Text += "Writing to file...";
        //open filestream
        FileStream fs = new FileStream
            (Server.MapPath("~/index.dat"), FileMode.OpenOrCreate, FileAccess.Write);

        //write to file close filestream
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, wordmap);
        }
        finally
        {
            fs.Close();
        }
        //need a catch?
        #endregion

        //shows only if successful
        IndexStatus.Text += "Done!" + System.Environment.NewLine; 
    }

    /*
     * Author: Pratik Pramanik
     * Universal usage to format a searched form
     */
    protected string FormatForm(int score, int ID, string name)
    {
        return score + ":" + ID + ":" + name;
    }

    /*
     * Author: Pratik Pramanik
     * Stopwords aquired from:
     * http://armandbrahaj.blog.al/2009/04/14/list-of-english-stop-words/
     */
    protected bool isStopword(string s)
    {
        #region BIG LIST OF WORDS
        string[] stopwords = {"a", "about", "above", "above", "across", "after", "afterwards", 
        "again", "against", "all", "almost", "alone", "along", "already", "also","although",
        "always","am","among", "amongst", "amoungst", "amount",  "an", "and", "another", "any",
        "anyhow","anyone","anything","anyway", "anywhere", "are", "around", "as",  "at", "back",
        "be","became", "because","become","becomes", "becoming", "been", "before", "beforehand", 
        "behind", "being", "below", "beside", "besides", "between", "beyond", "bill", "both", 
        "bottom","but", "by", "call", "can", "cannot", "cant", "co", "con", "could", "couldnt", 
        "cry", "de", "describe", "detail", "do", "done", "down", "due", "during", "each", "eg", 
        "eight", "either", "eleven","else", "elsewhere", "empty", "enough", "etc", "even", "ever", 
        "every", "everyone", "everything", "everywhere", "except", "few", "fifteen", "fify", 
        "fill", "find", "fire", "first", "five", "for", "former", "formerly", "forty", "found", 
        "four", "from", "front", "full", "further", "get", "give", "go", "had", "has", "hasnt", 
        "have", "he", "hence", "her", "here", "hereafter", "hereby", "herein", "hereupon", "hers", 
        "herself", "him", "himself", "his", "how", "however", "hundred", "ie", "if", "in", "inc", 
        "indeed", "interest", "into", "is", "it", "its", "itself", "keep", "last", "latter", 
        "latterly", "least", "less", "ltd", "made", "many", "may", "me", "meanwhile", "might", 
        "mill", "mine", "more", "moreover", "most", "mostly", "move", "much", "must", "my", 
        "myself", "name", "namely", "neither", "never", "nevertheless", "next", "nine", "no", 
        "nobody", "none", "noone", "nor", "not", "nothing", "now", "nowhere", "of", "off", 
        "often", "on", "once", "one", "only", "onto", "or", "other", "others", "otherwise", 
        "our", "ours", "ourselves", "out", "over", "own","part", "per", "perhaps", "please", 
        "put", "rather", "re", "same", "see", "seem", "seemed", "seeming", "seems", "serious", 
        "several", "she", "should", "show", "side", "since", "sincere", "six", "sixty", "so", 
        "some", "somehow", "someone", "something", "sometime", "sometimes", "somewhere", "still", 
        "such", "system", "take", "ten", "than", "that", "the", "their", "them", "themselves", 
        "then", "thence", "there", "thereafter", "thereby", "therefore", "therein", "thereupon", 
        "these", "they", "thick", "thin", "third", "this", "those", "though", "three", "through", 
        "throughout", "thru", "thus", "to", "together", "too", "top", "toward", "towards", "twelve", 
        "twenty", "two", "un", "under", "until", "up", "upon", "us", "very", "via", "was", "we", 
        "well", "were", "what", "whatever", "when", "whence", "whenever", "where", "whereafter", 
        "whereas", "whereby", "wherein", "whereupon", "wherever", "whether", "which", "while", 
        "whither", "who", "whoever", "whole", "whom", "whose", "why", "will", "with", "within", 
        "without", "would", "yet", "you", "your", "yours", "yourself", "yourselves"};
        #endregion

        for (int n = 0; n < stopwords.Length; n++)
        { if (s.Equals(stopwords[n])) { return true; } }
        return false;
    }
}

struct FullForm
{
    public int ID;
    public String Title, Code, Desc, Syn1, Syn2, Syn3, Syn4, Syn5;

    public string FormatName()
    {
        return Title + " [" + Code + "]";
    }
}
//String {score}:ID:formattedname